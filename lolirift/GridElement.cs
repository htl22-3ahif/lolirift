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
        private GridField[] grid;

        public int Height;
        public int Width;

        public GridElement(Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            grid = new GridField[Height * Width];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new GridField();
            }
        }

        public void Set(LoliriftElement lolirift, int x, int y)
        {
            grid[x + Width * y].Lolirift = lolirift;
        }

        public GridField Get(int x, int y)
        {
            return grid[x + Width * y];
        }
    }

    public struct GridField
    {
        public LoliriftElement Lolirift;
    }
}
