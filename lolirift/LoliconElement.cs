using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using Environment = fun.Core.Environment;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using lolirift.Controllers;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;

namespace lolirift
{
    public sealed class LoliconElement : Element
    {
        public string Name { get; set; }
        public UnitElement[] Units { get; set; }
        public Action<string> Send { get; set; }

        public LoliconElement(Environment environment, Entity entity)
            : base(environment, entity)
        {

        }
    }
}
