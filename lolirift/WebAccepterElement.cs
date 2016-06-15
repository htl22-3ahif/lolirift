using fun.Core;
using lolirift.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public sealed class WebAccepterElement : Element
    {
        private WebSocketServer wss;

        public int Port;

        public WebAccepterElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            wss = new WebSocketServer(Port);
            wss.AddWebSocketService("/", () => new Play(Environment));
            wss.Start();
        }

        class Play : WebSocketBehavior
        {
            private Environment environment;
            private DataStore data;
            private Controller[] controllers;

            public Play(Environment environment)
            {
                this.environment = environment;
            }

            protected override void OnOpen()
            {
                var entity = new Entity(ID, environment);
                entity.AddElement<LoliconElement>();
                entity.GetElement<LoliconElement>().Name = ID;
                entity.GetElement<LoliconElement>().Send = new Action<string>(Send);

                lock (environment)
                    environment.AddEntity(entity);

                entity.Initialize();

                data = new DataStore()
                {
                    Environment = environment,
                    Lolicon = entity.GetElement<LoliconElement>()
                }; ;

                controllers = new Controller[]
                {
                    new BuildController(data),
                    new HelloController(data),
                    new SeeController(data),
                    new MapController(data)
                };

                Console.WriteLine("New client! ID=" + ID);
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                var j = JsonConvert.DeserializeObject<JObject>(e.Data);

                try
                {
                    foreach (var controller in controllers)
                        if (controller.IsExecutable(j))
                        {
                            Console.WriteLine("A packet was sent refering to the build controller");
                            Console.WriteLine("Processing...");

                            controller.Execute(j);

                            Console.WriteLine("Processing successful!");
                        }
                }
                catch (Exception exc) { Console.WriteLine("Executing controller went wrong! Failure Message: " + exc.Message); }
            }
        }
    }
}
