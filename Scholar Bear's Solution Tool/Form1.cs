﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scholar_Bear_s_Solution_Tool {
    public partial class Form1 : Form {
        private Solver solver;

        public Form1() {
            InitializeComponent();
            solver = new Solver();
        }

        private int[] getSanitizedInput() {
            string valueString = maskedTextBox1.Text + maskedTextBox2.Text + maskedTextBox3.Text + maskedTextBox4.Text;
            if (valueString.Length != 4) {
                return null;
            }

            List<int> values = new List<int>();
            foreach (char c in valueString) {
                values.Add((int)Char.GetNumericValue(c));
            }

            return values.ToArray();
        }

        private void SolveButton_Click(object sender, EventArgs e) {
            var values = getSanitizedInput();

            string output = "Enter values and click \"Solve!\"\n\n<Invalid input!>";
            if (values != null) {
                var solution = solver.findBestSolution(values);
                output = solution.explain();
            }

            label1.Text = output;
        }
    }
}

