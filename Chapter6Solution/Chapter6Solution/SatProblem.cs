using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    public class SatProblem : IEnumerable<Clause>
    {
        public IReadOnlyCollection<Variable> Variables => _variables;
        public IReadOnlyList<Clause> Clauses => _clauses;

        private readonly HashSet<Variable> _variables = new HashSet<Variable>();
        private readonly List<Clause> _clauses = new List<Clause>();

        public void Add(Clause clause)
        {
            foreach (var clauseVariable in clause.Variables)
            {
                _variables.Add(clauseVariable);
            }
            _clauses.Add(clause);
        }

        public int GetNumberOfSatisfiedClauses(VariableAssignment assignment)
        {
            return _clauses.Count(clause => clause.IsSatisfied(assignment));
        }

        public IEnumerator<Clause> GetEnumerator()
        {
            return _clauses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(" ∧ ", this);
        }
    }

    [DebuggerDisplay("{ToString()}")]
    public class Clause : Dictionary<Variable, bool>
    {
        public Clause() { }

        public Clause(Clause oldClause) : base(oldClause) {}

        public KeyCollection Variables => this.Keys;

        public void Add(Variable variable)
        {
            Add(variable, false);
        }

        public new void Add(Variable variable, bool negated)
        {
            if (TryGetValue(variable, out bool n))
            {
                if (n != negated)
                    Remove(variable);
            }
            else
            {
                base.Add(variable, negated);
            }
        }

        public bool IsSatisfied(VariableAssignment assignment)
        {
            return Count == 0 || this.Any(literal => literal.Value != assignment[literal.Key]);
        }

        public bool Evaluate(Variable variable, bool value)
        {
            return this[variable] != value;
        }

        public override string ToString()
        {
            return $"({string.Join(" ∨ ", this.Select(p => (p.Value ? "¬" : "") + p.Key.Name))})";
        }
    }

    public class Variable
    {
        public string Name { get; }

        public Variable(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class VariableAssignment : Dictionary<Variable, bool>
    {
        public override string ToString()
        {
            return $"[{string.Join(", ",this.Select(p => $"{p.Key.Name} = {p.Value}"))}]";
        }
    }
}
