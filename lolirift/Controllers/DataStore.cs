using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using System.Net.Sockets;

namespace lolirift.Controllers
{
    public sealed class DataStore
    {
        public Environment Environment { get; set; }
        public LoliconElement Lolicon { get; set; }
        public Controller[] Controllers { get; set; }
        public byte[] ResponseData { get; set; }

        public DataStore()
        {
        }
    }
}
