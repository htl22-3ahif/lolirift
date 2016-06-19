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
                var j = JsonConvert.DeserializeObject<JObject>(e.Data);

                Console.WriteLine("Server says: ");
                foreach (var prop in j.Properties())
                {
                    Console.WriteLine("  " + prop.Name + ":" + prop.Value.ToString());
                }

                Console.WriteLine();
            };

            Console.WriteLine("Connected to Server!");

            do
            {
                try
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
                }
                catch (Exception) { Console.WriteLine("Invalid Input!"); Console.WriteLine(); }
            } while (line != string.Empty);
        }
    }
}
