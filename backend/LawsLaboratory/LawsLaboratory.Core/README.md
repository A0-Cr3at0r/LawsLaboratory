````markdown
# LawsLaboratory.Core

## Purpose

`LawsLaboratory.Core` defines the fundamental concepts, data structures,
and mathematical abstractions of LawsLaboratory.

The assembly describes **what the simulation is**, rather than **how the
simulation is executed**.

It is therefore the conceptual foundation of the system. Higher-level
assemblies may depend on Core, while Core remains independent from
application orchestration, execution pipelines, transport, serialization,
and external interfaces.

---

## Architectural Role

The project can be understood through the following separation:

```text
                         LawsLaboratory.Core
                                  │
          ┌───────────────────────┼───────────────────────┐
          │                       │                       │
       Formula                   Laws              SpatialModel
          │                       │                       │
     Expressions            Initialization           Position
     Compilation              Variation              Spatial state
     References              Transmission
          │                       │
          └───────────────────────┼───────────────────────┘
                                  │
                             Mathematics
                                  │
                    ┌─────────────┴─────────────┐
                    │                           │
              Distributions                  Domains
              Random generators              Geometric
                                             Composite
                                             Discrete
````

The central principle is:

> **Core defines the concepts manipulated by the simulation.
> Application and execution layers define how those concepts are
> orchestrated and executed.**

---

## Main Components

### Formula

The `Formula` namespace represents the expression system used by laws.

Expressions are initially represented as semantic elements such as:

* constants;
* variables;
* operators;
* symbols.

These elements can be transformed into a compiled representation based on
instructions.

A compiled expression contains two important pieces of information:

1. an executable instruction sequence;
2. the variable references required by the expression.

This creates a separation between the semantic representation of a formula
and the representation consumed by execution mechanisms.

```text
ExpressionElement[]
        │
        │ compilation
        ▼
CompiledExpression
        │
        ├── ExpressionInstruction[]
        │
        └── VariableReference[]
```

The execution layer therefore does not need to depend on the structure of
individual expression elements.

---

### Laws

The `Laws` namespace contains the domain-level representation of the rules
that govern the simulation.

A `Law` describes a target parameter and groups the rules associated with
its evolution:

```text
Law
 │
 ├── InitializationRule
 │
 ├── VariationRule
 │
 └── TransmissionRule
```

The three rule types represent distinct semantic concerns.

#### Initialization

`InitializationRule` describes how initial values are generated.

It may specify:

* a distribution for values;
* an optional number of target cells;
* an optional spatial distribution;
* an optional spatial domain.

Spatial initialization deliberately separates two concepts:

**Spatial distribution**

Defines how candidate positions are generated.

**Spatial domain**

Defines where those generated positions are considered valid.

Consequently, a distribution can describe an unconstrained spatial process
while a domain restricts the region in which the initialization is effective.

Conceptually:

```text
SpatialDistribution
        │
        │ generates candidate position
        ▼
      Position
        │
        │ domain constraint
        ▼
   accepted / rejected
```

This distinction allows the same spatial distribution to be combined with
different geometric constraints.

#### Variation

`VariationRule` describes how a parameter evolves according to a compiled
expression.

#### Transmission

`TransmissionRule` describes how values are transmitted spatially.

It contains:

* the original formula;
* its compiled expression;
* the relative destinations involved in the transmission.

The `Law` itself remains a domain abstraction rather than an execution
mechanism. This is intentional: if the internal rule model becomes more
complex in the future, execution layers should not need to depend on those
details.

---

### SpatialModel

`SpatialModel` contains the fundamental spatial representations used by the
simulation.

The spatial model is deliberately independent from the mechanisms used to
traverse or execute the simulation.

For example, a plane position represents a location in the simulation space,
while traversal strategies, buffering, and execution coordination belong to
higher layers.

This separation allows the same spatial concepts to be used by different
execution strategies.

---

### Value

The `Value` namespace defines the value representation used by the simulation.

Values are abstracted through the domain's value model rather than being
represented directly by primitive types everywhere.

This allows the simulation to distinguish concepts such as:

* actual values;
* the absence of a value (`Dead`);
* concrete value implementations.

The abstraction is particularly important for formulas and initialization,
where values are manipulated without requiring higher-level components to
know their concrete representation.

---

## Mathematics

The `Mathematics` namespace contains mathematical abstractions used by the
domain.

It is divided primarily into:

```text
Mathematics
│
├── RandomGenerators
│
├── Distributions
│   ├── RealDistributions
│   ├── DiscreteDistributions
│   └── SpatialDistribution
│
└── Domain
    ├── CompositeDomain
    ├── DiscreteDomain
    └── GeometricDomain
```

### Random Generators

`IRandomGenerator` abstracts the source of pseudo-random values.

The simulation therefore depends on an abstraction rather than directly on
`System.Random`.

This provides:

* deterministic generation through seeded generators;
* easier testing;
* the possibility of alternative random generators;
* independence between probability distributions and the underlying random
  source.

---

### Distributions

`IDistribution<T>` represents a source capable of generating values according
to a probability distribution.

```text
IDistribution<T>
       │
       ├── Real distributions
       ├── Discrete distributions
       └── Spatial distributions
