using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;

namespace lolirift.Controllers
{
    internal abstract class Controller
    {
        protected DataStore data;

        public abstract string Keyword { get; }
        public abstract Controller[] SubControllers { get; }

        public Controller(DataStore data)
        {
            this.data = data;
        }

        public bool TryParse(string[] args)
        {
            if (args.Length < 1)
                return false;

            if (args[0] != Keyword)
                return false;

            return true;
        }

        public void Parse(string[] args)
        {
            args = args.Skip(1).ToArray();

            if (args.Length == 0)
                Execute(args);
            else
            {
                if (SubControllers != null || SubControllers.Length == 0)
                {
                    foreach (var c in SubControllers)
                        if (c.Keyword == args[0])
                            c.Execute(args);
                }
                else
                    Execute(args);
            }
        }

        public abstract void Execute(string[] args);
    }
}
