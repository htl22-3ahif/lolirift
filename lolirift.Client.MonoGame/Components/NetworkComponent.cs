using lolirift.Client.OpenGL.Controllers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace lolirift.Client.MonoGame.Components
{
    class NetworkComponent : GameComponent
    {
        private WebSocket ws;
        private Controller[] controllers;
        private DataStore data;

        public NetworkComponent(Game game, DataStore data) 
            : base(game)
        {
            this.data = data;
            controllers = new Controller[]
            {
                new MapController(data)
            };
        }

        public override void Initialize()
        {
            ws = new WebSocket("ws://127.0.0.1:844");
            ws.Connect();

            ws.OnMessage += OnMessage;

            ws.Send(JsonConvert.SerializeObject(new
            {
                controller = "map"
            }));

            while (data.Grid == null) Thread.Sleep(100);
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            var j = JsonConvert.DeserializeObject<JObject>(e.Data);

            foreach (var controller in controllers)
                if (controller.IsExecutable(j))
                    controller.Execute(j);
        }
    }
}
