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
        WebSocket ws;
        DataStore data;
        Controller[] controllers;
        GridDrawer griddrawer;
        Camera camera;

        public LoliriftGame(string endPoint, int width, int height)
            : base(width, height)
        {
            data = new DataStore();
            controllers = new Controller[]
            {
                new MapController(data)
            };

            camera = new Camera(this);
            griddrawer = new GridDrawer(data,camera);

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
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (data.Grid == null)
                return;

            camera.Update(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (data.Grid == null)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            base.OnRenderFrame(e);

            camera.Draw(e);
            griddrawer.Draw(e);

            this.SwapBuffers();
        }
    }
}
