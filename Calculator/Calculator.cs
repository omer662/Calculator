using System;
using System.Collections.Generic;

namespace Calculator
{
    class Calculator
    {
        static CalculatorFunctions functions = new CalculatorFunctions();

        static void ProcessInput(string input)
        {
            string command = input.Substring(0, 4);
            if (command.ToLower().Equals("help"))
                functions.Help();
            else if (command.ToLower().Equals("newf"))
                functions.NewFunction(input);
            else if (command.ToLower().Equals("show"))
                functions.ShowFunction(input);
            else if (command.ToLower().Equals("calc"))
                functions.CalculateFunc(input);
        }

        static void Run()
        {
            Console.WriteLine("Welcome. Enter 'help' for explaination.");
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