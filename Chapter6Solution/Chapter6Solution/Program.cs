using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            Variable x1 = new Variable("x1");
            Variable x2 = new Variable("x2");
            Variable x3 = new Variable("x3");


            SatProblem parsedProblem = CnfFileParser.Parse(args[0]);

            SatProblem problem = new SatProblem
            {
                new Clause{{x1, true}, x2},
                new Clause{{x2, true}, x3}
            };


            problem = parsedProblem;
            Console.WriteLine($"Problem: n={problem.Variables.Count}, m={problem.Clauses.Count}");

            int benchmarkRuns = 1000;

            int trys = 100;


            Console.WriteLine();
            Console.WriteLine("Algorithmus A:");
            AlgorithmA algoA = new AlgorithmA();

            int satisfiedClausesA = algoA.GenerateAssingment(problem, trys, out VariableAssignment assignmentA);
            Console.WriteLine($"Anzahl der Versuche: {trys}");
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesA}");

            TimeSpan algoAExecutionTime = Benchmark(benchmarkRuns, () => algoA.GenerateAssingment(problem, trys, out assignmentA));
            Console.WriteLine($"Average Runtime for {benchmarkRuns} runs: {algoAExecutionTime.TotalMilliseconds:##0.#####} ms");


            Console.WriteLine();
            Console.WriteLine("Algorithmus B:");
            var algoB = new AlgorithmB(parsedProblem);
            algoB.GenerateProbabilities();

            int satisfiedClausesB = algoB.GenerateAssingment(problem, trys, out VariableAssignment assignmentB);
            Console.WriteLine($"Anzahl der Versuche: {trys}");
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesB}");

            TimeSpan algoBExecutionTime = Benchmark(benchmarkRuns, () =>
            {
                algoB.GenerateProbabilities();
                algoB.GenerateAssingment(problem, trys, out assignmentB);
            });
            Console.WriteLine($"Average Runtime for {benchmarkRuns} runs: {algoBExecutionTime.TotalMilliseconds:##0.#####} ms");


            Console.WriteLine();
            Console.WriteLine("Algorithmus DRAND_A:");

            int satisfiedClausesDrandA = ArgorithmDrand.GenerateAssignment(problem, algoA, out VariableAssignment assignmentDrandA);
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesDrandA}");

            TimeSpan algoDerandAExecutionTime = Benchmark(benchmarkRuns, () => ArgorithmDrand.GenerateAssignment(problem, algoA, out assignmentDrandA));
            Console.WriteLine($"Average Runtime for {benchmarkRuns} runs: {algoDerandAExecutionTime.TotalMilliseconds:##0.#####} ms");


            Console.WriteLine();
            Console.WriteLine("Algorithmus DRAND_B:");

            int satisfiedClausesDrandB = ArgorithmDrand.GenerateAssignment(problem, algoB, out VariableAssignment assignmentDrandB);
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesDrandB}");

            TimeSpan algoDerandBExecutionTime = Benchmark(benchmarkRuns, () => ArgorithmDrand.GenerateAssignment(problem, algoB, out assignmentDrandB));
            Console.WriteLine($"Average Runtime for {benchmarkRuns} runs: {algoDerandBExecutionTime.TotalMilliseconds:##0.#####} ms");


            Console.Read();
        }

        private static TimeSpan Benchmark(int n, Action f)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
            {
                f();
            }
            sw.Stop();
            return new TimeSpan(sw.ElapsedTicks / n);
        }
    }
}
