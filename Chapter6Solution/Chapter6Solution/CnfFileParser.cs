using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    public class CnfFileParser
    {
        private static readonly Regex ProblemLineRegex = new Regex(@"p\s+(?<ProblemType>\S+)\s+(?<VariableCount>\d+)\s(?<ClauseCount>\d+)", RegexOptions.Compiled);
        private static readonly Regex DataLineRegex = new Regex(@"\s*((?<Date>-?\d+)\s*)*", RegexOptions.Compiled);

        public static SatProblem Parse(string filePath)
        {
            SatProblem result = new SatProblem();

            using (StreamReader reader = new StreamReader(filePath))
            {
                Variable[] variables = null;
                Clause clause = new Clause();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("c"))
                        continue;
                    if (line.StartsWith("p"))
                    {
                        var match = ProblemLineRegex.Match(line);
                        if (match.Success && int.TryParse(match.Groups["VariableCount"].Value, out int variableCount))
                        {
                            variables = new Variable[variableCount];
                            for (int i = 0; i < variableCount; i++)
                            {
                                variables[i] = new Variable(i);
                            }
                        }
                        else throw new Exception($"Could not parse problem line of file {filePath}");
                    }
                    else if (variables != null)
                    {
                        var match = DataLineRegex.Match(line);
                        if (!match.Success)
                            continue;

                        foreach (Capture capture in match.Groups["Date"].Captures)
                        {
                            if (int.TryParse(capture.Value, out int value))
                            {
                                if (value == 0)
                                {
                                    result.Add(clause);
                                    clause = new Clause();
                                }
                                else
                                {
                                    clause.Add(variables[Math.Abs(value)-1], value < 0);
                                }
                            }
                        }
                    }
                }

                if (clause.Count > 0)
                    result.Add(clause);

                return result;
            }
        }
    }
}
