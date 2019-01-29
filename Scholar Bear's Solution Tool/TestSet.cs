using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//A class facilitating a collection of the objects required to attempt a solution
namespace Scholar_Bear_s_Solution_Tool {
    class TestSet {
        public TestSet(Pattern pat, OpSet ops, ValueSet vals) {
            pattern = pat;
            operators = ops;
            values = vals;
        }

        public Result Invoke() {
            int[] steps = pattern.Invoke(operators, values);
            return new Result(steps);
        }

        public readonly Pattern pattern;
        public readonly OpSet operators;
        public readonly ValueSet values;
    }
}
