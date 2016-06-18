using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

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

        public override void Execute(JObject j) { }
        //{
        //    var keyword = j["building"].ToString();
        //    var loli = j["loli"].ToString();
        //    var building = data.Environment.Entities
        //        .Where(e => e.ContainsElement<BuildableElement>())
        //        .Select(e => e.GetElement<BuildableElement>())
        //        .First(e => e.Name == keyword);

        //    if (building.IsTraining)
        //        throw new ArgumentException("The building is training a loli");

        //    new Thread(new ParameterizedThreadStart(Produce))
        //    {
        //        Priority = ThreadPriority.BelowNormal,
        //        IsBackground = true
        //    }.Start(new {
        //        building = building,
        //        loli = 
        //    });
        //}

        //private void Produce(object param)
        //{
        //    var building = param as BuildableElement;
        //    building.IsTraining = true;
        //    Thread.Sleep(building.Duration * 1000);
        //    building.IsTraining = false;

        //    var loli = building.Lolis
        //        .Select(l => FormatterServices.GetUninitializedObject(l) as LoliElement)
        //        .First(l => l.Name = ;
        //}
    }
}
