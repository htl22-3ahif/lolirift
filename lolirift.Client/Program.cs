using lolirift.Client.Controllers;
using lolirift.Client.Controllers.External;
using lolirift.Client.Controllers.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lolirift.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var endPoint = args[0].Split(':');
            var tcp = new TcpClient();
            var data = new DataStore()
            {
                Tcp = tcp
            };
            var init = new InInitializationController(data);
            var inControllers = new Controller[]
            {
                new InHelloController(data),
                new InBuildController(data),
                new InSeeController(data)
            };
            var exControllers = new Controller[]
            {
                new ExHelloController(data),
                new ExSeeController(data)
            };

            Console.WriteLine("Connecting to the host...");
            tcp.Connect(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));
            Console.WriteLine("Connecting successful!");
            Console.WriteLine();

            Console.Write("Write your name please: ");

            {
                var j = JObject.FromObject(new
                {
                    controller = "init",
                    args = Console.ReadLine().Split(' ')
                });

                if (init.IsExecutable(j))
                    init.Execute(j);
                else
                    throw new Exception();
            }

            Console.WriteLine("Starting to receive packets by host");
            new Task(() =>
            {
                while (true)
                {
                    var net = tcp.GetStream();

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

                    var j = JsonConvert.DeserializeObject(json) as JObject;

                    try
                    {
                        foreach (var controller in exControllers)
                            if (controller.IsExecutable(j))
                                controller.Execute(j);
                    }
                    catch (Exception e) { Console.WriteLine("Executing controller went wrong! Failure Message: " + e.Message); }
                }
            }).Start();

            while (true)
            {
                Console.WriteLine("You are now allowed to write your command!");
                var line = Console.ReadLine();

                var j = JObject.FromObject(new
                {
                    controller = line.Split(' ')[0],
                    args = line.Split(' ').Skip(1)
                });

                foreach (var controller in inControllers)
                    if (controller.IsExecutable(j))
                        controller.Execute(j);
                Console.WriteLine();
            }
        }
    }
}
