using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LawsLaboratory.Core.Value;



namespace LawsLaboratory.Core.SpatialModel
{
    internal sealed class Parameter
    {
        private IValue _value { get; set; }
        public int _id {  get; }
        
        public Parameter(int id)
        {
            _value = Dead.Instance;
            _id = id;
        }

        public void set(double value)
        {
            _value = _value.Set(value);
        }

    }
}