```

The implementation contains several standard probability distributions.

Real-valued distributions include, among others:

* Uniform;
* Normal;
* Exponential;
* Gamma;
* Beta;
* Cauchy;
* Laplace;
* Gumbel;
* Log-normal;
* Pareto;
* Rayleigh;
* Student's t;
* Triangular;
* Weibull.

Discrete distributions include:

* Bernoulli;
* Binomial;
* Geometric;
* Hypergeometric;
* Multinomial;
* Negative Binomial;
* Poisson;
* Discrete Uniform;
* Zipf.

Spatial distributions compose scalar distributions to generate positions in
the simulation plane.

For example:

```text
RadialDistribution
    ├── radius distribution
    └── angle distribution
              │
              ▼
           Vector2
```

Similarly, `IndependentAxisDistribution` generates a position by sampling
its two coordinates independently.

`MixtureDistribution<T>` allows several distributions to be combined and
selected according to relative weights.

The mathematical implementations are intentionally isolated behind the
`IDistribution<T>` abstraction so that the rest of the domain does not need
to know how a particular distribution is sampled.

---

### Domains

`IDomain<T>` represents a set for which membership can be evaluated.

```text
IDomain<T>
     │
     ├── Discrete domains
     ├── Geometric domains
     └── Composite domains
```

A domain answers a simple question:

```text
Contains(value) → true / false
```

Geometric domains currently include concepts such as:

* intervals;
* boxes;
* half-planes;
* ellipses;
* hyperbolas;
* parabolas;
* polygons.

Composite domains allow domains to be combined logically:

```text
Union
Intersection
Complement
```

This is particularly useful for spatial initialization, where a spatial
distribution can be constrained by an arbitrary geometric condition.

---

## Formula and Mathematics Are Complementary

Formula and Mathematics have different responsibilities.

Formula defines the **language and representation of expressions**.

Mathematics provides mechanisms that may be used by the domain, such as
probability distributions, random generation, and geometric constraints.

They should therefore not be conflated:

```text
Formula
    → represents expressions

Mathematics
    → provides mathematical mechanisms

Laws
    → gives those mechanisms domain meaning
```

For example, a law may use a distribution to describe initialization, while
the execution layer remains unaware of the underlying sampling algorithm.

---

## Dependency Direction

Core is intentionally positioned at the bottom of the architecture.

Conceptually:

```text
                    API / Presentation
                           │
                           ▼
                      Application
                           │
                           ▼
                     Core
```

Core should not acquire dependencies on:

* HTTP;
* controllers;
* gateways;
* transport protocols;
* serialization formats;
* execution pipelines;
* application orchestration;
* persistence mechanisms.

Instead, these higher-level concerns consume the abstractions defined by
Core.

This keeps the domain model reusable and prevents execution concerns from
leaking into the fundamental representation of the simulation.

---

## Execution Independence

One of the main architectural goals of Core is to keep the domain model
independent from execution.

For example, a law may evolve from a simple model:

```text
Initialization
Variation
Transmission
```

into a more sophisticated model in the future.

The execution layer should not need to understand the internal structure of
those rules. It should consume the representations exposed by Core that are
appropriate for execution, such as compiled expressions and variable
references.

This gives the architecture the following property:

```text
Domain semantics
       │
       ▼
Core representation
       │
       ▼
Execution representation
       │
       ▼
Execution mechanism
```

Changes in the domain model can therefore remain localized as long as the
contract exposed to execution remains stable.

---

## Design Principles

The Core assembly follows several principles.

### 1. Domain before execution

Core describes the simulation concepts without implementing the orchestration
of a simulation run.

### 2. Abstraction over implementation

Random generators, probability distributions, domains, and values are
represented through abstractions where appropriate.

### 3. Semantic separation

Different concepts remain separate even when they are technically related.

For example:

* a spatial distribution generates positions;
* a spatial domain constrains valid positions;
* a law gives initialization a domain meaning.

### 4. Composability

Mathematical abstractions are designed to be composed.

Examples include:

* distributions composed into spatial distributions;
* distributions combined through mixtures;
* domains combined through union, intersection, and complement.

### 5. Execution isolation

Execution mechanisms should depend on stable representations exposed by Core
rather than on the internal implementation of domain concepts.

### 6. Mathematical mechanisms remain explicit

Algorithms used for probability generation and geometric evaluation are kept
as explicit implementations rather than hidden behind application-level
services.

This makes the mathematical behavior inspectable, testable, and replaceable.

---

## Internal Access

Some Core implementation details are intentionally exposed as `internal`.

`LawsLaboratory.Application` is granted access where the application layer
needs to consume internal representations while keeping them outside the
public API.

`LawsLaboratory.Tests` is also granted access so that internal behavior can
be tested without making implementation details part of the public contract.

This distinction allows Core to maintain a relatively small public API while
still supporting the application and test assemblies.

---

## Summary

`LawsLaboratory.Core` is the conceptual foundation of LawsLaboratory.

It defines:

```text
                 CORE
                  │
     ┌────────────┼────────────┐
     │            │            │
  Formula       Laws      SpatialModel
     │            │            │
     └────────────┼────────────┘
                  │
             Mathematics
                  │
       ┌──────────┴──────────┐
       │                     │
  Distributions           Domains
       │                     │
       └──────────┬──────────┘
                  │
            Domain concepts
```

Its responsibility is to make the simulation's concepts precise and
composable while remaining independent from the mechanisms that execute,
coordinate, transport, or expose those concepts.

In short:

> **Core defines the vocabulary and mathematical foundations of the
> simulation. Higher layers decide how that vocabulary is executed.**

```
```
