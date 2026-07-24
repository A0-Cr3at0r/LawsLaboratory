using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LawsLaboratory.Core.Value
{
    public sealed class Dead : IValue
    {
        public static Dead Instance { get; } = new();

        public IValue Set(double value)
        {
            return new ScalarValue(value);
        }

        private Dead() {}
    }
}
