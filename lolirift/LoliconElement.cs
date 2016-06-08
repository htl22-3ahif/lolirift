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
            data = new DataStore();

            data.Environment = Environment;
            data.Tcp = Tcp;

            controllers = new Controller[]
            {
                new BuildController(data),
                new HelloController(data)
            };

            new Task(() => { while (true) ReceivePackets(); } ).Start();
        }

        public void ReceivePackets()
        {
            var net = Tcp.GetStream();

            var buffer = new byte[4096];
            var length = net.Read(buffer, 0, buffer.Length);

            if (length == 0)
                return;

            var json = string.Empty;
            var openBracketCount = buffer.Count(b => b == '{');
            var closedBracketCount = buffer.Count(b => b == '}');

            while (openBracketCount != closedBracketCount)
            {
                json += Encoding.UTF8.GetString(buffer, 0, length);
                length = net.Read(buffer, 0, buffer.Length);
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var args = dict["message"].Split(' ');

            try
            {
                foreach (var controller in controllers)
                    if (controller.TryParse(args))
                        controller.Parse(args);
            }
            catch (Exception e) { Console.WriteLine("Controller parsing went wrong! Failure Message: " + e.Message); }
        }
    }
}
