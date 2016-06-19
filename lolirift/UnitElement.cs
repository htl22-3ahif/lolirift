using fun.Core;
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
        UnitElement[] lastUnits;

        public LoliconElement Lolicon;

        public Point Position { get; set; }
        public string Name { get; set; }

        public abstract int Range { get; }
        public abstract string Keyword { get; }
        public abstract Point[] Spread { get; }

        public UnitElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            lastUnits = new UnitElement[0];

            var seeable = new List<object>();

            seeable.Add(new
            {
                x = Position.X,
                y = Position.Y,
                unit = Keyword,
                name = Name,
                owner = Lolicon.Name
            });

            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                source = Name,
                seeable = seeable.ToArray()
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
                    unit = unit.Keyword,
                    name = unit.Name
                });
            }

            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                source = Name,
                seeable = seeable.ToArray()
            });

            Lolicon.Send(json);

            lastUnits = units;
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

                        units.Add(g.Unit);
                    }

            return units.ToArray();
        }
    }
}
