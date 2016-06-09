using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;

namespace lolirift
{
    public abstract class LoliElement : LoliriftElement
    {
        public abstract int Speed { get; }

        public LoliElement(fun.Core.Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }
    }
}
