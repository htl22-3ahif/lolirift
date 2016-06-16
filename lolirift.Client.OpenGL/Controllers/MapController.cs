using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace lolirift.Client.OpenGL.Controllers
{
    class MapController : Controller
    {
        public MapController(DataStore data) : base(data)
        {
        }

        public override string Keyword
        {
            get
            {
                return "map";
            }
        }

        public override void Execute(JObject j)
        {
            data.Grid = new Grid(
                int.Parse(j["width"].ToString()),
                int.Parse(j["height"].ToString())
            );

            Console.WriteLine("Width: " + data.Grid.Width);
            Console.WriteLine("Height: " + data.Grid.Height);
        }
    }
}
