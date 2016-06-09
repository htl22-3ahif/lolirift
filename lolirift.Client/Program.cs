using lolirift.Client.Controllers;
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

            var data  =new DataStore();
            var controllers = new Controller[]
            {
                new HelloController(data)
            };

            Console.WriteLine("Connecting to the host...");
            tcp.Connect(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));
            Console.WriteLine("Connecting successful!");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("You are now allowed to write your command!");
                Console.WriteLine("We recommend using \"hello\"");
                var msg = Console.ReadLine();

                dict.Add("content", msg);

                var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict));

                Console.WriteLine("Sending message...");
                tcp.GetStream().Write(jsonData, 0, jsonData.Length);
                Console.WriteLine("Sending successful!");
                Console.WriteLine();
            }
        }
    }
}
