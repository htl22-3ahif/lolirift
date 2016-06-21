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
        private Camera camera;

        public GridDrawer(DataStore data, Camera camera)
        {
            this.data = data;
            this.camera = camera;
        }

        public void Draw(FrameEventArgs e)
        {
            for (int x = (int)Math.Max(0, camera.Position.X - 50); x < Math.Min(camera.Position.X + 50, data.Grid.Width); x++)
            {
                for (int y = (int)Math.Max(0, -camera.Position.Y - 50); y < Math.Min(-camera.Position.Y + 50, data.Grid.Height); y++)
                {
                    GL.Begin(BeginMode.Quads);
                    var height = data.Grid.Get(x, y).Height;

                    GL.Color3(height, height, height);
                    GL.Vertex3(x /*+ (x * 0.1f)*/, -y /*- (y * 0.1f)*/, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x /*+ (x * 0.1f)*/ + 1, -y /*- (y * 0.1f)*/, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x /*+ (x * 0.1f)*/ + 1, -y + 1 /*- (y * 0.1f)*/, 0);

                    GL.Color3(height, height, height);
                    GL.Vertex3(x /*+ (x * 0.1f)*/, -y + 1 /*- (y * 0.1f)*/, 0);

                    GL.End();
                }
            }
        }


    }
}
