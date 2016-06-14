using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace lolirift.Controllers
{
    internal sealed class DataStore
    {
        public Environment Environment { get; set; }
        public string OwnerID { get; set; }
        public Action<string> Send { get; set; }

        public DataStore()
        {
        }
    }
}
