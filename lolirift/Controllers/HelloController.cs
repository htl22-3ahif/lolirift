﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Controllers
{
    internal sealed class HelloController : Controller
    {
        public override string Keyword { get { return "hello"; } }
        public override string[] NeededKeys { get { return null; } }

        public HelloController(DataStore data)
            : base(data)
        {
        }

        public override void Execute(Dictionary<string, string> dict)
        {
            var response = new Dictionary<string, string>();
            var net = data.Tcp.GetStream();

            response.Add("message", "Hello!");
            var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            net.Write(jsonData, 0, jsonData.Length);
        }
    }
}
