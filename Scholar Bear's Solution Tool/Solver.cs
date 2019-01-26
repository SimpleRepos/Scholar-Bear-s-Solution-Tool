using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    class Solver {

        private List<int[]> generateCombinations(int[] inputSet) {
            int a = inputSet[0];
            int b = inputSet[1];
            int c = inputSet[2];
            int d = inputSet[3];

            return new List<int[]>() {
                new int[4] { a, b, c, d },
                new int[4] { a, b, d, c },
                new int[4] { a, c, b, d },
                new int[4] { a, c, d, b },
                new int[4] { a, d, b, c },
                new int[4] { a, d, c, b },
                new int[4] { b, a, c, d },
                new int[4] { b, a, d, c },
                new int[4] { b, c, a, d },
                new int[4] { b, c, d, a },
                new int[4] { b, d, a, c },
                new int[4] { b, d, c, a },
                new int[4] { c, a, b, d },
                new int[4] { c, a, d, b },
                new int[4] { c, b, a, d },
                new int[4] { c, b, d, a },
                new int[4] { c, d, a, b },
                new int[4] { c, d, b, a },
                new int[4] { d, a, b, c },
                new int[4] { d, a, c, b },
                new int[4] { d, b, a, c },
                new int[4] { d, b, c, a },
                new int[4] { d, c, a, b },
                new int[4] { d, c, b, a }
            };
        }

        public struct Solution {
            public enum Pattern { FAIL, SEQUENTIAL, PAIRED, CENTRIC };
            public Pattern pattern;
            public Operator[] operators;
            public int[] values;
            public int score;

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

        List<Solution> generateSolutions(int[] inputSet) {
            var combos = generateCombinations(inputSet);

            List<Solution> solutions = new List<Solution>();
            Solution sol;
            foreach(var set in combos) {
                foreach(var opSet in Operators.operatorSets) {
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
