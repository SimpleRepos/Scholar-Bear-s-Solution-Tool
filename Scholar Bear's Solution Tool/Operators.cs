using System;
using System.Collections.Generic;

namespace Scholar_Bear_s_Solution_Tool {
    // This is a short class used to represent the individual operators.
    // Each operator object will encapsulate the printable token representing the operator
    // as well as an invokable function that executes the operation.
    // These are to be stored in a dictionary which is keyed with enums naming the operators.
    class Operator {
        public enum Name { ADD, SUB, MUL, DIV };

        public Operator(char token, Func<int, int, int> func) {
            Token = token;
            Invoke = func;
        }

        public readonly char Token;

        public readonly Func<int, int, int> Invoke;
    }

    // This static class encapsulates the operator objects and provides
    // access to list of all possible operator combinations (of length 3).
    static class Operators {
        public static readonly Dictionary<Operator.Name, Operator> operators;

        public static readonly List<Operator[]> combinations;

        private static List<Operator[]> generateCombinations() {
            List<Operator[]> sets = new List<Operator[]>();
            foreach (var a in operators) {
                foreach (var b in operators) {
                    foreach (var c in operators) {
                        sets.Add(new Operator[] { a.Value, b.Value, c.Value });
                    }
                }
            }

            return sets;
        }

        static Operators() {
            //Instantiate operators
            operators = new Dictionary<Operator.Name, Operator>() {
                { Operator.Name.ADD,
                    new Operator('+', (int a, int b) => {
                        return a + b;
                }) },
                { Operator.Name.SUB, new Operator('-', (int a, int b) => {
                    int sult = a - b;
                    if(sult < 0) { return -1; }
                    return sult;
                }) },
                { Operator.Name.MUL, new Operator('*', (int a, int b) => {
                    return a * b;
                }) },
                { Operator.Name.DIV, new Operator('/', (int a, int b) => {
                    if(b == 0) { return -1; }
                    int sult = Math.DivRem(a, b, out int r);
                    if(r != 0) { return -1; }
                    return sult;
                }) }
            };

            combinations = generateCombinations();
        }

    }
}
