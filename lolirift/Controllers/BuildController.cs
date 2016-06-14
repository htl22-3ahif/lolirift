using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace lolirift.Controllers
{
    internal sealed class BuildController : Controller
    {
        private BuildableElement[] buildables;
        private GridElement grid;

        public override string Keyword { get { return "build"; } }
        public override string[] NeededKeys { get { return new[] { "name", "x", "y" }; } }

        public BuildController(DataStore data)
            : base(data)
        {
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();

            var allTypes = Assembly.GetExecutingAssembly().ExportedTypes;

            var types = Assembly.GetExecutingAssembly().ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(BuildableElement)))
                .ToArray();

            buildables = new BuildableElement[types.Length];

            for (int i = 0; i < types.Length; i++)
                buildables[i] = FormatterServices.GetUninitializedObject(types[i]) as BuildableElement;
        }

        public override void Execute(JObject j)
        {
            var keyword = j["name"].ToString();
            var posX = int.Parse(j["x"].ToString());
            var posY = int.Parse(j["y"].ToString());
            BuildableElement building;

            try { building = buildables.First(b => b.Name == keyword); }
            catch (Exception) { throw new ArgumentException(string.Format("Building with Keyword \"{0}\" does not work", keyword)); }

            var entity = new Entity(keyword + DateTime.Now.ToString("yyyy-mm-dd:hh:mm:ss:ffff"), data.Environment);
            entity.AddElement(building.GetType());
            (entity.GetElement(building.GetType()) as UnitElement).Lolicon = data.Lolicon;

            if (posX + building.Width > grid.Width && posY + building.Height > grid.Height)
                throw new ArgumentException(string.Format(
                    "The given X({0}) and Y({1}) arguments do not fit in the grid with the building's Width({2}) and Height({3})",
                    posX, posY, building.Width, building.Height));

            if (posX + building.Width > grid.Width)
                throw new ArgumentException(string.Format(
                    "The given X({0}) arguments do not fit in the grid with the building's Width({1})",
                    posX, building.Width));

            if (posY + building.Height > grid.Height)
                throw new ArgumentException(string.Format(
                    "The given Y({0}) arguments do not fit in the grid with the building's Height({1})",
                    posY, building.Height));

            for (int x = posX; x <= posX + building.Width; x++)
                for (int y = posY; y <= posY + building.Height; y++)
                    grid.Set(entity.GetElement(building.GetType()) as UnitElement, x, y);

            lock (data.Environment)
                data.Environment.AddEntity(entity);
        }
    }
}
