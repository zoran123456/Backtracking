using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            KnightsTour tour = new KnightsTour();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Solving...");
                Console.WriteLine("");


                Stopwatch sw = Stopwatch.StartNew();

                bool ok = tour.Solve(0, 0);

                sw.Stop();

                if (ok)
                    tour.DisplaySolution(sw);
                else
                    Console.WriteLine("No solution found :(");


                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("Press key for new test.");

                Console.ReadKey();
            }

        }
    }
}
