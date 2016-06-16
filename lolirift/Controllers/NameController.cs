using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers
{
    internal sealed class NameController : Controller
    {
        public override string Keyword
        {
            get { return "name"; }
        }

        public override string[] NeededKeys
        {
            get { return new[] { "name" }; }
        }

        public NameController(DataStore data)
            : base(data)
        {

        }

        public override void Execute(JObject j)
        {
            data.Lolicon.Name = j["name"].ToString();
        }
    }
}
