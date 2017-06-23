using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    class AlgorithmA : IWellAnalyzed
    {
        private Random rand = new Random();
        public double TrueProbability { get; set; } = 0.5;

        public int GenerateAssingment(SatProblem problem, out VariableAssignment assignment)
        {
            assignment = new VariableAssignment();
            foreach (var problemVariable in problem.Variables)
            {
                assignment[problemVariable] = rand.NextDouble() < TrueProbability;
            }

            return problem.GetNumberOfSatisfiedClauses(assignment);
        }

        public int GenerateAssingment(SatProblem problem, int trys, out VariableAssignment assignment)
        {
            int bestSatisfaction = 0;
            assignment = null;
            for (int i = 0; i < trys; i++)
            {
                int newSatisfaction;
                if ((newSatisfaction = GenerateAssingment(problem, out VariableAssignment newAssignment)) > bestSatisfaction)
                {
                    assignment = newAssignment;
                    bestSatisfaction = newSatisfaction;
                }
            }
            return bestSatisfaction;
        }

        public double GetExpectedValue(SatProblem problem)
        {
            return problem.Select(clause => clause.Count == 0 ? 1 : 1 - 1.0 / (1 << clause.Count)).Sum();
        }
    }
}
