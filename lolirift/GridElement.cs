using fun.Core;
using Environment = fun.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift
{
    public sealed class GridElement : Element
    {
        private LoliriftElement[] loliriftGrid;

        public int Height;
        public int Width;

        public GridElement(Environment environment, Entity entity) : base(environment, entity)
        {
        }

        public void Set(LoliriftElement lolirift, int x, int y)
        {
            loliriftGrid[x + Width * y] = lolirift;
        }

        public LoliriftElement Get(int x, int y)
        {
            return loliriftGrid[x + Width * y];
        }

        public override void Initialize()
        {
            loliriftGrid = new LoliriftElement[Height * Width];
        }
    }
}
