using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    class Solver {
        // Generates a list of all possible combinations of the input values
        private List<int[]> generateCombinations(int[] inputSet) {
            return new List<int[]>() {
                new int[4] { inputSet[0], inputSet[1], inputSet[2], inputSet[3] },
                new int[4] { inputSet[0], inputSet[1], inputSet[3], inputSet[2] },
                new int[4] { inputSet[0], inputSet[2], inputSet[1], inputSet[3] },
                new int[4] { inputSet[0], inputSet[2], inputSet[3], inputSet[1] },
                new int[4] { inputSet[0], inputSet[3], inputSet[1], inputSet[2] },
                new int[4] { inputSet[0], inputSet[3], inputSet[2], inputSet[1] },
                new int[4] { inputSet[1], inputSet[0], inputSet[2], inputSet[3] },
                new int[4] { inputSet[1], inputSet[0], inputSet[3], inputSet[2] },
                new int[4] { inputSet[1], inputSet[2], inputSet[0], inputSet[3] },
                new int[4] { inputSet[1], inputSet[2], inputSet[3], inputSet[0] },
                new int[4] { inputSet[1], inputSet[3], inputSet[0], inputSet[2] },
                new int[4] { inputSet[1], inputSet[3], inputSet[2], inputSet[0] },
                new int[4] { inputSet[2], inputSet[0], inputSet[1], inputSet[3] },
                new int[4] { inputSet[2], inputSet[0], inputSet[3], inputSet[1] },
                new int[4] { inputSet[2], inputSet[1], inputSet[0], inputSet[3] },
                new int[4] { inputSet[2], inputSet[1], inputSet[3], inputSet[0] },
                new int[4] { inputSet[2], inputSet[3], inputSet[0], inputSet[1] },
                new int[4] { inputSet[2], inputSet[3], inputSet[1], inputSet[0] },
                new int[4] { inputSet[3], inputSet[0], inputSet[1], inputSet[2] },
                new int[4] { inputSet[3], inputSet[0], inputSet[2], inputSet[1] },
                new int[4] { inputSet[3], inputSet[1], inputSet[0], inputSet[2] },
                new int[4] { inputSet[3], inputSet[1], inputSet[2], inputSet[0] },
                new int[4] { inputSet[3], inputSet[2], inputSet[0], inputSet[1] },
                new int[4] { inputSet[3], inputSet[2], inputSet[1], inputSet[0] }
            };
        }

        // Represents a potential solution to a set of values and provides a user-readable
        // explanation for the solution.
        public struct Solution { //~~@ should this be a struct? look at usage
            //The pattern type indicates the order of operations for the solution, or indicates failure.
            public enum Pattern {
                FAIL,       // Not a valid solution
                SEQUENTIAL, // ((a . b) . c) . d
                PAIRED,     // (a . b) . (c . d)
                CENTRIC     // (a . (b . c)) . d
                // ~~@ there should be other patterns here too, such as 'a . ((b . c) . d)'
            };

            public Pattern pattern; // ~~@ Patterns should have a class
            public Operator[] operators;
            public int[] values;
            public int score;
            public int[] stepResults;

            public Solution(Pattern pat, Operator[] ops, int[] vals) {
                pattern = pat;
                operators = ops;
                values = vals;
                stepResults = new int[3];
                score = 0;
                score = execute();
            }

            private int executeSequential() {
                stepResults[0] = operators[0].Invoke(values[0], values[1]);
                if (stepResults[0] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[0];

                stepResults[1] = operators[1].Invoke(stepResults[0], values[2]);
                if (stepResults[1] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[1];

                stepResults[2] = operators[2].Invoke(stepResults[1], values[3]);
                if (stepResults[2] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[2];

                if (stepResults[2] != 10) { pattern = Pattern.FAIL; return -1; }
                return score;
            }

            private int executePaired() {
                stepResults[0] = operators[0].Invoke(values[0], values[1]);
                if (stepResults[0] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[0];

                stepResults[1] = operators[2].Invoke(values[2], values[3]);
                if (stepResults[1] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[1];

                stepResults[2] = operators[1].Invoke(stepResults[0], stepResults[1]);
                if (stepResults[2] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[2];

                if (stepResults[2] != 10) { pattern = Pattern.FAIL; return -1; }
                return score;
            }

            private int executeCentric() {
                stepResults[0] = operators[1].Invoke(values[1], values[2]);
                if (stepResults[0] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[0];

                stepResults[1] = operators[0].Invoke(values[0], stepResults[1]);
                if (stepResults[1] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[1];

                stepResults[2] = operators[2].Invoke(stepResults[1], values[3]);
                if (stepResults[2] == -1) { pattern = Pattern.FAIL; return -1; }
                score += stepResults[2];

                if (stepResults[2] != 10) { pattern = Pattern.FAIL; return -1; }
                return score;
            }

            private int execute() {
                switch(pattern) {
                    case Pattern.SEQUENTIAL: return executeSequential();
                    case Pattern.PAIRED: return executePaired();
                    case Pattern.CENTRIC: return executeCentric();
                }
                pattern = Pattern.FAIL;
                return -1;
            }

            // These functions generate explanation strings for the user.
            // Call .explain() to get the right one.

            private string opToString(int a, Operator op, int b, int result) {
                return a + " " + op.Token + " " + b + " = " + result + "\n";
            }


            private string explainSequential() {
                string explanation = "";
                explanation += opToString(values[0], operators[0], values[1], stepResults[0]);
                explanation += opToString(stepResults[0], operators[1], values[2], stepResults[1]);
                explanation += opToString(stepResults[1], operators[2], values[3], stepResults[2]);
                explanation += "Score = " + score + "\n\n";
                return explanation;
            }

            private string explainPaired() {
                string explanation = "";
                explanation += opToString(values[0], operators[0], values[1], stepResults[0]);
                explanation += opToString(values[2], operators[2], values[3], stepResults[1]);
                explanation += opToString(stepResults[0], operators[2], stepResults[1], stepResults[2]);
                explanation += "Score = " + score + "\n\n";
                return explanation;
            }

            private string explainCentric() {
                string explanation = "";
                explanation += opToString(values[1], operators[1], values[2], stepResults[0]);
                explanation += opToString(values[0], operators[0], stepResults[0], stepResults[1]);
                explanation += opToString(stepResults[1], operators[2], values[3], stepResults[2]);
                explanation += "Score = " + score + "\n\n";
                return explanation;
            }

            public string explain() {
                switch (pattern) {
                    case Pattern.SEQUENTIAL: return explainSequential();
                    case Pattern.PAIRED:     return explainPaired();
                    case Pattern.CENTRIC:    return explainCentric();
                }
                return "No solution found!";
            }
        }

        // Generate a list of all non-failed solutions
        List<Solution> generateSolutions(int[] inputSet) {
            List<int[]> combos = generateCombinations(inputSet);
            List<Solution.Pattern> patterns = new List<Solution.Pattern>(Enum.GetValues(typeof(Solution.Pattern)).Cast<Solution.Pattern>());
            patterns.Remove(Solution.Pattern.FAIL);

            List<Solution> solutions = new List<Solution>();
            foreach(var set in combos) {
                Solution sol;
                foreach(var opSet in Operators.combinations) {
                    foreach(Solution.Pattern pattern in patterns) {
                        sol = new Solution(pattern, opSet, set);
                        if (sol.pattern != Solution.Pattern.FAIL) { solutions.Add(sol); }
                    }
                }
            }

            return solutions;
        }

        // Return the solution with the highest score.
        public Solution findBestSolution(int[] inputSet) {
            List<Solution> candidates = generateSolutions(inputSet);
            Solution winner = new Solution { pattern = Solution.Pattern.FAIL, score = -1 };

            foreach (var sol in candidates) {
                if(sol.score > winner.score) { winner = sol; }
            }

            return winner;
        }

    }
}
