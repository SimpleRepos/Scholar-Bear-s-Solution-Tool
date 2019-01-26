using System;
using System.Collections.Generic;

namespace Scholar_Bear_s_Solution_Tool {
    class Operator {
        public Operator(char token, Func<int, int, int> func) {
            Token = token;
            Invoke = func;
        }

        public readonly char Token;

        public readonly Func<int, int, int> Invoke;

    }

    static class Operators {
        public enum Op { ADD, SUB, MUL, DIV };

        public static readonly Dictionary<Op, Operator> ops;

        public static readonly List<Operator[]> operatorSets;

        private static List<Operator[]> generateOperatorSets() {
            List<Operator[]> sets = new List<Operator[]>();
            foreach (var a in ops) {
                foreach (var b in ops) {
                    foreach (var c in ops) {
                        sets.Add(new Operator[] { a.Value, b.Value, c.Value });
                    }
                }
            }

            return sets;
        }

        static Operators() {
            ops = new Dictionary<Op, Operator>() {
                { Op.ADD,
                    new Operator('+', (int a, int b) => {
                        return a + b;
                }) },
                { Op.SUB, new Operator('-', (int a, int b) => {
                    int sult = a - b;
                    if(sult < 0) { return -1; }
                    return sult;
                }) },
                { Op.MUL, new Operator('*', (int a, int b) => {
                    return a * b;
                }) },
                { Op.DIV, new Operator('/', (int a, int b) => {
                    if(b == 0) { return -1; }
                    int sult = Math.DivRem(a, b, out int r);
                    if(r != 0) { return -1; }
                    return sult;
                }) }
            };

            operatorSets = generateOperatorSets();
        }

    }
}
