using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers.External
{
    internal sealed class ExSeeController : Controller
    {
        private GridElement grid;

        public override string Keyword { get { return "see"; } }
        public override string[] NeededKeys { get { return null; } }

        public ExSeeController(DataStore data)
            : base(data)
        {
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
        }

        public override void Execute(JObject j)
        {
            var owneds = data.Environment.Entities.Where(e => e.Elements.Any(el => el.GetType().IsSubclassOf(typeof(LoliriftElement))))
                .Select(e => e.Elements.First(el => el.GetType().IsSubclassOf(typeof(LoliriftElement))))
                .Where(e => (e as LoliriftElement).Lolicon == data.Lolicon).ToArray();

            var seeable = new List<object>();

            for (int x = 0; x < grid.Width; x++)
                for (int y = 0; y < grid.Height; y++)
                    if (owneds.Contains(grid.Get(x, y).Lolirift))
                    {
                        var owned = grid.Get(x, y).Lolirift;
                        seeable.Add(new
                        {
                            x = x,
                            y = y,
                            lolirift = grid.Get(x, y).Lolirift.Name
                        });

                        for (int offx = -owned.Range; offx < owned.Range; offx++)
                            for (int offy = -owned.Range; offy < owned.Range; offy++)
                                if (offx * offx + offy * offy <= owned.Range * owned.Range)
                                    seeable.Add(new
                                    {
                                        x = offx + x,
                                        y = offy + y,
                                        lolirift = grid.Get(offx + x, offy + y).Lolirift != null
                                            ? grid.Get(offx + x, offy + y).Lolirift.Name : null
                                    });
                    }

            var response = new
            {
                controller = "see",
                seeable = seeable.ToArray()
            };

            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, Formatting.None,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
