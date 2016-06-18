using fun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public abstract class BuildableElement : UnitElement
    {
        public abstract Type[]Lolis { get; }
        public abstract int Duration { get; }

        public bool IsTraining { get; set; }

        public BuildableElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
            IsTraining = false;
        }
    }
}
