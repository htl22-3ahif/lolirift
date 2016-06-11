using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers.Internal
{
    internal sealed class InBuildController : Controller
    {
        public override string Keyword { get { return "build"; } }
        public override string[] NeededKeys { get { return new[] { "args" }; } }

        public InBuildController(DataStore data) 
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var response = new Dictionary<string, string>();
            response.Add("controller", "build");

            var args = j["args"].Select(e => e.ToString()).ToArray();
            response.Add("name", args[0]);
            response.Add("x", args[1]);
            response.Add("y", args[2]);

            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
