using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace lolirift.Client.OpenGL.Controllers
{
    internal abstract class Controller
    {
        protected DataStore data;

        public abstract string Keyword { get; }

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

            return true;
        }

        public abstract void Execute(JObject j);
    }
}
