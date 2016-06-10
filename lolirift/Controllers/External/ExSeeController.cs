using Newtonsoft.Json;
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

        public override void Execute(Dictionary<string, string> dict)
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

                        for (int i = -owned.Range; i < owned.Range; i++)
                            for (int j = -owned.Range; j < owned.Range; j++)
                                if (i * i + j * j <= owned.Range * owned.Range)
                                    seeable.Add(new
                                    {
                                        x = i+x,
                                        y = j+y,
                                        lolirift = grid.Get(i + x, j + y).Lolirift != null
                                            ? grid.Get(i + x, j + y).Lolirift.Name : string.Empty
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
                    NullValueHandling = NullValueHandling.Include
                }));

            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
