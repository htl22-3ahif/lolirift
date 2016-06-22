using lolirift.Client.OpenGL.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.MonoGame.Components
{
    class GridComponent : DrawableGameComponent
    {
        private CameraComponent camera;
        private DataStore data;

        private BasicEffect effect;
        private IndexBuffer ib;
        private VertexBuffer vb;

        public GridComponent(Game game, DataStore data, CameraComponent camera)
            : base(game)
        {
            this.camera = camera;
            this.data = data;
        }

        public override void Initialize()
        {
            effect = new BasicEffect(GraphicsDevice);
            effect.LightingEnabled = true;

            var vertices = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(new Vector3(0, 0, 0), new Vector3(0,0,1), new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3(1, 0, 0), new Vector3(0,0,1), new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3(1, -1, 0), new Vector3(0,0,1), new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3(0, -1, 0), new Vector3(0,0,1), new Vector2(0,0))
            };

            var indices = new int[]
            {
                0, 1, 2,
                2, 3, 0
            };

            vb = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            ib = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);

            vb.SetData(vertices);
            ib.SetData(indices);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Projection = camera.Projection;
            effect.View = camera.View;

            GraphicsDevice.SetVertexBuffer(vb);
            GraphicsDevice.Indices = ib;

            for (int x = 0; x < data.Grid.Width; x++)
            {
                for (int y = 0; y < data.Grid.Height; y++)
                {
                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        effect.World = Matrix.CreateTranslation(new Vector3(x, -y, 0));
                        var height = data.Grid.Get(x, y).Height;
                        effect.AmbientLightColor = new Color(height, height, height).ToVector3();

                        pass.Apply();
                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
                    }
                }
            }

        }
    }
}
