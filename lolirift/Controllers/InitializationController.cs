using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using fun.Core;
using Environment = fun.Core.Environment;
using Newtonsoft.Json;

namespace lolirift.Controllers
{
    internal sealed class InitializationController : Controller
    {
        public override string Keyword { get { return "init"; } }
        public override string[] NeededKeys { get { return new[] { "name" }; } }

        public InitializationController(DataStore data) 
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var name = j["name"].ToString();

            if (data.Environment.Entities.Any(e => e.Name == name))
            {
                var response = JObject.FromObject(new
                {
                    controller = "error",
                    message = "Name already exists"
                });

                data.ResponseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                return;
            }

            var entity = new Entity(name, data.Environment);
            entity.AddElement<LoliconElement>();
            entity.GetElement<LoliconElement>().Data = data;

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
