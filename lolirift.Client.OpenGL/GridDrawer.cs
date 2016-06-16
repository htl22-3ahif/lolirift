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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }
    }
}
