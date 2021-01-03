using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Tokens;

namespace Calculator
{
    class Program
    {
        const string breakLine = "____________________________________________________________";

        static void Main(string[] args)
        {
            //Console.WriteLine(breakLine);
            Console.WriteLine();
            //do
            while (true)
            {
                Console.Write("Input Calculation: ");
                Infix infix = new Infix(Console.ReadLine());
                Postfix postfix = infix.ToPostfix();

                if (infix.RawInfix == "")
                {
                    break;
                }

                bool debug = false;
                //debug = true;

                bool checkInfix = false;
                //checkInfix = true;
                //Console.WriteLine(infix.TokenizedInfix.ToArray());
                if (debug)
                {
                    if (checkInfix)
                    {
                        foreach (IToken token in infix.InfixTokens)
                        {
                            //if (token is Operand)
                            //{
                            //    Operand operand = (Operand)token;
                            //    if (operand.Negative)
                            //        Console.WriteLine("-" + token.Token);
                            //    else
                            //        Console.WriteLine(token.Token);
                            //}
                            //else
                            Console.WriteLine("Debug:               " + token.Token);
                            //if (token is Operator)
                            //{
                            //    Console.WriteLine("operator:                        " + token.Token);
                            //}
                        }
                    }
                    else
                    {
                        foreach (IToken token in postfix.PostfixExpression)
                        {
                            //Console.WriteLine("bazinga");
                            Console.WriteLine("Debug:                "+ token.Token);
                        }

                        //Console.WriteLine();
                        //Console.WriteLine("Answer: " + postfix.SolvedExpression());
                    }
                }
                Console.WriteLine("Answer: " + postfix.SolvedExpression());

                //if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                //{
                //    break;
                //}
                //else
                //{
                    //Console.WriteLine();
                    Console.WriteLine(breakLine);
                    //Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine();
                //}
            }// while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));
        }


        //\E,0,200\|((-1)^&i)*#p^(2&i+1)/((2&i+1)!)|
    }
}
