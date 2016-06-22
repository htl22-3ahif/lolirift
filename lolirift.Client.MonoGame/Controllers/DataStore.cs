using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace lolirift.Client.OpenGL.Controllers
{
    public sealed class DataStore
    {
        public Grid Grid { get; set; }

        public DataStore()
        {
        }
    }
}
