using fun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public abstract class BuildableElement : UnitElement
    {
        public abstract Type[]Lolis { get; }
        public abstract int Duration { get; }

        public bool IsTraining { get; set; }

        public BuildableElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
            IsTraining = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            var seeable = new List<object>();

            seeable.Add(new
            {
                x = Position.X,
                y = Position.Y,
                unit = Name,
                owner = Lolicon.Name

            });

            foreach (var unit in this.InRangeUnits())
            {
                seeable.Add(new
                {
                    x = unit.Position.X,
                    y = unit.Position.Y,
                    unit = unit.Name,
                    owner = unit.Lolicon.Name
                });
            }

            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                seeable = seeable.ToArray()
            });

            Lolicon.Send(json);
        }
    }
}
