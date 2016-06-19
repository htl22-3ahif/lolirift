﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fun.Core;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace lolirift.Controllers
{
    internal sealed class BuildController : Controller
    {
        private BuildableElement[] buildables;
        private GridElement grid;
        private int count;

        public override string Keyword { get { return "build"; } }
        public override string[] NeededKeys { get { return new[] { "building", "x", "y" }; } }

        public BuildController(DataStore data)
            : base(data)
        {
            count = 0;
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();

            buildables = data.Environment.GetEntity("Buildings").Elements
                .Where(e => e.GetType().IsSubclassOf(typeof(BuildableElement)))
                .Select(e => e as BuildableElement)
                .ToArray();
        }

        public override void Execute(JObject j)
        {
            var keyword = j["building"].ToString();
            var pos = new Point(
                int.Parse(j["x"].ToString()),
                int.Parse(j["y"].ToString()));
            BuildableElement building;

            try { building = buildables.First(b => b.Keyword == keyword); }
            catch (Exception) { throw new ArgumentException(string.Format(
                "Building with Keyword \"{0}\" does not work", keyword)); }

            if (pos.X < 0 || pos.Y < 0 || pos.X >= grid.Width || pos.Y >= grid.Height)
                throw new ArgumentException();

            if (building.Spread != null)
                foreach (var p in building.Spread)
                    if (p.X < 0 || p.Y < 0 || p.X >= grid.Width || p.Y >= grid.Height)
                        throw new ArgumentException();

            if (grid.Get(pos).Unit != null)
                throw new ArgumentException("There is already a unit");

            var entity = new Entity(data.Lolicon.Entity.Name +':'+ keyword + count, data.Environment);
            entity.AddElement(building.GetType());
            (entity.GetElement(building.GetType()) as UnitElement).Lolicon = data.Lolicon;
            (entity.GetElement(building.GetType()) as UnitElement).Position = pos;
            (entity.GetElement(building.GetType()) as UnitElement).Name = keyword + count;
            count++;

            grid.Set(entity.GetElement(building.GetType()) as UnitElement, pos);
            if (building.Spread != null)
                foreach (var p in building.Spread)
                    grid.Set(entity.GetElement(building.GetType()) as UnitElement, p);

            lock (data.Environment)
                data.Environment.AddEntity(entity);

            entity.Initialize();
        }
    }
}
