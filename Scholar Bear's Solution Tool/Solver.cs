using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    //The top-level class which encapsulates the solution-finding process.
    //A single instance can be invoked via findBestSolution() for multiple input sets.
    class Solver {
        // Generates a list of all possible combinations of the input values
        private List<ValueSet> generateCombinations(int[] inputSet) {
            return new List<ValueSet>() {
                new ValueSet(inputSet[0], inputSet[1], inputSet[2], inputSet[3]),
                new ValueSet(inputSet[0], inputSet[1], inputSet[3], inputSet[2]),
                new ValueSet(inputSet[0], inputSet[2], inputSet[1], inputSet[3]),
                new ValueSet(inputSet[0], inputSet[2], inputSet[3], inputSet[1]),
                new ValueSet(inputSet[0], inputSet[3], inputSet[1], inputSet[2]),
                new ValueSet(inputSet[0], inputSet[3], inputSet[2], inputSet[1]),
                new ValueSet(inputSet[1], inputSet[0], inputSet[2], inputSet[3]),
                new ValueSet(inputSet[1], inputSet[0], inputSet[3], inputSet[2]),
                new ValueSet(inputSet[1], inputSet[2], inputSet[0], inputSet[3]),
                new ValueSet(inputSet[1], inputSet[2], inputSet[3], inputSet[0]),
                new ValueSet(inputSet[1], inputSet[3], inputSet[0], inputSet[2]),
                new ValueSet(inputSet[1], inputSet[3], inputSet[2], inputSet[0]),
                new ValueSet(inputSet[2], inputSet[0], inputSet[1], inputSet[3]),
                new ValueSet(inputSet[2], inputSet[0], inputSet[3], inputSet[1]),
                new ValueSet(inputSet[2], inputSet[1], inputSet[0], inputSet[3]),
                new ValueSet(inputSet[2], inputSet[1], inputSet[3], inputSet[0]),
                new ValueSet(inputSet[2], inputSet[3], inputSet[0], inputSet[1]),
                new ValueSet(inputSet[2], inputSet[3], inputSet[1], inputSet[0]),
                new ValueSet(inputSet[3], inputSet[0], inputSet[1], inputSet[2]),
                new ValueSet(inputSet[3], inputSet[0], inputSet[2], inputSet[1]),
                new ValueSet(inputSet[3], inputSet[1], inputSet[0], inputSet[2]),
                new ValueSet(inputSet[3], inputSet[1], inputSet[2], inputSet[0]),
                new ValueSet(inputSet[3], inputSet[2], inputSet[0], inputSet[1]),
                new ValueSet(inputSet[3], inputSet[2], inputSet[1], inputSet[0])
            };
        }

        // Generate a list of all non-failed solutions
        List<Solution> generateSolutions(int[] inputSet) {
            List<Solution> solutions = new List<Solution>();

            foreach (var vals in generateCombinations(inputSet)) {
                foreach (OpSet ops in OpSet.opSets) {
                    foreach (Pattern pattern in Pattern.patterns.Values) {
                        Solution sol = new Solution(new TestSet(pattern, ops, vals));
                        if(sol != null) { solutions.Add(sol); }
                    }
                }
            }

            return solutions;
        }

        // Return the solution with the highest score.
        public Solution findBestSolution(int[] inputSet) {
            Solution winner = null;
            int hiScore = -1;

            foreach(var solution in generateSolutions(inputSet)) {
                if(solution.score > hiScore) {
                    hiScore = solution.score;
                    winner = solution;
                }
            }

            return winner;
        }
    }
}
