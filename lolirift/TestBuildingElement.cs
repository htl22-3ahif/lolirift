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
    public sealed class TestBuildingElement : BuildingElement
    {
        public override string Type { get { return "test"; } }
        public override int Range { get { return 5; } }
        public override string[] Lolis { get { return new[] { "test-loli" }; } }
        public override Point[] Spread { get { return null; } }
        public override int TrainDuration { get { return 5; } }

        public TestBuildingElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
        }
    }
}
