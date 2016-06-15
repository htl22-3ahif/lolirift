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

        public int PosX { get; set; }
        public int PosY { get; set; }

        public abstract int Range { get; }
        public abstract string Name { get; }

        public UnitElement(fun.Core.Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        protected UnitElement[] InRageUnits()
        {
            var grid = Environment.GetEntity("Grid").GetElement<GridElement>();
            var units = new List<UnitElement>();

            for (int offx = -Range; offx < Range; offx++)
                for (int offy = -Range; offy < Range; offy++)
                    if (offx * offx + offy * offy <= Range * Range)
                    {
                        if (offx == 0 && offy == 0)
                            continue;

                        var x = offx + PosX;
                        var y = offy + PosY;

                        if (x < 0 || y < 0 || x >= grid.Width || y >= grid.Height)
                            continue;

                        var g = grid.Get(x, y);

                        if (g.Unit == null)
                            continue;

                        units.Add(g.Unit);
                    }

            return units.ToArray();
        }
    }
}
