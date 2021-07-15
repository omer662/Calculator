using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    class MulOp : IOp
    {
        private IOp op1;
        private IOp op2;
        private bool normal;

        public MulOp(IOp op1, IOp op2)
        {
            this.op1 = op1;
            this.op2 = op2;
            this.normal = false;
        }

        public MulOp(IOp op1, IOp op2, bool normal)
        {
            this.op1 = op1;
            this.op2 = op2;
            this.normal = normal;
        }

        public double Calculate(double x)
        {
            return op1.Calculate(x) * op2.Calculate(x);
        }

        public IOp GetDerivative()
        {
            return new PlusOp(new MulOp(op1.GetDerivative(), op2), new MulOp(op1, op2.GetDerivative())).Clean();
        }

        public IOp Clean()
        {
            IOp newOp1 = op1.Clean();
            IOp newOp2 = op2.Clean();
            if (newOp1 is NumOp && newOp2 is NumOp)
                return new NumOp((int)(newOp1.Calculate(0) * newOp2.Calculate(0)));
            else if ((newOp1 is NumOp && newOp1.Calculate(0) == 0) || (newOp2 is NumOp && newOp2.Calculate(0) == 0))
                return new NumOp(0);
            else if (newOp1 is NumOp && newOp1.Calculate(0) == 1)
                return newOp2;
            else if (newOp2 is NumOp)
                if (newOp2.Calculate(0) == 1)
                    return newOp1;
                else
                    return new MulOp(newOp2, newOp1, true);
            else
                return new MulOp(newOp1, newOp2, true);
        }

        public string GetSign() { return "* "; }

        public override string ToString()
        {
            if (normal)
            {
                string op1S = op1.ToString();
                string op2S = op2.ToString();
                if (!(op1 is NumOp || op1 is XOp))
                    op1S = "(" + op1S.Substring(0, op1S.Length - 1) + ") ";
                if (!(op2 is NumOp || op2 is XOp))
                    op2S = "(" + op2S.Substring(0, op2S.Length - 1) + ") ";
                return op1S + this.GetSign() + op2S;
            }
            return this.GetSign() + op1.ToString() + op2.ToString();
        }

        public string GetFinalResult(double x)
        {
            return ToString() + "= " + this.Calculate(x).ToString() + " ( x = " + x + " )";
        }
    }
}
