using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Drawing;
using fun.Core;

namespace lolirift.Controllers
{
    class TrainController : Controller
    {
        private GridElement grid;
        private int count;

        public TrainController(DataStore data) : base(data)
        {
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
            count = 0;
        }

        public override string Keyword
        {
            get
            {
                return "train";
            }
        }

        public override string[] NeededKeys
        {
            get
            {
                return new[] { "building", "loli" };
            }
        }

        public override void Execute(JObject j)
        {
            var keyword = j["building"].ToString();
            var loliName = j["loli"].ToString();

            var building = data.Lolicon.GetUnits()
                .Where(u => u is BuildableElement)
                .Select(u => u as BuildableElement)
                .First(b => b.Keyword == keyword);

            if (building.IsTraining)
                throw new ArgumentException();

            lock (building)
                building.IsTraining = true;
            Thread.Sleep(building.Duration * 1000);
            lock (building)
                building.IsTraining = false;

            var loli = building.Lolis
                .Select(l => FormatterServices.GetUninitializedObject(l) as LoliElement)
                .First(l => l.Keyword == loliName);

            var lolipos = new Point(building.Position.X + 1, building.Position.Y);

            if (grid.Get(lolipos).Unit != null)
                throw new ArgumentException("There is already an unit");

            var entity = new Entity(data.Lolicon.Entity.Name + ':' + loliName + count, data.Environment);
            entity.AddElement(loli.GetType());
            (entity.GetElement(loli.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(loli.GetType()) as UnitElement).Position = lolipos;
            (entity.GetElement(loli.GetType()) as UnitElement).Name = loliName + count;
            count++;

            grid.Set(entity.GetElement(loli.GetType()) as UnitElement, lolipos);

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
