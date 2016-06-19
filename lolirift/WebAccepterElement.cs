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
using System.Threading;
using System.Drawing;

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
            private GridElement grid;

            public Play(Environment environment)
            {
                this.environment = environment;
                grid = environment.GetEntity("Grid").GetElement<GridElement>();
            }

            protected override void OnOpen()
            {
                Random rnd = new Random();

                var entity = new Entity(ID, environment);
                entity.AddElement<LoliconElement>();
                entity.GetElement<LoliconElement>().Name = ID;
                entity.GetElement<LoliconElement>().Send = new Action<string>(Send);

                var main = new Entity(ID + ':' + "main", environment);
                main.AddElement<MainBuildingElement>();
                var mainBuilding = main.GetElement<MainBuildingElement>();
                mainBuilding.Position = new Point(
                    rnd.Next(grid.Width),
                    rnd.Next(grid.Height));
                mainBuilding.Lolicon = entity.GetElement<LoliconElement>();
                mainBuilding.Name = "main";
                grid.Set(mainBuilding, mainBuilding.Position);

                lock (environment)
                {
                    environment.AddEntity(entity);
                    environment.AddEntity(main);
                }

                entity.Initialize();
                main.Initialize();

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
                    new MapController(data),
                    new NameController(data),
                    new TrainController(data),
                    new MoveController(data)
                };

                Console.WriteLine("New client! ID=" + ID);
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                new Thread(() =>
                {
                    var j = JsonConvert.DeserializeObject<JObject>(e.Data);

                    try
                    {
                        foreach (var controller in controllers)
                            if (controller.IsExecutable(j))
                            {
                                Console.WriteLine("Controller: {0}", controller.Keyword);
                                Console.WriteLine("Processing...");

                                controller.Execute(j);

                                Console.WriteLine("Processing successful!");
                            }
                    }
                    catch (Exception exc) { Console.WriteLine("Executing controller went wrong! Failure Message: " + exc.Message); }
                })
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal
                }.Start();
            }
        }
    }
}
