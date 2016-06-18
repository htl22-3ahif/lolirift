using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace lolirift.Controllers
{
    internal sealed class BuildController : Controller
    {
        private BuildableElement[] buildables;
        private GridElement grid;

        public override string Keyword { get { return "build"; } }
        public override string[] NeededKeys { get { return new[] { "building", "x", "y" }; } }

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
            var keyword = j["building"].ToString();
            var pos = new Point(
                int.Parse(j["x"].ToString()),
                int.Parse(j["y"].ToString()));
            BuildableElement building;

            try { building = buildables.First(b => b.Name == keyword); }
            catch (Exception) { throw new ArgumentException(string.Format(
                "Building with Keyword \"{0}\" does not work", keyword)); }

            if (pos.X < 0 || pos.Y < 0 || pos.X >= grid.Width || pos.Y >= grid.Height)
                throw new ArgumentException();

            if (building.Spread != null)
                foreach (var p in building.Spread)
                    if (p.X < 0 || p.Y < 0 || p.X >= grid.Width || p.Y >= grid.Height)
                        throw new ArgumentException();

            if (grid.Get(pos).Unit != null)
                throw new ArgumentException("There is already a unit");

            var entity = new Entity(keyword + DateTime.Now.ToString("yyyy-mm-dd:hh:mm:ss:ffff"), data.Environment);
            entity.AddElement(building.GetType());
            (entity.GetElement(building.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(building.GetType()) as UnitElement).Position = pos;

            grid.Set(entity.GetElement(building.GetType()) as UnitElement, pos);
            if (building.Spread != null)
                foreach (var p in building.Spread)
                    grid.Set(entity.GetElement(building.GetType()) as UnitElement, p);

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
            data.Lolicon.Units.Add(entity.GetElement(building.GetType()) as UnitElement);
        }
    }
}
