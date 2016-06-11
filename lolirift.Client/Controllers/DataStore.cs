using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace lolirift.Client.Controllers
{
    internal sealed class DataStore
    {
        public TcpClient Tcp { get; set; }
    }
}
