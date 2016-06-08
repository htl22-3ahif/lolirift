using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers
{
    internal sealed class HelloController : Controller
    {
        public override string Keyword { get { return "hello"; } }
        public override Controller[] SubControllers { get { return null; } }

        public HelloController(DataStore data) 
            : base(data)
        {
        }

        public override void Execute(string[] args)
        {
            var dict = new Dictionary<string, string>();
            var net = data.Tcp.GetStream();

            dict.Add("message", "Hello!");
            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict));

            net.Write(jsonData, 0, jsonData.Length);
        }
    }
}
