using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers.Internal
{
    class InHelloController : Controller
    {
        public InHelloController(DataStore data) : base(data) { }

        public override string Keyword { get { return "hello"; } }
        public override string[] NeededKeys { get { return new string[] { "args" }; } }

        public override void Execute(Dictionary<string, string> dict)
        {
            Console.WriteLine("Sending message...");
            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
            Console.WriteLine("Sending successful!");
            Console.WriteLine();
        }
    }
}
