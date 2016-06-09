using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.Controllers
{
    internal abstract class Controller
    {
        protected DataStore data;

        public abstract string Keyword { get; }
        public abstract string[] NeededKeys { get; }

        public Controller(DataStore data)
        {
            this.data = data;
        }

        public bool Executable(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey(GetType().Name.ToLower()))
                return false;

            if (dict[GetType().Name.ToLower()] != Keyword)
                return false;

            if (NeededKeys == null)
                return true;

            foreach (var key in NeededKeys)
                if (!dict.ContainsKey(key))
                    return false;

            return true;
        }

        public abstract void Execute(Dictionary<string, string> dict);
    }
}
