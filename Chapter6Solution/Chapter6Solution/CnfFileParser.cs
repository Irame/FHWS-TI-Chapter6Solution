using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6Solution
{
    class CnfFileParser
    {
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
                        if (int.TryParse(line.Split(' ')[2], out int variableCount))
                        {
                            variables = new Variable[variableCount];
                            for (int i = 0; i < variableCount; i++)
                            {
                                variables[i] = new Variable($"x{i}");
                            }
                        }
                    }
                    else if (variables != null)
                    {
                        foreach (var s in line.Split(' '))
                        {
                            if (int.TryParse(s, out int value))
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
