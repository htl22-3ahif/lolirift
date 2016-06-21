using lolirift.Client.OpenGL.Controllers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.OpenGL
{
    class GridDrawer
    {
        private DataStore data;

        public GridDrawer(DataStore data)
        {
            this.data = data;
        }

        public void Draw(FrameEventArgs e)
        {
            for (int x = 0; x < data.Grid.Width; x++)
            {
                for (int y = 0; y < data.Grid.Height; y++)
                {
                    GL.Begin(BeginMode.Quads);
                    var height = data.Grid.Get(x, y).Height;

                    GL.Color3(height, height, height);
                    GL.Vertex3(x, -y, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x + 1, -y, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x + 1, -y + 1, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x, -y + 1, 0);

                    GL.End();
                }
            }
        }
    }
}
