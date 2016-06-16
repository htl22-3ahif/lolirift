using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace lolirift.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = "";
            var ws = new WebSocket("ws://127.0.0.1:844");
            ws.Connect();

            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine("Server says: " + e.Data);
                Console.WriteLine();
            };

            Console.WriteLine("Connected to Server!");

            do
            {
                line = Console.ReadLine();
                var props = line.Split(' ');
                var j = new JObject();

                foreach (var prop in props)
                {
                    var key = prop.Split(':')[0];
                    var value = prop.Split(':')[1];

                    j[key] = value;
                }

                ws.Send(JsonConvert.SerializeObject(j));

            } while (line != string.Empty);
        }
    }
}
