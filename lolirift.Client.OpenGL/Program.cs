using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolirift.Client.OpenGL
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new LoliriftGame("ws://127.0.0.1:844", 1280, 720))
                game.Run();
        }
    }
}
