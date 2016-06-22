using lolirift.Client.MonoGame.Components;
using lolirift.Client.OpenGL.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lolirift.Client.MonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LoliriftGame : Game
    {
        GraphicsDeviceManager graphics;

        private DataStore data;
        private NetworkComponent network;
        private InputComponent input;
        private CameraComponent camera;
        private GridComponent grid;

        public LoliriftGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            data = new DataStore();

            network = new NetworkComponent(this, data);
            Components.Add(network);

            input = new InputComponent(this);
            Components.Add(input);

            camera = new CameraComponent(this, input);
            Components.Add(camera);

            grid = new GridComponent(this, data, camera);
            Components.Add(grid);
        }
    }
}
