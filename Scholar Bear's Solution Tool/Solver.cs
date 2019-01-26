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

            public Pattern pattern;
            public Operator[] operators;
            public int[] values;
            public int score;
            
            // These functions generate explanation strings for the user.
            // Call .explain() to get the right one.

            private string explainSequential() {
                int a = operators[0].Invoke(values[0], values[1]);
                int b = operators[1].Invoke(a, values[2]);
                int c = operators[2].Invoke(b, values[3]);

                string explanation = "";
                explanation += values[0] + " " + operators[0].Token + " " + values[1] + " = " + a + "\n";
                explanation += a + " " + operators[1].Token + " " + values[2] + " = " + b + "\n";
                explanation += b + " " + operators[2].Token + " " + values[3] + " = " + c + "\n";
                explanation += "Score = " + score + "\n\n";

                return explanation;
            }

            private string explainPaired() {
                int a = operators[0].Invoke(values[0], values[1]);
                int b = operators[2].Invoke(values[2], values[3]);
                int c = operators[1].Invoke(a, b);

                string explanation = "";
                explanation += values[0] + " " + operators[0].Token + " " + values[1] + " = " + a + "\n";
                explanation += values[2] + " " + operators[2].Token + " " + values[3] + " = " + b + "\n";
                explanation += a + " " + operators[1].Token + " " + b + " = " + c + "\n";
                explanation += "Score = " + score + "\n\n";

                return explanation;
            }

            private string explainCentric() {
                int a = operators[1].Invoke(values[1], values[2]);
                int b = operators[0].Invoke(values[0], a);
                int c = operators[2].Invoke(b, values[3]);

                string explanation = "";
                explanation += values[1] + " " + operators[1].Token + " " + values[2] + " = " + a + "\n";
                explanation += values[0] + " " + operators[0].Token + " " + a + " = " + b + "\n";
                explanation += b + " " + operators[2].Token + " " + values[3] + " = " + c + "\n";
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

        // These functions execute the indicated pattern on the values and operators
        // provided and return the resulting solution object

        private Solution doSequential(Operator[] ops, int[] set) {
            Solution sol;
            sol.pattern = Solution.Pattern.SEQUENTIAL;
            sol.operators = ops;
            sol.values = set;
            sol.score = 0;

            int a = ops[0].Invoke(set[0], set[1]);
            if (a == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += a;

            int b = ops[1].Invoke(a, set[2]);
            if (b == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += b;

            int c = ops[2].Invoke(b, set[3]);
            if (c == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += c;

            if (c != 10) { sol.pattern = Solution.Pattern.FAIL; }
            return sol;
        }

        private Solution doPaired(Operator[] ops, int[] set) {
            Solution sol;
            sol.pattern = Solution.Pattern.PAIRED;
            sol.operators = ops;
            sol.values = set;
            sol.score = 0;

            int a = ops[0].Invoke(set[0], set[1]);
            if (a == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += a;

            int b = ops[2].Invoke(set[2], set[3]);
            if (b == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += b;

            int c = ops[1].Invoke(a, b);
            if (c == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += c;

            if (c != 10) { sol.pattern = Solution.Pattern.FAIL; }
            return sol;
        }

        private Solution doCentric(Operator[] ops, int[] set) {
            Solution sol;
            sol.pattern = Solution.Pattern.CENTRIC;
            sol.operators = ops;
            sol.values = set;
            sol.score = 0;

            int a = ops[1].Invoke(set[1], set[2]);
            if (a == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += a;

            int b = ops[0].Invoke(set[0], a);
            if (b == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += b;

            int c = ops[2].Invoke(b, set[3]);
            if (c == -1) { sol.pattern = Solution.Pattern.FAIL; return sol; }
            sol.score += c;

            if (c != 10) { sol.pattern = Solution.Pattern.FAIL; }
            return sol;
        }


        // Generate a list of all non-failed solutions
        List<Solution> generateSolutions(int[] inputSet) {
            var combos = generateCombinations(inputSet);

            List<Solution> solutions = new List<Solution>();
            Solution sol;
            foreach(var set in combos) {
                foreach(var opSet in Operators.combinations) {
                    sol = doSequential(opSet, set);
                    if (sol.pattern != Solution.Pattern.FAIL) { solutions.Add(sol); }
                    sol = doPaired(opSet, set);
                    if (sol.pattern != Solution.Pattern.FAIL) { solutions.Add(sol); }
                    sol = doCentric(opSet, set);
                    if (sol.pattern != Solution.Pattern.FAIL) { solutions.Add(sol); }
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
