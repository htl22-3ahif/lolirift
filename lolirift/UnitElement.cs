﻿using fun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lolirift
{
    public abstract class UnitElement : Element
    {
        private UnitElement[] lastUnits;
        private Point oldPosition;

        public LoliconElement Lolicon;

        public Point Position { get; set; }
        public string Name { get; set; }

        public abstract int Range { get; }
        public abstract string Type { get; }
        public abstract Point[] Spread { get; }

        public UnitElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            lastUnits = new UnitElement[0];
            oldPosition = Position;

            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                source = new
                {
                    x = Position.X,
                    y = Position.Y,
                    unit = Type,
                    name = Name,
                    owner = Lolicon.Name
                },
                seeable = new object[] { }
            });

            Lolicon.Send(json);

            new Thread(new ThreadStart(() => { while (true) SendInfo(); }))
            {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true,
            }.Start();
        }

        private void SendInfo()
        {
            var units = InRangeUnits();

            if (units.Length == 0)
                return;

            if (Enumerable.SequenceEqual(units, lastUnits))
                return;

            var seeable = new List<object>();

            foreach (var unit in units)
            {
                seeable.Add(new
                {
                    x = unit.Position.X,
                    y = unit.Position.Y,
                    owner = unit.Lolicon.Name,
                    unit = unit.Type,
                    name = unit.Name
                });
            }

            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                source = new
                {
                    x = Position.X,
                    y = Position.Y,
                    unit = Type,
                    name = Name,
                    owner = Lolicon.Name
                },
                seeable = seeable.ToArray()
            });

            Lolicon.Send(json);

            lastUnits = units;
        }

        public override void Update(double time)
        {
            if (oldPosition != Position)
            {
                var json = JsonConvert.SerializeObject(new
                {
                    controller = "see",
                    source = new
                    {
                        x = Position.X,
                        y = Position.Y,
                        unit = Type,
                        name = Name,
                        owner = Lolicon.Name
                    },
                    seeable = new object[] { }
                });

                Lolicon.Send(json);
            }

            oldPosition = Position;
        }

        public UnitElement[] InRangeUnits()
        {
            var grid = Environment.GetEntity("Grid").GetElement<GridElement>();
            var units = new List<UnitElement>();

            for (int offx = -Range; offx < Range; offx++)
                for (int offy = -Range; offy < Range; offy++)
                    if (offx * offx + offy * offy <= Range * Range)
                    {
                        if (offx == 0 && offy == 0)
                            continue;

                        var x = offx + Position.X;
                        var y = offy + Position.Y;

                        if (x < 0 || y < 0 || x >= grid.Width || y >= grid.Height)
                            continue;

                        var g = grid.Get(new Point(x, y));

                        if (g.Unit == null)
                            continue;

                        if (g.Unit.Lolicon == Lolicon)
                            continue;

                        units.Add(g.Unit);
                    }

            return units.ToArray();
        }
    }
}
