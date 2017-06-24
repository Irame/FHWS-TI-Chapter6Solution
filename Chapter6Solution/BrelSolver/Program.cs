using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter6Solution;

namespace BrelSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: BrelSolver.exe <path-to-cnf-file>");
                Environment.Exit(1);
            }

            string filePath = Path.GetFullPath(args[0]);
            if (File.Exists(filePath))
            {
                SatProblem problem = CnfFileParser.Parse(filePath);
                AlgorithmB algoB = new AlgorithmB(problem);
                algoB.GenerateProbabilities();
                string result = string.Join(",", algoB.Probailities.OrderBy(pair => pair.Key.Index).Select(pair => $"{pair.Value.ToString(CultureInfo.InvariantCulture)}"));
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"File '{filePath}' not found.");
            }
        }
    }
}
