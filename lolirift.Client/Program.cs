using lolirift.Client.Controllers;
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
            var dict = new Dictionary<string, string>();

            var data = new DataStore();
            data.Tcp = tcp;

            var inControllers = new Controller[]
            {
                new InHelloController(data)
            };

            Console.WriteLine("Connecting to the host...");
            tcp.Connect(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));
            Console.WriteLine("Connecting successful!");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("You are now allowed to write your command!");
                Console.WriteLine("We recommend using \"hello\"");
                var line = Console.ReadLine();


                dict.Add("controller", line.Split(' ')[0]);
                dict.Add("args", line.Substring(line.IndexOf(' ') + 1));

                foreach (var controller in inControllers)
                    if (controller.Executable(dict))
                        controller.Execute(dict);
            }
        }
    }
}
