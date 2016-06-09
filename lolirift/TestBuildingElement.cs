using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public sealed class TestBuildingElement : BuildableElement
    {
        public override string Keyword { get { return "test"; } }
        public override int Height { get { return 0; } }
        public override int Width { get { return 0; } }
        public override int Range { get { return 5; } }

        public TestBuildingElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
        }
    }
}
