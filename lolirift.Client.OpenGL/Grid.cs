using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.OpenGL
{
    class Grid
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private GridField[] grid;

        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            grid = new GridField[width * height];
            Position = new Vector2();
            Scale = 1f;
        }

        public GridField Get(int x, int y)
        {
            return grid[y * Width + x];
        }

        public void Set(int x, int y, GridField field)
        {
            grid[y * Width + x] = field;
        }

        public struct GridField
        {

        }
    }
}
