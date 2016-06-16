using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers
{
    internal sealed class SeeController : Controller
    {
        private GridElement grid;

        public override string Keyword { get { return "see"; } }
        public override string[] NeededKeys { get { return null; } }

        public SeeController(DataStore data)
            : base(data)
        {
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
        }

        public override void Execute(JObject j)
        {
            var seeable = new List<object>();

            foreach (var unit in data.Lolicon.Units)
            {
                seeable.Add(new
                {
                    x = unit.PosX,
                    y = unit.PosY,
                    unit = unit.Name,
                    owner = unit.Lolicon.Name
                });

                foreach (var see in unit.InRangeUnits())
                {
                    seeable.Add(new
                    {
                        x = see.PosX,
                        y = see.PosY,
                        unit = see.Name,
                        owner = see.Lolicon.Name
                    });
                }
            }

            var response = new
            {
                controller = "see",
                seeable = seeable.ToArray()
            };

            var json = JsonConvert.SerializeObject(response, Formatting.None,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            data.Lolicon.Send(json);
        }
    }
}
