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
        private BuildingElement[] buildables;
        private GridElement grid;
        private int count;

        public override string Keyword { get { return "build"; } }
        public override string[] NeededKeys { get { return new[] { "building-type", "x", "y" }; } }

        public BuildController(DataStore data)
            : base(data)
        {
            count = 0;
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();

            buildables = data.Environment.GetEntity("Buildings").Elements
                .Where(e => e.GetType().IsSubclassOf(typeof(BuildingElement)))
                .Select(e => e as BuildingElement)
                .ToArray();
        }

        public override void Execute(JObject j)
        {
            var type = j["building-type"].ToString();
            var pos = new Point(
                int.Parse(j["x"].ToString()),
                int.Parse(j["y"].ToString()));
            BuildingElement building;

            try { building = buildables.First(b => b.Type == type); }
            catch (Exception) { throw new ArgumentException(string.Format(
                "Building with Keyword \"{0}\" does not work", type)); }

            if (pos.X < 0 || pos.Y < 0 || pos.X >= grid.Width || pos.Y >= grid.Height)
                throw new ArgumentException();

            if (building.Spread != null)
                foreach (var p in building.Spread)
                    if (p.X < 0 || p.Y < 0 || p.X >= grid.Width || p.Y >= grid.Height)
                        throw new ArgumentException();

            if (grid.Get(pos).Unit != null)
                throw new ArgumentException("There is already a unit");

            var entity = new Entity(data.Lolicon.Entity.Name +':'+ type + count, data.Environment);
            entity.AddElement(building.GetType());
            (entity.GetElement(building.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(building.GetType()) as UnitElement).Position = pos;
            (entity.GetElement(building.GetType()) as UnitElement).Name = type + count;
            count++;

            grid.Set(entity.GetElement(building.GetType()) as UnitElement, pos);
            if (building.Spread != null)
                foreach (var p in building.Spread)
                    grid.Set(entity.GetElement(building.GetType()) as UnitElement, p);

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
