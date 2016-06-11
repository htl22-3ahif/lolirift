using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers.Internal
{
    class InSeeController : Controller
    {
        public InSeeController(DataStore data) : base(data) { }

        public override string Keyword { get { return "see"; } }
        public override string[] NeededKeys { get { return new string[] { "args" }; } }

        public override void Execute(JObject j)
        {
            var response = new Dictionary<string, string>();

            response.Add("controller", "see");

            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
