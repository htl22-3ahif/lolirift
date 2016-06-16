using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Drawing;
using lolirift.Client.OpenGL.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Input;

namespace lolirift.Client.OpenGL
{
    class LoliriftGame : GameWindow
    {
        private MouseState curr;
        private MouseState prev;

        WebSocket ws;
        DataStore data;
        Controller[] controllers;

        public LoliriftGame(string endPoint, int width, int height)
            : base(width, height)
        {
            data = new DataStore();
            controllers = new Controller[]
            {
                new MapController(data)
            };

            ws = new WebSocket(endPoint);
            ws.OnMessage += OnMessage;
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new
            {
                controller = "map"
            }));
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            var j = JsonConvert.DeserializeObject<JObject>(e.Data);

            foreach (var controller in controllers)
                if (controller.IsExecutable(j))
                    controller.Execute(j);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);

            prev = OpenTK.Input.Mouse.GetState();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (data.Grid == null)
                return;

            var delta = new Vector3();

            curr = OpenTK.Input.Mouse.GetState();
            if (curr != prev)
                delta = new Vector3(
                    (prev.X - curr.X),
                    (prev.Y - curr.Y),
                    prev.Wheel - curr.Wheel);
            else
                delta = Vector3.Zero;

            prev = curr;

            if (curr.IsButtonDown(MouseButton.Left))
                data.Grid.Position += new Vector2(-delta.X, delta.Y);

            if (delta.Z < 0)
                data.Grid.Scale += 0.1f * data.Grid.Scale;
            else if (delta.Z > 0)
                data.Grid.Scale -= 0.1f * data.Grid.Scale;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (data.Grid == null)
                return;

            base.OnRenderFrame(e);

            this.SwapBuffers();
        }
    }
}
