using lolirift.Client.OpenGL.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.MonoGame.Components
{
    class HudComponent : GameComponent
    {
        private DataStore data;
        private CameraComponent camera;
        private GridComponent grid;

        public HudComponent(Game game, DataStore data, CameraComponent camera, GridComponent grid) 
            : base(game)
        {
            this.data = data;
            this.camera = camera;
            this.grid = grid;
        }
    }
}
