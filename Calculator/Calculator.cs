using System;
using System.Collections.Generic;

namespace Calculator
{
    class Calculator
    {
        static CalculatorFunctions functions = new CalculatorFunctions();
        static bool isNormal = true;

        static void ProcessInput(string input)
        {
            bool success = false;
            if (input.Length > 5)
            {
                string eType = input.Substring(0, 6);
                success = true;
                if (eType.Equals("normal"))
                    isNormal = true;
                else if (eType.Equals("polish"))
                    isNormal = false;
                else
                    success = false;
            }
            if (input.Length > 3 && !success)
            {
                string command = input.Substring(0, 4);
                success = true;
                if (command.ToLower().Equals("help"))
                    functions.Help();
                else if (command.ToLower().Equals("newf"))
                    success = functions.NewFunction(input, isNormal);
                else if (command.ToLower().Equals("show"))
                    success = functions.ShowFunction(input);
                else if (command.ToLower().Equals("calc"))
                    success = functions.CalculateFunc(input);
                else
                    success = false;
            }
            if (!success)
                Console.WriteLine("Illegal Request.");
        }

        static void Run()
        {
            Console.WriteLine("Welcome. Enter 'help' for explaination.\nCurrent expression type is normal, enter 'polish' or 'normal' to change");
            Console.Write("-> ");
            string inp = Console.ReadLine();
            while (!inp.Equals("exit"))
            {
                ProcessInput(inp);
                Console.Write("-> ");
                inp = Console.ReadLine();
            }
        }


        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Run();
        }
    }
}