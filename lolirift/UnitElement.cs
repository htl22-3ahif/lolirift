using fun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lolirift
{
    public abstract class UnitElement : Element
    {
        public LoliconElement Lolicon;

        public int PosX { get; set; }
        public int PosY { get; set; }

        public abstract int Range { get; }
        public abstract string Name { get; }

        public UnitElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            new Thread(new ThreadStart(() => { while (true) SendInfo(); }))
            {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true,
            }.Start();
        }

        public void SendInfo()
        {
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                var units = InRangeUnits();
                if (units == null)
                    return;

                var seeable = new List<object>();

                foreach (var unit in units)
                {
                    seeable.Add(new
                    {
                        x = unit.PosX,
                        y = unit.PosY,
                        owner = unit.Lolicon.Name,
                        unit = unit.Name
                    });
                }

                var json = JsonConvert.SerializeObject(new
                {
                    controller = "see",
                    seeable = seeable.ToArray()
                });

                Lolicon.Send(json);
            }
            finally
            {
                // Restore the thread default priority.
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
            }
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

                        var x = offx + PosX;
                        var y = offy + PosY;

                        if (x < 0 || y < 0 || x >= grid.Width || y >= grid.Height)
                            continue;

                        var g = grid.Get(x, y);

                        if (g.Unit == null)
                            continue;

                        units.Add(g.Unit);
                    }

            return units.ToArray();
        }
    }
}
