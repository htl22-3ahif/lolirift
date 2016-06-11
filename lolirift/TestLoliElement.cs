﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;

namespace lolirift
{
    public sealed class TestLoliElement : LoliElement
    {
        public override int Range { get { return 5; } }
        public override int Speed { get { return 1; } }
        public override string Name { get { return "loli-test"; } }

        public TestLoliElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

    }
}