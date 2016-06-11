using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace lolirift.Controllers.External
{
    internal sealed class ExMapController : Controller
    {
        public override string Keyword
        {
            get
            {
                return "map";
            }
        }

        public override string[] NeededKeys
        {
            get
            {
                return null;
            }
        }

        public ExMapController(DataStore data)
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
            var response = new
            {
                width = grid.Width,
                height = grid.Height
            };

            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
        }
    }
}
