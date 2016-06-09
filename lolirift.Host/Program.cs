using fun.IO;
using Environment = fun.Core.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lolirift.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] libraries;
            var environment = new EnvironmentXmlReader().Load(new FileStream("lolirift.xml", FileMode.Open), out libraries)[0];

            Console.WriteLine("Environment ready!");

            environment.Initialize();
            while (true)
            {
                Thread.Sleep(20);
                environment.Update(0.02);
            }
        }
    }
}
