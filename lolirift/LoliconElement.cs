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
using lolirift.Controllers.External;

namespace lolirift
{
    public sealed class LoliconElement : Element
    {
        private Controller[] exControllers;
        private Controller[] inControllers;
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

            exControllers = new Controller[]
            {
                new ExBuildController(data),
                new ExHelloController(data)
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
            json += Encoding.UTF8.GetString(buffer, 0, length);

            while (openBracketCount != closedBracketCount)
            {
                length = net.Read(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length);
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            try
            {
                foreach (var controller in exControllers)
                    if (controller.Executable(dict))
                    {
                        Console.WriteLine("A packet was sent refering to the build controller");
                        Console.WriteLine("Processing...");

                        controller.Execute(dict);

                        Console.WriteLine("Processing successful!");
                    }
            }
            catch (Exception e) { Console.WriteLine("Executing controller went wrong! Failure Message: " + e.Message); }
        }
    }
}
