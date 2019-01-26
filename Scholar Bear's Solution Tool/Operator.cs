using System;
using System.Collections.Generic;

namespace Scholar_Bear_s_Solution_Tool {
    class Operator {
        public enum Name { ADD, SUB, MUL, DIV };

        public Operator(char token, Func<int, int, int> invoke) {
            Token = token;
            Invoke = invoke;
        }

        public readonly char Token;

        public readonly Func<int, int, int> Invoke;

        public static readonly Dictionary<Name, Operator> operators;

        public static string Explain(int a, Operator op, int b, int sult) {
            return a + " " + op.Token + " " + b + " = " + sult + "\n";
        }

        static Operator() {
            operators = new Dictionary<Name, Operator>();

            operators.Add(
                Name.ADD,
                new Operator('+', (int a, int b) => {
                    return a + b;
                })
            );

            operators.Add(
                Name.SUB,
                new Operator('-', (int a, int b) => {
                    int sult = a - b;
                    if(sult < 0) { sult = -1; }
                    return sult;
                })
            );

            operators.Add(
                Name.MUL,
                new Operator('*', (int a, int b) => {
                    return a * b;
                })
            );

            operators.Add(
                Name.DIV,
                new Operator('/', (int a, int b) => {
                    int sult = -1;
                    if(b != 0) {
                        sult = Math.DivRem(a, b, out int r);
                        if(r != 0) { sult = -1; }
                    }
                    return sult;
                })
            );
        }
    }

    /*
    private static List<Operator[]> generateCombinations() {
        List<Operator[]> sets = new List<Operator[]>();
        foreach (var a in Operator.operators) {
            foreach (var b in Operator.operators) {
                foreach (var c in Operator.operators) {
                    sets.Add(new Operator[] { a.Value, b.Value, c.Value });
                }
            }
        }

        return sets;
    }
    */
}
