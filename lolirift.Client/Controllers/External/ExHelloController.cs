using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers.External
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
            Console.WriteLine(j["message"].ToString());
        }
    }
}
