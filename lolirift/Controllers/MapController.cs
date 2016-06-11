﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace lolirift.Controllers
{
    internal sealed class MapController : Controller
    {
        public override string Keyword
        {
            get
            {
                return "map";
            }
        }

        public override string[] NeededKeys
        {
            get
            {
                return null;
            }
        }

        public MapController(DataStore data)
            : base(data)
        {
        }

        public override void Execute(JObject j)
        {
            var grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
            var response = new
            {
                controller = "map",
                width = grid.Width,
                height = grid.Height
            };

            data.ResponseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
        }
    }
}
