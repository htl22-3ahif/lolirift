using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.MonoGame.Components
{
    class CameraComponent : GameComponent
    {
        private InputComponent input;
        private Vector3 position;

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public CameraComponent(Game game, InputComponent input) 
            : base(game)
        {
            this.input = input;
        }

        public override void Initialize()
        {
            position = new Vector3(0, 0, 10);

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game.GraphicsDevice.DisplayMode.AspectRatio, 0.0001f, 10000f);

            View = Matrix.CreateLookAt(position, position + -Vector3.UnitZ, Vector3.UnitY);
        }

        public override void Update(GameTime gameTime)
        {
            if (input.Mouse.GetMouseButtonDown(MouseButton.Left))
                position += new Vector3(
                    -input.Mouse.Delta.X * (position.Z / 100), 
                    input.Mouse.Delta.Y * (position.Z / 100), 0);

            position += new Vector3(0, 0, -input.Mouse.Delta.Z * 0.01f);

            View = Matrix.CreateLookAt(position, position + -Vector3.UnitZ, Vector3.UnitY);
        }
    }
}
