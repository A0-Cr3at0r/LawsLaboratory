using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LawsLaboratory.Core.Value
{
    public sealed class ScalarValue : IValue
    {
        private double Value { get; set;  }
        internal ScalarValue(double value) { 
            Value = value;
        }
        public IValue Set(double value) {
            Value = value;
            return this;
        }

        internal double get()
        {
            return Value;
        }

     }
}
