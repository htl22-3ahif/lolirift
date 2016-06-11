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

namespace lolirift
{
    public sealed class LoliconElement : Element
    {
        public Controller[] Controllers { get; private set; }
        public DataStore Data { get; set; }

        public LoliconElement(Environment environment, Entity entity)
            : base(environment, entity)
        {

        }

        public override void Initialize()
        {
            Controllers = new Controller[]
            {
                new BuildController(Data),
                new HelloController(Data),
                new SeeController(Data),
                new MapController(Data)
            };
        }
    }
}
