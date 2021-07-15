using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    interface IOp
    {
        public double Calculate(double x);

        public IOp GetDerivative();

        public IOp Clean();

        public string GetSign();

        public string ToString();

        public string GetFinalResult(double x);
    }
}
