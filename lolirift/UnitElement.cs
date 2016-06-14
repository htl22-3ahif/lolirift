using fun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift
{
    public abstract class UnitElement : Element
    {
        public LoliconElement Lolicon;

        public abstract int Range { get; }
        public abstract string Name { get; }

        public UnitElement(fun.Core.Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }
    }
}
