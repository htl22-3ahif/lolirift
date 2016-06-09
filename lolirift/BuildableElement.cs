﻿using fun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public abstract class BuildableElement : Element
    {
        public abstract string Keyword { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public BuildableElement(Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }
    }
}