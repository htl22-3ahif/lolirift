using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using Newtonsoft.Json.Linq;

namespace lolirift.Controllers
{
    public abstract class Controller
    {
        protected DataStore data;

        public abstract string Keyword { get; }
        public abstract string[] NeededKeys { get; }

        public Controller(DataStore data)
        {
            this.data = data;
        }

        public bool IsExecutable(JObject j)
        {
            if (j["controller"] == null)
                return false;

            if (j["controller"].ToString() != Keyword)
                return false;

            if (NeededKeys == null)
                return true;
            
            foreach (var key in NeededKeys)
                if (j[key] == null)
                    return false;

            return true;
        }

        public abstract void Execute(JObject j);
    }
}
