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

            var from = loli.Position;
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
            var startTime = DateTime.Now;

            while (loli.Position != to)
            {
                var time = (DateTime.Now - startTime).TotalSeconds;

                var covered = loli.Speed * time;
                covered = Math.Min(covered, length);

                var x = direction.X * covered;
                var y = direction.Y * covered;
                Console.WriteLine("delta={0}", time);
                Console.WriteLine("x={0} ; y={1}",x,y);

                var old = loli.Position;

                loli.Position = new Point(
                    from.X + (x > 0 ? (int)Math.Floor(x) : (int)Math.Ceiling(x)),
                    from.Y + (y > 0 ? (int)Math.Floor(y) : (int)Math.Ceiling(y)));

                if (old != loli.Position)
                    lock (grid)
                    {
                        grid.Set(null, old);
                        grid.Set(loli, loli.Position);
                    }
            }
        }
    }
}
