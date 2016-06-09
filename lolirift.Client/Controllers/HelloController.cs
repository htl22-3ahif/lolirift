using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers
{
    internal sealed class HelloController : Controller
    {
        public override string Keyword { get { return "hello"; } }
        public override string[] NeededKeys { get { return null; } }

        public HelloController(DataStore data)
            : base(data)
        {
        }

        public override void Execute(Dictionary<string, string> dict)
        {
            Console.WriteLine(dict["message"]);
        }

        public override string ToJson(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
