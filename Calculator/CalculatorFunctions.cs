using System;
using System.Collections.Generic;

namespace Calculator
{
    class CalculatorFunctions
    {
        int pIndex;
        List<IOp> functions;
        List<char> names;

        public CalculatorFunctions()
        {
            pIndex = 0;
            functions = new List<IOp>();
            names = new List<char>();
        }

        public int GetRootIndex(string str)
        {
            int rIndex = -1;
            int pNum = 0;

            // Go from right to left to find current root action
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i].Equals(')')) // Make sure the action is NOT in an unwanted parentheses
                    pNum++;
                else if (str[i].Equals('('))
                    pNum--;

                // If +/- return instantly
                if ((str[i].Equals('+') || str[i].Equals('-')) && pNum == 0)
                    return i;

                // If * or / save index (in case no +/- is found)
                else if (((str[i].Equals('*') || str[i].Equals('/')) && rIndex == -1) && pNum == 0)
                    rIndex = i;
            }
            return rIndex;
        }

        public IOp NormalToTree(string str)
        {
            int opIndex = GetRootIndex(str); // Get root action index
            if (opIndex == -1) // If no action was found
            {
                int pStart = str.IndexOf('(');
                int pEnd = str.LastIndexOf(')');
                if (pStart == -1 && pEnd == -1) // If this substring is not parentheses (number)
                {
                    int n = 0;
                    // Get the number
                    for (int i = 0; i < str.Length; i++)
                    {
                        // If digit add to n
                        if (char.IsDigit(str[i]))
                        {
                            n *= 10;
                            n += int.Parse(str[i].ToString());
                        }
                        else if (str[i].Equals('x'))
                        {

                            // if only x return XOp
                            if (n == 0)
                                return new XOp();

                            // else, there is a multiplier for the XOp
                            else
                                return new MulOp(new NumOp(n), new XOp(), true);
                        }
                    }
                    return new NumOp(n); // Return new NumOp with value n
                }
                else // If this substring is a parentheses
                    return NormalToTree(str.Substring(pStart + 1, pEnd - pStart - 1));

            }
            IOp op1 = NormalToTree(str.Substring(0, opIndex));  // Get left part of operation
            IOp op2 = NormalToTree(str.Substring(opIndex + 1)); // Get right part of operation

            // return Op of same action type
            if (str[opIndex].Equals('+'))
                return new PlusOp(op1, op2, true);
            if (str[opIndex].Equals('-'))
                return new MinusOp(op1, op2, true);
            if (str[opIndex].Equals('/'))
                return new DivOp(op1, op2, true);
            return new MulOp(op1, op2, true);
        }

        public IOp PolishToTree(string str)
        {
            if (str[pIndex].Equals('x')) // If current pIndex contains 'x'
            {
                pIndex += 2;
                return new XOp();
            }
            else if (char.IsDigit(str[pIndex])) // If current pIndex contains a number
            {
                if (pIndex == str.Length - 1) // If biggest pIndex -> return only last digit
                    return new NumOp(int.Parse(str.Substring(pIndex)));

                // Get pIndex after number ends
                int nextSpace = str.IndexOf(" ", pIndex);
                int n;

                // If not found -> take from pIndex to end of string
                if (nextSpace == -1)
                    n = int.Parse(str.Substring(pIndex));

                // If found -> take from pIndex to one char before next space
                else
                    n = int.Parse(str.Substring(pIndex, nextSpace - pIndex));
                pIndex = nextSpace + 1;
                return new NumOp(n);
            }
            int actionpIndex = pIndex; // save location of current pIndex
            pIndex += 2;
            IOp op1 = PolishToTree(str); // Get IOp 1 for this IOp
            IOp op2 = PolishToTree(str); // Get IOp 2 for this IOp

            // Return IOp depending on char in saved pIndex (actionpIndex)
            if (str[actionpIndex].Equals('+'))
                return new PlusOp(op1, op2).Clean();
            else if (str[actionpIndex].Equals('-'))
                return new MinusOp(op1, op2).Clean();
            else if (str[actionpIndex].Equals('/'))
                return new DivOp(op1, op2).Clean();
            else
                return new MulOp(op1, op2).Clean();
        }

        public void Help()
        {
            Console.WriteLine(
                "To enter new functions: \n" +
                "newf <function name char>(x)\n" +
                "Enter the <function> after '<function name char>(x) = '\n" +
                "If the name already exists the previous function will be replaced.\n" +
                "Exmaples:\n" +
                "-> newf f(x)\n" +
                "f(x) = x + 3\n" +
                "-----------------------------------------------------------------------------------------------------\n" +
                "To show a function:\n" +
                "show <function name char><as many ' as you want>(x)'\n" +
                "Example:\n" +
                "-> show f(x)\n" +
                "x + 3\n" +
                "-> show f'(x)\n" +
                "1\n" +
                "-----------------------------------------------------------------------------------------------------\n" +
                "To calculate a function for a certain x value:\n" +
                "calc <function name><as many ' as you want>(<number>).\n" +
                "Example:\n" +
                "-> calc f(3)\n" +
                "f(3) = 6" +
                "-----------------------------------------------------------------------------------------------------\n" +
                "To solve a function for x value:\n" +
                "getx f(x)\n" +
                "Enter the value to solve for after 'x = '\n" +
                "Example:\n" +
                "-> getx f(x)\n" +
                "f(x) = 4\n" +
                "x = 1\n"
                );
        }

        public bool NewFunction(string input, bool normal)
        {
            if (input.Length < 6)
                return false;
            char fName = input[5];
            Console.Write(fName + "(x) = ");
            string funcStr = Console.ReadLine();
            IOp func;
            if (normal)
                func = NormalToTree(funcStr).Clean();
            else
                func = PolishToTree(funcStr).Clean();
            if (names.Contains(fName))
            {
                functions[names.IndexOf(fName)] = func;
            }
            else
            {
                functions.Add(func);
                names.Add(fName);
            }
            return true;
        }

        public bool ShowFunction(string input)
        {
            if (input.Length < 6)
                return false;
            char fName = input[5];
            if (!names.Contains(fName))
                Console.WriteLine("Function not found.");
            else
            {
                int derLevel = input.IndexOf('(') - 6; //Ignore command and the function's name
                IOp func = functions[names.IndexOf(fName)];
                for (int i = 0; i < derLevel; i++)
                    func = func.GetDerivative();
                Console.WriteLine(func.ToString());
            }
            return true;
        }

        public bool CalculateFunc(string input)
        {
            if (input.Length < 6)
                return false;
            char fName = input[5];
            if (!names.Contains(fName))
                Console.WriteLine("Function not found.");
            else
            {
                int derLevel = input.IndexOf('(') - 6;
                int val = int.Parse(input.Substring(7 + derLevel, input.IndexOf(')') - input.IndexOf('(') - 1));
                IOp func = functions[names.IndexOf(fName)];
                for (int i = 0; i < derLevel; i++)
                    func = func.GetDerivative();
                Console.WriteLine(func.Calculate(val));
            }
            return true;
        }
    }
}
