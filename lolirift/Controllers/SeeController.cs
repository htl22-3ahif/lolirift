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
            var owneds = data.Environment.Entities.Where(e => e.Elements.Any(el => el.GetType().IsSubclassOf(typeof(UnitElement))))
                .Select(e => e.Elements.First(el => el.GetType().IsSubclassOf(typeof(UnitElement))))
                .Where(e => (e as UnitElement).Lolicon == data.Lolicon).ToArray();

            var seeable = new List<object>();

            for (int x = 0; x < grid.Width; x++)
                for (int y = 0; y < grid.Height; y++)
                    if (owneds.Contains(grid.Get(x, y).Unit))
                    {
                        var g = grid.Get(x, y);
                        var owned = g.Unit;
                        seeable.Add(new
                        {
                            x = x,
                            y = y,
                            information = new
                            {
                                unit = g.Unit.Name,
                                owner = g.Unit.Lolicon.Entity.Name
                            }
                        });

                        for (int offx = -owned.Range; offx < owned.Range; offx++)
                            for (int offy = -owned.Range; offy < owned.Range; offy++)
                                if (offx * offx + offy * offy <= owned.Range * owned.Range)
                                {
                                    g = grid.Get(offx + x, offy + y);

                                    if (offx == 0 && offy == 0)
                                        continue;

                                    if (g.Unit == null)
                                        continue;

                                    seeable.Add(new
                                    {
                                        x = offx + x,
                                        y = offy + y,
                                        information = new
                                        {
                                            unit = g.Unit.Name,
                                            owner = g.Unit.Lolicon.Entity.Name
                                        }
                                    });
                                }
                    }

            var response = new
            {
                controller = "see",
                seeable = seeable.ToArray()
            };

            data.ResponseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.None,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));
        }
    }
}
