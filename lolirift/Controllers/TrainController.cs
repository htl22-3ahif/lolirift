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

        public TrainController(DataStore data) : base(data)
        {
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
            var building = data.Environment.Entities
                .Where(e => e.ContainsElement<BuildableElement>())
                .Select(e => e.GetElement<BuildableElement>())
                .First(e => e.Name == keyword);

            if (building.IsTraining)
                throw new ArgumentException();

            building.IsTraining = true;
            Thread.Sleep(building.Duration * 1000);
            building.IsTraining = false;

            var loli = building.Lolis
                .Select(l => FormatterServices.GetUninitializedObject(l) as LoliElement)
                .First(l => l.Name == loliName);

            var lolipos = new Point(building.Position.X + 1, building.Position.Y);

            if (grid.Get(lolipos).Unit != null)
                throw new ArgumentException("There is already an unit");

            var entity = new Entity(keyword + DateTime.Now.ToString("yyyy-mm-dd:hh:mm:ss:ffff"), data.Environment);
            entity.AddElement(loli.GetType());
            (entity.GetElement(loli.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(loli.GetType()) as UnitElement).Position = lolipos;

            grid.Set(entity.GetElement(loli.GetType()) as UnitElement, lolipos);

            lock(data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
