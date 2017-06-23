using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    class ArgorithmDrand
    {
        public static int GenerateAssignment(SatProblem origProblem, IWellAnalyzed algorithm, out VariableAssignment assignment)
        {
            assignment = new VariableAssignment();

            var curProblem = origProblem;
            foreach (var problemVariable in origProblem.Variables)
            {
                var subsProblem0 = SubstituteVaraible(curProblem, problemVariable, false);
                var subsProblem1 = SubstituteVaraible(curProblem, problemVariable, true);

                double W0 = algorithm.GetExpectedValue(subsProblem0);
                double W1 = algorithm.GetExpectedValue(subsProblem1);

                if (W0 <= W1)
                {
                    assignment[problemVariable] = true;
                    curProblem = subsProblem1;
                }
                else
                {
                    assignment[problemVariable] = false;
                    curProblem = subsProblem0;
                }
            }

            return origProblem.GetNumberOfSatisfiedClauses(assignment);
        }

        private static SatProblem SubstituteVaraible(SatProblem problem, Variable variable, bool value)
        {
            SatProblem result = new SatProblem();
            foreach (var clause in problem)
            {
                if (clause.TryGetValue(variable, out bool negated))
                {
                    if (negated != value)
                        result.Add(new Clause());
                    else if (clause.Count > 1)
                    {
                        Clause newClause = new Clause(clause);
                        newClause.Remove(variable);
                        result.Add(newClause);
                    }
                }
                else
                {
                    result.Add(new Clause(clause));
                }
            }

            return result;
        }
    }
}
