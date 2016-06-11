using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace lolirift.Client.Controllers.Internal
{
    internal sealed class InInitializationController : Controller
    {
        public override string Keyword
        {
            get
            {
                return "init";
            }
        }

        public override string[] NeededKeys
        {
            get
            {
                return new[] { "args" };
            }
        }

        public InInitializationController(DataStore data) 
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var args = j["args"].Select(e => e.ToString()).ToArray();

            var response = JObject.FromObject(new
            {
                controller = "init",
                name = data.Name = args[0]
            });

            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
