using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    class XOp : IOp
    {

        public XOp() {}

        public double Calculate(double x)
        {
            return x;
        }

        public IOp GetDerivative()
        {
            return new NumOp(1);
        }

        public IOp Clean()
        {
            return this;
        }

        public string GetSign()
        {
            return "x ";
        }

        public override string ToString()
        {
            return GetSign();
        }

        public string GetFinalResult(double x)
        {
            return ToString() + "= " + this.Calculate(x).ToString();
        }

    }

}
