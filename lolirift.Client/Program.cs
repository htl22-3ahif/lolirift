using lolirift.Client.Controllers;
using lolirift.Client.Controllers.External;
using lolirift.Client.Controllers.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var endPoint = args[0].Split(':');
            var tcp = new TcpClient();

            var data = new DataStore();
            data.Tcp = tcp;

            var inControllers = new Controller[]
            {
                new InHelloController(data),
                new InBuildController(data),
                new InSeeController(data)
            };
            
            var exControllers = new Controller[]
            {
                new ExHelloController(data)
            };

            
            Console.WriteLine("Connecting to the host...");
            tcp.Connect(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));
            Console.WriteLine("Connecting successful!");
            Console.WriteLine();

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
                    
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    try
                    {
                        foreach (var controller in exControllers)
                            if (controller.Executable(dict))
                                controller.Execute(dict);
                    }
                    catch (Exception e) { Console.WriteLine("Executing controller went wrong! Failure Message: " + e.Message); }
                }
            }).Start();

            while (true)
            {
                Console.WriteLine("You are now allowed to write your command!");
                var line = Console.ReadLine();

                var dict = new Dictionary<string, string>();

                dict.Add("controller", line.Split(' ')[0]);
                dict.Add("args", line.Split(' ').Length > 1 ? (line.Substring(line.IndexOf(' ') + 1)) : string.Empty);

                foreach (var controller in inControllers)
                    if (controller.Executable(dict))
                        controller.Execute(dict);
                Console.WriteLine();
            }
        }
    }
}
