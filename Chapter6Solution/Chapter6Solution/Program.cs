using System;
using System.Collections.Generic;
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

            int trys = 100;

            Console.WriteLine();
            Console.WriteLine("Algorithmus A:");
            AlgorithmA algoA = new AlgorithmA();
            int satisfiedClausesA = algoA.GenerateAssingment(problem, trys, out VariableAssignment assignmentA);
            Console.WriteLine($"Anzahl der Versuche: {trys}");
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesA}");


            Console.WriteLine();
            Console.WriteLine("Algorithmus B:");
            var algoB = new AlgorithmB(parsedProblem);
            algoB.GenerateProbabilities();
            int satisfiedClausesB = algoB.GenerateAssingment(problem, trys, out VariableAssignment assignmentB);
            Console.WriteLine($"Anzahl der Versuche: {trys}");
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesB}");


            Console.WriteLine();
            Console.WriteLine("Algorithmus DRAND_A:");
            int satisfiedClausesDrandA = ArgorithmDrand.GenerateAssignment(problem, algoA, out VariableAssignment assignmentDrandA);
            Console.WriteLine($"Erfüllte Klauseln: {satisfiedClausesDrandA}");

            Console.Read();
        }
    }
}
