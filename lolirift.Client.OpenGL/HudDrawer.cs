using lolirift.Client.OpenGL.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.OpenGL
{
    class HudDrawer
    {
        private DataStore data;
        private Camera camera;

        public HudDrawer(Camera camera, DataStore data)
        {
            this.camera = camera;
            this.data = data;
        }
    }
}
