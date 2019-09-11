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
            NQueenProblem problem = new NQueenProblem();


            Console.Clear();

            Console.WriteLine("Solving...");
            Console.WriteLine("");


            Stopwatch sw = Stopwatch.StartNew();

            bool ok = problem.Solve();

            Console.WriteLine($"Finding solution took {sw.Elapsed.Milliseconds} ms");

            sw.Stop();

            if (ok)
                problem.DisplayBoard();
            else
                Console.WriteLine("No solution found :(");


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Press key to exit.");

            Console.ReadKey();


        }
    }
}
