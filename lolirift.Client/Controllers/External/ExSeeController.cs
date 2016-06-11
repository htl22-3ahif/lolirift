using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace lolirift.Client.Controllers.External
{
    internal sealed class ExSeeController : Controller
    {
        public override string Keyword { get { return "see"; } }

        public override string[] NeededKeys { get { return new[] { "seeable" }; } }

        public ExSeeController(DataStore data) 
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var seeable = j["seeable"].ToArray();
            foreach (var s in seeable)
            {
                var x = int.Parse(s["x"].ToString());
                var y = int.Parse(s["y"].ToString());
                var owner = s["information"]["owner"].ToString();
                var unit = s["information"]["unit"].ToString();
                Console.WriteLine("{0}/{1}:{2} by {3}", x.ToString("000"), y.ToString("000"), unit, owner);
            }
        }
    }
}
