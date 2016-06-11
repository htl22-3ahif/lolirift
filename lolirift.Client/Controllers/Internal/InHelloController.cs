using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public override void Execute(JObject j)
        {
            Console.WriteLine("Sending message...");
            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(j));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
            Console.WriteLine("Sending successful!");
            Console.WriteLine();
        }
    }
}
