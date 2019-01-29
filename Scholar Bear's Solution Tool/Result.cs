using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    //This class represents the result of executing a pattern on a set of values.
    //It exploses the steps taken, and can report the resulting score.
    class Result {
        public Result(int[] steps) {
            this.steps = steps;
        }

        public readonly int[] steps = new int[3];

        public int score {
            get {
                if (steps == null) { return -1; }
                return steps[0] + steps[1] + steps[2];
            }
        }
    }
}
