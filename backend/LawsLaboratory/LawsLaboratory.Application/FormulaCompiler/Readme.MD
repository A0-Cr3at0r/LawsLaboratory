# FormulaCompiler

The `FormulaCompiler` subsystem transforms a formula written in the Laws Laboratory formula language into a `CompiledExpression` that can be evaluated by a calculation engine.

The compiler separates the different concerns involved in transforming a formula:

```text
Source Formula
      │
      ▼
   Lexer
      │
      ▼
   Tokens
      │
      ▼
   Parser
      │
      ▼
  Syntax AST
      │
      ▼
Semantic Analyzer
      │
      ▼
Scientific AST
      │
      ├──────────────────────────────┐
      │                              │
      │ optimize = false             │ optimize = true
      │                              │
      │                         FormulaOptimizer
      │                              │
      └──────────────┬───────────────┘
                     ▼
             CompiledGenerator
                     │
                     ▼
            CompiledExpression
```

## Responsibilities

Each stage has a distinct responsibility.

### Lexer

`FormulaLexer` performs lexical analysis.

It converts the source string into a sequence of `FormulaToken` instances.

It recognizes:

* numeric literals;
* identifiers;
* arithmetic operators;
* logical operators;
* parentheses;
* relative-position brackets;
* function argument separators.

The lexer does not determine the meaning of identifiers.

For example, an identifier such as `temperature` is initially represented as an `Identifier` token. Its interpretation as a parameter, symbol, or invalid identifier is performed later by the semantic analyzer.

### Parser

`FormulaParser` performs syntactic analysis using recursive descent parsing.

It transforms the token sequence into a syntax tree while enforcing the grammar and operator precedence of the formula language.

The parser is deliberately independent from the parameter registry and does not determine whether an identifier represents an actual simulation parameter.

## Formula Grammar

The parser is based on a Context-Free Grammar (CFG) implemented through recursive descent parsing.

The following grammar defines the syntactic structure and operator precedence of the formula language.

### Non-terminal symbols

The grammar uses the following non-terminal symbols:

```text
Expression
LogicalOr
LogicalXor
LogicalAnd
Addition
Multiplication
Power
Unary
Primary
FunctionCall
```

These symbols exist only during syntactic analysis. They are not preserved in the resulting AST.

### Grammar rules

```text
Expression
    → LogicalOr

LogicalOr
    → LogicalXor
      ( "||" LogicalXor )*

LogicalXor
    → LogicalAnd
      ( "xor" LogicalAnd )*

LogicalAnd
    → Addition
      ( "&&" Addition )*

Addition
    → Multiplication
      ( ("+" | "-") Multiplication )*

Multiplication
    → Power
      ( ("*" | "/") Power )*

Power
    → Unary
      ( "^" Unary )*

Unary
    → "-" Unary
    → "!" Unary
    → Primary

Primary
    → Number
    → Identifier
    → FunctionCall
    → "(" Expression ")"

FunctionCall
    → Identifier "(" Arguments ")"

Arguments
    → Expression
      ( "," Expression )*
    → ε
```

The grammar establishes the following precedence, from lowest to highest:

```text
|| 
xor
&&
+ -
* /
^
unary - !
primary expressions
```

Power is right-associative, while the binary operator levels above it are parsed left-to-right.

### Relative parameter positions

An identifier may optionally be followed by a relative position:

```text
Identifier
    → Identifier "[" SignedInteger "," SignedInteger "]"
```

For example:

```text
temperature
temperature[1,0]
temperature[-1,0]
temperature[0,1]
```

When no position is specified, `[0,0]` is used.

The relative position is retained in the syntax tree and later associated with the resolved parameter by the semantic analyzer.

### Example

The formula:

```text
2 + 3 * x && !enabled
```

is parsed according to operator precedence as:

```text
              AND
             /   \
           (+)    !
          /  \     \
         2    *   enabled
             / \
            3   x
```

The intermediate grammar productions such as `Addition`, `Multiplication`, and `LogicalAnd` are not represented as AST node types. They only control how the final syntax tree is constructed.

The resulting syntax tree contains only domain-independent syntax nodes:

```text
ConstantNode
IdentifierNode
OperatorNode
FunctionCallNode
```

The interpretation of these nodes is performed by the semantic analyzer.


### Semantic Analyzer

`SemanticAnalyzer` gives domain meaning to the syntax tree.

It resolves:

* parameter identifiers through `IParameterRegistry`;
* symbolic constants such as `pi` and `e`;
* supported function names;
* function argument counts.

It transforms syntax-level nodes into the scientific expression model used by the rest of the application.

For example:

```text
IdentifierNode("temperature")
        ↓
VariableNode(ParameterId, [0,0])
```

and:

```text
FunctionCallNode("sin", ...)
        ↓
OperatorNode(Sin, ...)
```

Unknown parameters, symbols, functions, or invalid function arities result in a semantic error.

## Supported operators

The formula language supports arithmetic, mathematical, logical, and rounding operators.

### Arithmetic

```text
+    Addition
-    Subtraction
*    Multiplication
/    Division
^    Power
```

### Mathematical functions

```text
sin(x)
cos(x)
sqrt(x)
ln(x)
log(x)
```

`log(x, b)` represents the logarithm of `x` in base `b`.

### Logical operators

```text
&&   And
||   Or
!    Not
xor  Xor
```

Logical operators are part of the formula language itself and are recognized lexically before semantic analysis.

### Rounding functions

```text
floor(x)
ceil(x)
```

## Symbols

The language currently provides two symbolic constants:

```text
pi
e
```

They are resolved by the semantic analyzer into the corresponding `SymbolType` values.

## Relative parameter positions

Parameter references may specify a position relative to the cell currently being evaluated.

For example:

```text
temperature[1,0]
temperature[-1,0]
temperature[0,1]
temperature[0,0]
```

The default position is `[0,0]` when no relative position is specified.

The parser records the position syntactically, while the semantic analyzer associates the identifier with its registered parameter.

This allows formulas to express spatial dependencies without coupling the parser to the simulation grid.

## Optimization

Optimization is **optional**.

The compiler can produce a compiled expression directly from the scientific AST, or it can first pass that tree through `FormulaOptimizer`.

```csharp
Compile(formula, optimize: false)
```

does not perform optimization.

```csharp
Compile(formula, optimize: true)
```

enables the optimizer.

Optimization exists primarily to improve the performance of the internal Laws Laboratory calculation engine. It is not required for a formula to be valid or compilable.

In particular, optimization is not imposed on external calculation engines.

An external engine may have its own evaluation strategy, numerical precision, simplification rules, or optimization mechanisms. Therefore, callers integrating another engine can disable the Laws Laboratory optimizer and receive a representation corresponding more directly to the semantic expression.

The optimizer applies only transformations considered appropriate for the real-valued formula model of the current version.

## Compilation result

After semantic analysis, and optionally optimization, `CompiledGenerator` converts the expression tree into a postfix representation.

For example:

```text
a + b * 2
```

is represented conceptually as:

```text
a
b
2
*
+
```

This representation is stored as a `CompiledExpression`.

The generator does not perform semantic analysis or optimization. Its sole responsibility is to transform the scientific expression tree into the linear representation expected by the calculation layer.

## Design principle

The compiler deliberately separates:

```text
Syntax
  ≠
Meaning
  ≠
Optimization
  ≠
Execution
```

The `FormulaCompiler` therefore prepares an expression for execution but does not execute it.

This separation allows the same formula language and semantic model to be used with different calculation engines while keeping engine-specific execution concerns outside the compiler.
