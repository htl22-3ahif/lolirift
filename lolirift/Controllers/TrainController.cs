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
                return new[] { "building", "loli-type" };
            }
        }

        public override void Execute(JObject j)
        {
            var buildingname = j["building"].ToString();
            var lolitype = j["loli-type"].ToString();

            var building = data.Lolicon.GetUnits()
                .Where(u => u is BuildingElement)
                .Select(u => u as BuildingElement)
                .First(b => b.Name == buildingname);

            if (building.IsTraining)
                throw new ArgumentException();

            lock (building)
                building.IsTraining = true;
            Thread.Sleep(building.TrainDuration * 1000);
            lock (building)
                building.IsTraining = false;

            if (!building.Lolis.Contains(lolitype))
                throw new ArgumentException(string.Format("Building {0} can not train {1}", building.Name, lolitype));

            var loli = data.Environment.GetEntity("Lolis").Elements
                .Where(e => e is LoliElement)
                .Select(e => e as LoliElement)
                .First(e => e.Type == lolitype);

            var lolipos = new Point(building.Position.X + 1, building.Position.Y);

            if (grid.Get(lolipos).Unit != null)
                throw new ArgumentException("There is already an unit");

            var entity = new Entity(data.Lolicon.Entity.Name + ':' + lolitype + count, data.Environment);
            entity.AddElement(loli.GetType());
            (entity.GetElement(loli.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(loli.GetType()) as UnitElement).Position = lolipos;
            (entity.GetElement(loli.GetType()) as UnitElement).Name = lolitype + count;
            count++;

            grid.Set(entity.GetElement(loli.GetType()) as UnitElement, lolipos);

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
