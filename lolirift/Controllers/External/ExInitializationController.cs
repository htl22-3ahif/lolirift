using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using fun.Core;
using Environment = fun.Core.Environment;
using Newtonsoft.Json;

namespace lolirift.Controllers.External
{
    internal sealed class ExInitializationController : Controller
    {
        public override string Keyword { get { return "init"; } }
        public override string[] NeededKeys { get { return new[] { "name" }; } }

        public ExInitializationController(DataStore data) 
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

                var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                data.Tcp.GetStream().Write(jsonData, 0, jsonData.Length);
                data.Tcp.Close();
                return;
            }

            var entity = new Entity(name, data.Environment);
            entity.AddElement<LoliconElement>();
            entity.GetElement<LoliconElement>().Tcp = data.Tcp;

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
