using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.OpenGL
{
    class Camera
    {
        private MouseState curr;
        private MouseState prev;
        private GameWindow game;

        public Vector3 Position { get; set; }

        public Camera(GameWindow game)
        {
            this.game = game;
            this.Position = new Vector3(0, 0, 10);
        }

        public void Update(FrameEventArgs e)
        {
            var delta = new Vector3();

            curr = Mouse.GetState();
            if (curr != prev)
                delta = new Vector3(
                    (prev.X - curr.X),
                    (prev.Y - curr.Y),
                    prev.Wheel - curr.Wheel);
            else
                delta = Vector3.Zero;

            prev = curr;

            if (curr.IsButtonDown(MouseButton.Left))
                Position += new Vector3(delta.X * (Position.Z / 1000), -delta.Y * (Position.Z / 1000), 0);

            Position += new Vector3(0, 0, delta.Z);
        }

        public void Draw(FrameEventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            var proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)game.Width / (float)game.Height, 0.001f, 1000);//Matrix4.CreateOrthographic(game.Width, game.Height, 0, 10000);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            var view = Matrix4.LookAt(Position, Position + new Vector3(0, 0, -1), new Vector3(0, 1, 0));
            GL.LoadMatrix(ref view);
        }
    }
}
