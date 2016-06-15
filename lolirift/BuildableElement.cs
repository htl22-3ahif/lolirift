using fun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = fun.Core.Environment;

namespace lolirift
{
    public abstract class BuildableElement : UnitElement
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract Type Loli { get; }

        public BuildableElement(Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            var json = JsonConvert.SerializeObject(new
            {
                controller = "see",
                seeable = new[]
                {
                    new
                    {
                        x = PosX,
                        y = PosY,
                        information = new
                        {
                            unit = Name,
                            owner = Lolicon.Name
                        }
                    }
                }
            });

            Lolicon.Send(json);
        }
    }
}
