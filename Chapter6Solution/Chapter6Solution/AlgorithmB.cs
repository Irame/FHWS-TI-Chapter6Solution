using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;

namespace Chapter6Solution
{
    class AlgorithmB : IWellAnalyzed
    {
        private Random _rand = new Random();
        private SatProblem _problem;
        private Dictionary<Variable, double> _probailities;

        public AlgorithmB(SatProblem problem)
        {
            _problem = problem;
        }

        public void GenerateProbabilities()
        {
            if (_problem.Variables.Count == 0)
                return;

            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Dictionary<Variable, (Decision NotNegated, Decision Negated)> decisions =
                _problem.Variables.ToDictionary(
                    variable => variable,
                    variable => (new Decision(Domain.RealNonnegative, variable.Name), new Decision(Domain.RealNonnegative, "_" + variable.Name)));

            model.AddDecisions(decisions.Values.SelectMany(t => new[] { t.NotNegated, t.Negated }).ToArray());

            foreach (var pair in decisions)
            {
                model.AddConstraint("balance_" + pair.Key.Name, pair.Value.NotNegated + pair.Value.Negated == 1);
            }

            int clauseNum = 0;
            foreach (var problemClause in _problem.Clauses)
            {
                Term term = null;
                foreach (var lieral in problemClause)
                {
                    var decisionToAdd = lieral.Value ? decisions[lieral.Key].Negated : decisions[lieral.Key].NotNegated;
                    term = object.Equals(term, null) ? decisionToAdd : term + decisionToAdd;
                }

                if (!object.Equals(term, null))
                    model.AddConstraint($"clause_{clauseNum++}", term >= 1);
            }

            foreach (var valueTuple in decisions)
            {
                model.AddGoal($"goal_{valueTuple.Key.Name}", GoalKind.Maximize,
                    valueTuple.Value.NotNegated + valueTuple.Value.Negated);
            }

            Solution solution = context.Solve(new SimplexDirective());
            context.ClearModel();

            _probailities = decisions.ToDictionary(pair => pair.Key, pair => pair.Value.NotNegated.ToDouble());
        }


        public int GenerateAssingment(SatProblem problem, out VariableAssignment assignment)
        {
            assignment = new VariableAssignment();
            foreach (var problemVariable in problem.Variables)
            {
                assignment[problemVariable] = _rand.NextDouble() < _probailities[problemVariable];
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
            var algoObjectToUse = this;
            if (problem != _problem)
                algoObjectToUse = new AlgorithmB(problem);

            if (algoObjectToUse._probailities == null)
                algoObjectToUse.GenerateProbabilities();

            double result = 0;
            foreach (var problemClause in problem.Clauses)
            {
                if (problemClause.Count == 0)
                {
                    result++;
                    continue;
                }

                double falsePropability = 1;
                foreach (var literal in problemClause)
                {
                    falsePropability *= literal.Value ? 1 - algoObjectToUse._probailities[literal.Key] : algoObjectToUse._probailities[literal.Key];
                }
                result += 1 - falsePropability;
            }
            return result;
        }
    }
}
