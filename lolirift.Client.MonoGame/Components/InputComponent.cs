using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lolirift.Client.MonoGame.Components
{
    internal sealed class InputComponent : GameComponent
    {
        public MouseInput Mouse { get; private set; }
        public KeyboardInput Keyboard { get; private set; }
        //public GamePadInput Gamepad { get; private set; }

        public InputComponent(Game game)
            : base(game)
        {
            Mouse = new MouseInput(game);
            Keyboard = new KeyboardInput();
        }

        public override void Initialize()
        {
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

            if (Game.IsActive)
                Mouse.Update();
            Keyboard.Update();
        }
    }

    internal sealed class MouseInput
    {
        private Game game;
        private List<MouseButton> lastMouseButtons;
        private List<MouseButton> currMouseButtons;
        private MouseState curr;
        private MouseState last;

        public Vector3 Delta { get; set; }
        public float MouseSensitivity { get; set; }

        public MouseInput(Game game)
        {
            this.game = game;

            lastMouseButtons = new List<MouseButton>();
            currMouseButtons = new List<MouseButton>();
            last = Mouse.GetState();

            MouseSensitivity = 0.1f;
        }

        public bool GetMouseButtonPressed(MouseButton mouseButton)
        {
            return currMouseButtons.Contains(mouseButton) && !lastMouseButtons.Contains(mouseButton);
        }
        public bool GetMouseButtonDown(MouseButton mouseButton)
        {
            return currMouseButtons.Contains(mouseButton);
        }
        public bool GetMouseButtonRelease(MouseButton mouseButton)
        {
            return !currMouseButtons.Contains(mouseButton) && lastMouseButtons.Contains(mouseButton);
        }

        public void Update()
        {
            lastMouseButtons = currMouseButtons;
            var centreScreen = new Point(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);

            curr = Mouse.GetState();

            currMouseButtons = new List<MouseButton>();

            if (!currMouseButtons.Contains(MouseButton.Right) && curr.RightButton == ButtonState.Pressed)
                currMouseButtons.Add(MouseButton.Right);
            if (currMouseButtons.Contains(MouseButton.Right) && curr.RightButton == ButtonState.Released)
                currMouseButtons.Remove(MouseButton.Right);

            if (!currMouseButtons.Contains(MouseButton.Left) && curr.LeftButton == ButtonState.Pressed)
                currMouseButtons.Add(MouseButton.Left);
            if (currMouseButtons.Contains(MouseButton.Left) && curr.LeftButton == ButtonState.Released)
                currMouseButtons.Remove(MouseButton.Left);

            if (!currMouseButtons.Contains(MouseButton.Middle) && curr.MiddleButton == ButtonState.Pressed)
                currMouseButtons.Add(MouseButton.Middle);
            if (currMouseButtons.Contains(MouseButton.Middle) && curr.MiddleButton == ButtonState.Released)
                currMouseButtons.Remove(MouseButton.Middle);


            Delta = new Vector3(
                (curr.X - last.X) * MouseSensitivity,
                (curr.Y - last.Y) * MouseSensitivity,
                (curr.ScrollWheelValue - last.ScrollWheelValue));

            last = curr;
        }
    }
    internal sealed class KeyboardInput
    {
        private Keys[] lastKeys;
        private Keys[] currKeys;

        public Keys[] DownKeys { get { return currKeys; } }
        public Keys[] ReleasedKeys { get { return lastKeys.Where(k => !currKeys.Contains(k)).ToArray(); } }

        public KeyboardInput()
        {
            currKeys = new Keys[0];
            lastKeys = new Keys[0];
        }

        public bool GetKeyPressed(Keys key)
        {
            return !lastKeys.Contains(key) && currKeys.Contains(key);
        }
        public bool GetKeyDown(Keys key)
        {
            return currKeys.Contains(key);
        }
        public bool GetKeyRelease(Keys key)
        {
            return lastKeys.Contains(key) && !currKeys.Contains(key);
        }

        public void Update()
        {
            lastKeys = currKeys;
            KeyboardState keyboardState = Keyboard.GetState();
            currKeys = keyboardState.GetPressedKeys();
        }

    }
    public enum MouseButton
    {
        Right,
        Left,
        Middle
    }
    // TODO: Gamepad implementireren
}
