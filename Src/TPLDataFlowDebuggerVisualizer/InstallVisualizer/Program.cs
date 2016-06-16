using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var vs11COMNTOOLS = Environment.GetEnvironmentVariable("VS110COMNTOOLS");

          

            if (string.IsNullOrWhiteSpace(vs11COMNTOOLS))
            {
                Console.WriteLine("Can NOT find install dir for vs 2010");
                Console.ReadKey();
            }
            else
            {
                var visualizersPath = Path.Combine(vs11COMNTOOLS.Remove(vs11COMNTOOLS.Length - 6, 6), @"Packages\Debugger\Visualizers\");
                foreach (var source in args)
                {
                    var target = Path.Combine(visualizersPath, Path.GetFileName(source));
                    File.Copy(source, target, true);
                }
              

            }

           
        }
    }
}
