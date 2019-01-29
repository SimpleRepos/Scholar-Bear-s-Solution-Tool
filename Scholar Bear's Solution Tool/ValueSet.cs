using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//A simple class for holding a readonly set of values
namespace Scholar_Bear_s_Solution_Tool {
    class ValueSet {
        public ValueSet(int a, int b, int c, int d) {
            values = new int[4] { a, b, c, d };
        }

        private readonly int[] values;

        public int this[int index] {
            get { return values[index]; }
        }
    }
}
