using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers.External
{
    internal sealed class ExHelloController : Controller
    {
        public override string Keyword { get { return "hello"; } }
        public override string[] NeededKeys { get { return null; } }

        public ExHelloController(DataStore data)
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var response = new Dictionary<string, string>();
            var net = data.Tcp.GetStream();

            response.Add("controller", "hello");
            response.Add("message", "Hello!");
            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            net.Write(jsonData, 0, jsonData.Length);
        }
    }
}
