using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Threading;

namespace lolirift.Controllers
{
    class MoveController : Controller
    {
        private const double MIN = 0.2;
        private const double MAX = 1.5;

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
            var time = DateTime.Now;
            var covered = 0.00001;

            while (loli.Position != to)
            {
                Thread.Sleep(200);
                var delta = (DateTime.Now - time).TotalSeconds;
                time = DateTime.Now;

                var nextField = new Point(
                    loli.Position.X + 
                        (Math.Abs(1 / (direction.X * covered)) <= Math.Abs(1 / (direction.Y * covered))
                        ? vector.X / Math.Abs(vector.X) : 0),
                    loli.Position.Y + 
                        (Math.Abs(1 / (direction.X * covered)) >= Math.Abs(1 / (direction.Y * covered))
                        ? vector.Y / Math.Abs(vector.Y) : 0));
                Console.WriteLine(nextField);

                var error = grid.Get(nextField).Height - grid.Get(loli.Position).Height;
                var factor = f(error);
                Console.WriteLine("error:{0}\nfactor:{1}", error, factor);

                covered += loli.Speed * delta * factor;
                covered = Math.Min(covered, length);

                var x = direction.X * covered;
                var y = direction.Y * covered;
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

        private double f(double x)
        {
            var a = (MAX + MIN - 2) / (2 * 255 * 255);
            var b = ((MAX - 1) - 255 * 255 * a) / 255;

            return (x * x * a) + (x * b) + 1;
        }
    }
}
