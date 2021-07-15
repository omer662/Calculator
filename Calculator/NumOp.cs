using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    class NumOp : IOp
    {
        private int num;

        public NumOp(int n)
        {
            this.num = n;
        }

        public double Calculate(double x)
        {
            return this.num;
        }

        public IOp GetDerivative()
        {
            return new NumOp(0);
        }

        public IOp Clean()
        {
            return this;
        }

        public string GetSign() { return num.ToString() + " ";  }

        public override string ToString()
        {
            return GetSign();
        }

        public string GetFinalResult(double x)
        {
            return this.ToString() + "= " + this.Calculate(x).ToString();
        }
    }
}
