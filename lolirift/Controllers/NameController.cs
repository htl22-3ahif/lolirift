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
            get { return new[] { "name", "target" }; }
        }

        public NameController(DataStore data)
            : base(data)
        {

        }

        public override void Execute(JObject j)
        {
            var target = j["target"].ToString();
            var name = j["name"].ToString();

            if (target == "me")
            {
                if (data.Environment.Entities
                    .Where(e => e.ContainsElement<LoliconElement>())
                    .Any(e => e.GetElement<LoliconElement>().Name == name))
                    throw new ArgumentException("There is already aa lolicon named " + name);

                data.Lolicon.Name = name;
                return;
            }

            if (data.Lolicon.GetUnits().Any(u => u.Name == name))
                throw new ArgumentException("There is already an unit named " + name);

            var targetUnit = data.Lolicon.GetUnits()
                .First(u => u.Name == target);
            targetUnit.Name = name;
        }
    }
}
