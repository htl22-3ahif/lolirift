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
        private Entity[] grid;

        public int Height;
        public int Width;

        public GridElement(Environment environment, Entity entity) : base(environment, entity)
        {
        }

        public void Set(Entity entity, int x, int y)
        {
            grid[x + Width * y] = entity;
        }

        public Entity Get(int x, int y)
        {
            return grid[x + Width * y];
        }

        public override void Initialize()
        {
            grid = new Entity[Height * Width];
        }
    }
}
