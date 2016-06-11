using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using lolirift.Controllers;
using Newtonsoft.Json.Linq;

namespace lolirift
{
    public sealed class LoliconElement : Element
    {
        private Controller[] controllers;
        private DataStore data;

        public TcpClient Tcp { get; set; }

        public LoliconElement(Environment environment, Entity entity)
            : base(environment, entity)
        {

        }

        public override void Initialize()
        {
            data = new DataStore()
            {
                Environment = Environment,
                Tcp = Tcp,
                Lolicon = this
            }; ;

            controllers = new Controller[]
            {
                new BuildController(data),
                new HelloController(data),
                new SeeController(data),
                new MapController(data)
            };

            data.Controllers = controllers;

            new Task(() => { while (true) ReceivePackets(); } ).Start();
        }

        public void ReceivePackets()
        {
            var net = Tcp.GetStream();
            var buffer = new byte[4096];
            int length = 0;
            do
            {
                length = net.Read(buffer, 0, buffer.Length);
            } while (length == 0);

            var json = string.Empty;
            var openBracketCount = buffer.Count(b => b == '{');
            var closedBracketCount = buffer.Count(b => b == '}');
            json += Encoding.UTF8.GetString(buffer, 0, length);
            
            while (openBracketCount != closedBracketCount)
            {
                length = net.Read(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length);
            }

            var j = JsonConvert.DeserializeObject(json) as JObject;

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
            catch (Exception e) { Console.WriteLine("Executing controller went wrong! Failure Message: " + e.Message); }
        }
    }
}
