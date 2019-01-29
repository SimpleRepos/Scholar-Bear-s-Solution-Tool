using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    //A class representing an ordered set of three operators,
    //intended for use in a solution. Also contains a collection
    //of all possible sets.
    class OpSet {
        public OpSet(Operator a, Operator b, Operator c) {
            set = new Operator[3] { a, b, c };
        }

        readonly Operator[] set;

        public Operator this[int index] {
            get {
                return set[index];
            }
        }

        //A collection of all the possible combinations of operators.
        public static readonly OpSet[] opSets;

        static OpSet() {
            List<OpSet> sets = new List<OpSet>();

            foreach (var a in Operator.operators) {
                foreach (var b in Operator.operators) {
                    foreach (var c in Operator.operators) {
                        sets.Add(new OpSet(a.Value, b.Value, c.Value));
                    }
                }
            }

            opSets = sets.ToArray();
        }
    }
}
