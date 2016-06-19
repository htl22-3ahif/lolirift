using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace lolirift.Controllers
{
    class MoveController : Controller
    {
        private GridElement grid;

        public MoveController(DataStore data) : base(data)
        {
            grid = data.Environment.GetEntity("Grid").GetElement<GridElement>();
        }

        public override string Keyword
        {
            get
            {
                return "move";
            }
        }

        public override string[] NeededKeys
        {
            get
            {
                return new[] { "loli", "x", "y" };
            }
        }

        public override void Execute(JObject j)
        {
            var name = j["loli"].ToString();
            var loli = data.Lolicon.GetUnits()
                .Where(u => u is LoliElement)
                .Select(u => u as LoliElement)
                .First(l => l.Name == name);
            
            var to = new Point(
                int.Parse(j["x"].ToString()),
                int.Parse(j["y"].ToString()));
            var vector = new Point(
                to.X - loli.Position.X,
                to.Y - loli.Position.Y);
            var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            var direction = new
            {
                X = vector.X / length,
                Y = vector.Y / length
            };
            var covered = 0.0;
            var startTime = DateTime.Now;

            while (loli.Position != to)
            {
                var delta = (DateTime.Now - startTime).TotalSeconds;
                covered += loli.Speed * delta;
                covered = Math.Min(covered, length);
                var x = direction.X * covered;
                var y = direction.Y * covered;
                Console.WriteLine("x={0} ; y={1}",x,y);
                lock (grid)
                    grid.Set(null, loli.Position);
                loli.Position = new Point(
                    loli.Position.X + (int)Math.Floor(x),
                    loli.Position.Y + (int)Math.Floor(y));
                lock (grid)
                    grid.Set(loli, loli.Position);
            }
        }
    }
}
