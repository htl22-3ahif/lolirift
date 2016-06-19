using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;

namespace lolirift
{
    class MainBuildingElement : BuildableElement
    {
        public MainBuildingElement(fun.Core.Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        public override int Duration
        {
            get
            {
                return 0;
            }
        }

        public override Type[] Lolis
        {
            get
            {
                return null;
            }
        }

        public override string Keyword
        {
            get
            {
                return "main";
            }
        }

        public override int Range
        {
            get
            {
                return 20;
            }
        }

        public override Point[] Spread
        {
            get
            {
                return null;
            }
        }
    }
}
