using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;

namespace lolirift
{
    class MainBuildingElement : BuildingElement
    {
        public MainBuildingElement(fun.Core.Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        public override int TrainDuration
        {
            get
            {
                return 0;
            }
        }

        public override string Type
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

        public override string[] Lolis
        {
            get
            {
                return null;
            }
        }
    }
}
