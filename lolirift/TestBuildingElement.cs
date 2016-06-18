﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using System.Drawing;

namespace lolirift
{
    public sealed class TestBuildingElement : BuildableElement
    {
        public override string Name { get { return "test"; } }
        public override int Range { get { return 5; } }
        public override Type[] Lolis { get { return new[] { typeof(TestLoliElement) }; } }
        public override Point[] Spread { get { return null; } }
        public override int Duration { get { return 5; } }

        public TestBuildingElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
        }
    }
}
