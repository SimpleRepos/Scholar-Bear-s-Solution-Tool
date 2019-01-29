using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    //A class representing a valid solution for an input set.
    //This may or may not be the optimal solution. Its score should be
    //compared with other available solutions to find out.
    class Solution {
        public Solution(TestSet testSet) {
            set = testSet;
            result = set.Invoke();
        }

        public string Explain() {
            return set.pattern.Explain(set.operators, set.values, result);
        }

        public int score { get { return result.score; } }

        readonly Result result;
        readonly TestSet set;
    }
}
