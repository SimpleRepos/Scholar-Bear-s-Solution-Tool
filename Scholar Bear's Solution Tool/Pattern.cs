using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scholar_Bear_s_Solution_Tool {
    class Pattern {
        Pattern(Func<Operator[], int[], Result> invoke, Func<Operator[], int[], Result, string> explain) {
            Invoke = invoke;
            Explain = explain;
        }

        public enum Name { SEQUENTIAL, PAIRED, CENTRIC, TAILCENTRIC, REVERSED }

        public readonly Func<Operator[], int[], Result> Invoke;

        public readonly Func<Operator[], int[], Result, string> Explain;

        public class Result {
            public int[] steps = new int[3];

            public static readonly Result BAD = new Result() { steps = null };

            public int score {
                get {
                    if (steps == null) { return -1; }
                    return steps[0] + steps[1] + steps[2];
                }
            }
        }

        public static readonly Dictionary<Name, Pattern> patterns;

        static Pattern() {
            patterns = new Dictionary<Name, Pattern>();

            string BadResultExplanation = "Failed!";

            Result sequentialInvoke(Operator[] ops, int[] values) {
                Result sult = new Result();
                sult.steps[0] = ops[0].Invoke(values[0], values[1]);
                if(sult.steps[0] == -1) { return Result.BAD; }
                sult.steps[1] = ops[1].Invoke(sult.steps[0], values[2]);
                if(sult.steps[1] == -1) { return Result.BAD; }
                sult.steps[2] = ops[2].Invoke(sult.steps[1], values[3]);
                if(sult.steps[2] != 10) { return Result.BAD; }
                return sult;
            }

            string sequentialExplain(Operator[] ops, int[] values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[0], ops[0], values[1], sult.steps[0]);
                exp += Operator.Explain(sult.steps[0], ops[1], values[2], sult.steps[1]);
                exp += Operator.Explain(sult.steps[1], ops[2], values[3], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.SEQUENTIAL, new Pattern(sequentialInvoke, sequentialExplain));

            Result pairedInvoke(Operator[] ops, int[] values) {
                Result sult = new Result();
                sult.steps[0] = ops[0].Invoke(values[0], values[1]);
                if(sult.steps[0] == -1) { return Result.BAD; }
                sult.steps[1] = ops[2].Invoke(values[2], values[3]);
                if(sult.steps[1] == -1) { return Result.BAD; }
                sult.steps[2] = ops[1].Invoke(sult.steps[0], sult.steps[1]);
                if(sult.steps[2] != 10) { return Result.BAD; }
                return sult;
            }

            string pairedExplain(Operator[] ops, int[] values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[0], ops[0], values[1], sult.steps[0]);
                exp += Operator.Explain(values[2], ops[2], values[3], sult.steps[1]);
                exp += Operator.Explain(sult.steps[0], ops[1], sult.steps[1], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.PAIRED, new Pattern(pairedInvoke, pairedExplain));

            Result centricInvoke(Operator[] ops, int[] values) {
                Result sult = new Result();
                sult.steps[0] = ops[1].Invoke(values[1], values[2]);
                if (sult.steps[0] == -1) { return Result.BAD; }
                sult.steps[1] = ops[0].Invoke(values[0], sult.steps[0]);
                if (sult.steps[1] == -1) { return Result.BAD; }
                sult.steps[2] = ops[2].Invoke(sult.steps[1], values[3]);
                if (sult.steps[2] != 10) { return Result.BAD; }
                return sult;
            }

            string centricExplain(Operator[] ops, int[] values, Result sult) {
                if(sult.steps == null) { return BadResultExplanation; }

                string exp = "";
                exp += Operator.Explain(values[1], ops[1], values[2], sult.steps[0]);
                exp += Operator.Explain(values[0], ops[0], sult.steps[0], sult.steps[1]);
                exp += Operator.Explain(sult.steps[1], ops[2], values[3], sult.steps[2]);
                exp += "Score: " + sult.score + "\n";
                return exp;
            }

            patterns.Add(Name.CENTRIC, new Pattern(centricInvoke, centricExplain));

            //~~_ TAILCENTRIC
            //~~_ REVERSED
        }
    }
}
