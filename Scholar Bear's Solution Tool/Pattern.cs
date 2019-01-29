using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    //A class representing the order of operations to use in a solution.
    //Contains a dictionary of possible patterns.
    class Pattern {
        Pattern(Func<OpSet, ValueSet, int[]> invoke, Func<OpSet, ValueSet, Result, string> explain) {
            Invoke = invoke;
            Explain = explain;
        }

        public readonly Func<OpSet, ValueSet, int[]> Invoke;

        public readonly Func<OpSet, ValueSet, Result, string> Explain;

        //Pattern.patterns is a dictionary of the available patterns

        public enum Name {
            SEQUENTIAL,  //((a.b).c).d
            PAIRED,      //(a.b).(c.d)
            CENTRIC,     //(a.(b.c)).d
            TAILCENTRIC, //a.((b.c).d)
            REVERSED     //a.(b.(c.d))
        }

        public static readonly Dictionary<Name, Pattern> patterns;

        static Pattern() {
            patterns = new Dictionary<Name, Pattern>();

            string BadResultExplanation = "Failed!";

            int[] sequentialInvoke(OpSet ops, ValueSet values) {
                int[] steps = new int[3];
                steps[0] = ops[0].Invoke(values[0], values[1]);
                if(steps[0] == -1) { return null; }
                steps[1] = ops[1].Invoke(steps[0], values[2]);
                if(steps[1] == -1) { return null; }
                steps[2] = ops[2].Invoke(steps[1], values[3]);
                if(steps[2] != 10) { return null; }
                return steps;
            }

            string sequentialExplain(OpSet ops, ValueSet values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[0], ops[0], values[1], sult.steps[0]);
                exp += Operator.Explain(sult.steps[0], ops[1], values[2], sult.steps[1]);
                exp += Operator.Explain(sult.steps[1], ops[2], values[3], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.SEQUENTIAL, new Pattern(sequentialInvoke, sequentialExplain));

            int[] pairedInvoke(OpSet ops, ValueSet values) {
                int[] steps = new int[3];
                steps[0] = ops[0].Invoke(values[0], values[1]);
                if(steps[0] == -1) { return null; }
                steps[1] = ops[2].Invoke(values[2], values[3]);
                if(steps[1] == -1) { return null; }
                steps[2] = ops[1].Invoke(steps[0], steps[1]);
                if(steps[2] != 10) { return null; }
                return steps;
            }

            string pairedExplain(OpSet ops, ValueSet values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[0], ops[0], values[1], sult.steps[0]);
                exp += Operator.Explain(values[2], ops[2], values[3], sult.steps[1]);
                exp += Operator.Explain(sult.steps[0], ops[1], sult.steps[1], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.PAIRED, new Pattern(pairedInvoke, pairedExplain));

            int[] centricInvoke(OpSet ops, ValueSet values) {
                int[] steps = new int[3];
                steps[0] = ops[1].Invoke(values[1], values[2]);
                if (steps[0] == -1) { return null; }
                steps[1] = ops[0].Invoke(values[0], steps[0]);
                if (steps[1] == -1) { return null; }
                steps[2] = ops[2].Invoke(steps[1], values[3]);
                if (steps[2] != 10) { return null; }
                return steps;
            }

            string centricExplain(OpSet ops, ValueSet values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[1], ops[1], values[2], sult.steps[0]);
                exp += Operator.Explain(values[0], ops[0], sult.steps[0], sult.steps[1]);
                exp += Operator.Explain(sult.steps[1], ops[2], values[3], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.CENTRIC, new Pattern(centricInvoke, centricExplain));

            int[] tailCentricInvoke(OpSet ops, ValueSet values) {
                int[] steps = new int[3];
                steps[0] = ops[1].Invoke(values[1], values[2]);
                if (steps[0] == -1) { return null; }
                steps[1] = ops[2].Invoke(steps[0], values[3]);
                if (steps[1] == -1) { return null; }
                steps[2] = ops[0].Invoke(values[0], steps[1]);
                if (steps[2] != 10) { return null; }
                return steps;
            }

            string tailCentricExplain(OpSet ops, ValueSet values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[1], ops[1], values[2], sult.steps[0]);
                exp += Operator.Explain(sult.steps[0], ops[2], values[3], sult.steps[1]);
                exp += Operator.Explain(values[0], ops[0], sult.steps[1], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.TAILCENTRIC, new Pattern(tailCentricInvoke, tailCentricExplain));

            int[] reversedInvoke(OpSet ops, ValueSet values) {
                int[] steps = new int[3];
                steps[0] = ops[2].Invoke(values[2], values[3]);
                if (steps[0] == -1) { return null; }
                steps[1] = ops[1].Invoke(values[1], steps[0]);
                if (steps[1] == -1) { return null; }
                steps[2] = ops[0].Invoke(values[0], steps[1]);
                if (steps[2] != 10) { return null; }
                return steps;
            }

            string reversedExplain(OpSet ops, ValueSet values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[2], ops[2], values[3], sult.steps[0]);
                exp += Operator.Explain(values[1], ops[1], sult.steps[0], sult.steps[1]);
                exp += Operator.Explain(values[0], ops[0], sult.steps[1], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.REVERSED, new Pattern(reversedInvoke, reversedExplain));
        }

    }
}







