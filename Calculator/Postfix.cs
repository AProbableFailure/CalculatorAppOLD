using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculator.Tokens;

namespace Calculator
{
    class Postfix
    {
        public List<IToken> PostfixExpression { get; set; }

        public Postfix(List<IToken> _postfixExpression)
        {
            PostfixExpression = _postfixExpression;
        }

        List<string> unaryOperators = new List<string>()
        {
            //"neg",
            //"sin", "cos", "tan",
            //"sind", "cosd", "tand",
            //"sqrt",
            "!",
            TokenLibrary.tokenLibrary["negative"],
            TokenLibrary.tokenLibrary["sine"], TokenLibrary.tokenLibrary["cosine"], TokenLibrary.tokenLibrary["tangent"],
            TokenLibrary.tokenLibrary["sine degrees"], TokenLibrary.tokenLibrary["cosine degrees"], TokenLibrary.tokenLibrary["tangent degrees"],
            TokenLibrary.tokenLibrary["square root"]
        };

        List<string> specialOperators = new List<string>()
        {
            TokenLibrary.tokenLibrary["summation"], TokenLibrary.tokenLibrary["integral"]
        };

        public double SolvedExpression(int cycle = 0)
        {
            //foreach (IToken token in PostfixExpression)
            //{
            //    Console.WriteLine(token.Token);
            //}

            Stack<IOperand> answerHolder = new Stack<IOperand>();

            foreach (IToken token in PostfixExpression)
            {
                if (token is Operand)
                {
                    answerHolder.Push((Operand)token);
                }
                else if (token is SpecialOperand)
                {
                    answerHolder.Push((SpecialOperand)token);
                }
                else if (token is SpecialOperatorContainer)
                {
                    //Console.WriteLine("gggg");
                    answerHolder.Push((SpecialOperatorContainer)token);
                }
                else if (token is SpecialOperator)
                {
                    SpecialOperator sop = (SpecialOperator)token;
                    
                    if (specialOperators.Contains(sop.OperatorToken))
                    {
                        //Console.WriteLine("Hiiii");
                        List<string> nonOperatorComponents = new List<string>();
                        for (int i = 1; i < sop.TokenComponents.Count; i++)
                        {
                            nonOperatorComponents.Add(sop.TokenComponents[i]);
                        }
                        //Console.WriteLine(sop.Token);
                        SpecialOperatorContainer a = (SpecialOperatorContainer)answerHolder.Pop();

                        answerHolder.Push(ApplySpecialOperator(sop.OperatorToken, nonOperatorComponents, a, cycle));
                    }
                }
                else
                {
                    Operator op = (Operator)token;

                    if (unaryOperators.Contains(token.Token))
                    {
                        IOperand a = answerHolder.Pop();
                        answerHolder.Push(ApplyUnaryOperator(a, op));
                    }
                    else
                    {
                        //Console.WriteLine("OP           " + op.Token);
                        IOperand b = answerHolder.Pop();
                        //Console.WriteLine("B           " + b.Token);
                        IOperand a = answerHolder.Pop();
                        answerHolder.Push(ApplyBinaryOperator(a, op, b));
                    }
                }

                //foreach (IOperand answer in answerHolder)
                //{
                //    Console.WriteLine("Processing:              " + answer.Token);
                //}
            }

            if (answerHolder.Count == 1)
                return answerHolder.Pop().TokenValue;//RoundAnswer(answerHolder.Pop().TokenValue); // I've gotta make this more accurate. I mean come on
            else
                return -10000;//1000000000000000;
        }

        private double RoundAnswer(double answer)
        {
            return Math.Abs(answer) < 1E-15 ? 0 : answer;

            //if (Math.Abs(answer) < 1E-15)
            //{
            //    return 0;
            //}
            //else
            //    return answer;
        }

        private Operand ApplyUnaryOperator(IOperand a, Operator op)
        {
            double returnValue;

            if (op.Token == TokenLibrary.tokenLibrary["negative"])
                returnValue = a.TokenValue * -1;
            else if (op.Token == TokenLibrary.tokenLibrary["sine"])
                returnValue = Math.Sin((double)a.TokenValue);
            else if (op.Token == TokenLibrary.tokenLibrary["cosine"])
                returnValue = Math.Cos((double)a.TokenValue);
            else if (op.Token == TokenLibrary.tokenLibrary["tangent"])
                returnValue = Math.Tan((double)a.TokenValue);
            else if (op.Token == TokenLibrary.tokenLibrary["sine degrees"])
                returnValue = Math.Sin(DegreesToRadians(a.TokenValue));
            else if (op.Token == TokenLibrary.tokenLibrary["cosine degrees"])
                returnValue = Math.Cos(DegreesToRadians(a.TokenValue));
            else if (op.Token == TokenLibrary.tokenLibrary["tangent degrees"])
                returnValue = Math.Tan(DegreesToRadians(a.TokenValue));
            else if (op.Token == TokenLibrary.tokenLibrary["square root"])
                returnValue = Math.Sqrt((double)a.TokenValue);
            else if (op.Token == "!")
            {
                if (a.TokenValue == 0)
                    returnValue = 1;
                else
                {
                    returnValue = a.TokenValue;
                    for (double i = a.TokenValue - 1; i >= 1; i--)
                    {
                        returnValue *= i;
                    }
                }
            }
            else
                returnValue = 0;
            //switch (op.Token)
            //{
            //    case "negative":
            //        returnValue = a.TokenValue * -1;
            //        break;
            //    case "sin":
            //        returnValue = (decimal)Math.Sin((double)a.TokenValue);
            //        break;
            //    case "cos":
            //        returnValue = (decimal)Math.Cos((double)a.TokenValue);
            //        break;
            //    case "tan":
            //        returnValue = (decimal)Math.Tan((double)a.TokenValue);
            //        break;
            //    case "sind":
            //        returnValue = (decimal)Math.Sin(DegreesToRadians(a.TokenValue));
            //        break;
            //    case "cosd":
            //        returnValue = (decimal)Math.Cos(DegreesToRadians(a.TokenValue));
            //        break;
            //    case "tand":
            //        returnValue = (decimal)Math.Tan(DegreesToRadians(a.TokenValue));
            //        break;
            //    case "sqrt":
            //        returnValue = (decimal)Math.Sqrt((double)a.TokenValue);
            //        break;
            //    default:
            //        returnValue = 0;
            //        break;
            //}

            return new Operand(returnValue);
        }

        private Operand ApplyBinaryOperator(IOperand a, Operator op, IOperand b)
        {
            double returnValue;
            switch (op.Token)
            {
                case "+":
                    returnValue = a.TokenValue + b.TokenValue;
                    break;
                case "-":
                    returnValue = a.TokenValue - b.TokenValue;
                    break;
                case "*":
                    returnValue = a.TokenValue * b.TokenValue;
                    break;
                case "/":
                    returnValue = a.TokenValue / b.TokenValue;
                    break;
                case "^":
                    returnValue = Math.Pow(a.TokenValue, b.TokenValue);
                    break;
                default:
                    returnValue = 0;
                    break;
            }
            return new Operand(returnValue);
        }

        private Operand ApplySpecialOperator(string op, List<string> nonOpComponents, SpecialOperatorContainer a, int cycle)
        {
            //Console.WriteLine("testing:                " + op);
            //switch (op)
            //{
            //    //case "sin":
            //    //    return new Operand((decimal)Math.Sin(Convert.ToDouble(nonOpComponents[0])));
            //    default:
            //        return new Operand(0);
            //}
            double returnValue = 0;
            string parsedA = a.Token.Replace("&i", "&1").Replace("&j", "&2").Replace("&k", "&3");

            if (op == TokenLibrary.tokenLibrary["summation"])
            {
                returnValue = Summation(nonOpComponents, parsedA, cycle);
                //int i = Convert.ToInt32(nonOpComponents[0]);
                //int top = Convert.ToInt32(nonOpComponents[1]);
                ////while (i <= top)
                ////{
                ////    i++;
                ////}
                //double addValue = 0;

                //if (!a.Token.Contains("&i"))
                //{
                //    //Postfix addValuePostfix = new Infix(a.Token).ToPostfix();
                //    addValue = new Infix(a.Token).ToPostfix().SolvedExpression();
                //}

                //for (; i <= top; i++)
                //{
                //    if (a.Token.Contains("&i"))
                //    {
                //        //Console.WriteLine(i);
                //        string newRawInfix = a.Token.Replace("&i", "&" + Convert.ToString(i) + "&");
                //        //Console.WriteLine(newRawInfix);
                //        addValue = new Infix(newRawInfix).ToPostfix().SolvedExpression();
                //    }

                //    returnValue += addValue;
                //}
            }
            else if (op == TokenLibrary.tokenLibrary["integral"])
            {
                returnValue = Integral(nonOpComponents, parsedA, cycle);
            }
            else
                returnValue = 0;

            return new Operand(returnValue);
        }

        #region Summation
        private double Summation(List<string> nonOpComponents, string a, int cycle = 0)
        {
            double returnValue = 0;
            int cycles = nonOpComponents.Count / 2;
            //while (i <= top)
            //{
            //    i++;
            //}
            //\I,0,#p\[\sin\&x]
            double addValue = 0;
            //Console.WriteLine(a);

            //\E,0,2,0,2\[2^\E,0,2\[&i&j&k]]

            //for (int i = 1; i <= Regex.Matches(Regex.Escape(a), Regex.Escape(@"\" + TokenLibrary.tokenLibrary["summation"])).Count + 1; i++)
            //{
            //    a = a.Replace("&" + i, "&" + i + ":");
            //}
            //foreach (Match match in Regex.Matches(Regex.Escape(a), Regex.Escape("&")))
            //{
            //    int index = match.Index + 2;
            //    Console.WriteLine("Index         " + index);
            //    a.Insert(match.Index + 2, ":");
            //}
            //int endVariableIndexBuffer = 0;
            //Console.WriteLine("A            " + a);
            //foreach (int index in Extensions.StringExtensions.AllIndexesOf(Regex.Escape(a), "&"))
            //{
            //    //Console.WriteLine("Index1               " +index);
            //    Console.WriteLine("EndIndex             " + a[index - 1]);// + 0 + endVariableIndexBuffer]);
            //    a.Insert(index - 0 + endVariableIndexBuffer, ":");
            //    endVariableIndexBuffer++;
            //}

            if (cycles <= 1)
            {
                int i = (int)tokenToDouble(nonOpComponents[0], cycle);
                int top = (int)tokenToDouble(nonOpComponents[1], cycle);
                //\E,0,2\[\E,0,&i\[4]]
                //\E,0,4,0,5\[&i^&j]
                //if (!a.Contains("&"))
                //{
                //    addValue = new Infix(a).ToPostfix().SolvedExpression();
                //}
                for (int j = 1; j <= Regex.Matches(Regex.Escape(a), Regex.Escape(@"\" + TokenLibrary.tokenLibrary["summation"])).Count + 1; j++)
                {
                    a = a.Replace("&" + j, "&" + j + ":");
                }

                //int nestedSummations = 1;

                for (; i <= top; i++)
                {
                    string parseString = a;
                    string newRawInfix = a;
                    //Console.WriteLine("ParseString1:              " + parseString);
                    //do
                    //{
                    //    int index = parseString.IndexOf(@"\" + TokenLibrary.tokenLibrary["summation"]);
                    //    if (index == -1)
                    //        index = 0;
                    //    else
                    //        index += 2;
                    //    //Console.WriteLine(index);
                    //    Console.WriteLine("Substring             "+parseString.Substring(index));
                    //    parseString = parseString.Substring(index);
                    //    newRawInfix = newRawInfix  .Replace(",&" + nestedSummations + ":,", "," + i + ",")
                    //                            .Replace(",&" + nestedSummations + @":\", "," + i + @"\")
                    //                            .Replace("&" + nestedSummations + ":", "&" + i + ";");
                    //    Console.WriteLine("NestedSummation             "+ nestedSummations);
                    //    nestedSummations++;
                    //    Console.WriteLine("ParseString:              " + parseString);
                    //    //Console.WriteLine()
                    //}
                    //while (parseString.Contains(@"\" + TokenLibrary.tokenLibrary["summation"]));

                    //for (int j = 1; j <= Regex.Matches(Regex.Escape(a), Regex.Escape(@"\" + TokenLibrary.tokenLibrary["summation"])).Count + 1; j++)
                    //{
                    //    //Console.WriteLine("h");
                    //    int index = parseString.IndexOf(@"\" + TokenLibrary.tokenLibrary["summation"]);
                    //    if (index == -1)
                    //        index = 0;
                    //    else
                    //        index += 2;
                    //    //Console.WriteLine(index);
                    //    //Console.WriteLine("Substring             " + parseString.Substring(index));
                    //    parseString = parseString.Substring(index);
                    //Console.WriteLine("Summation Raw Infix          " + newRawInfix);
                    //newRawInfix = newRawInfix   .Replace(",&" + j + ":,", "," + i + ",")
                    //                            .Replace(",&" + j + @":\", "," + i + @"\")
                    //                            .Replace("&" + j + ":", "&" + i + ";")
                    //                            .Replace(";;",";")
                    //                            .Replace(";:", ";"); 
                    newRawInfix = newRawInfix   .Replace(",&" + Convert.ToString(cycle + 1) + ":,", "," + i + ",")
                                                .Replace(",&" + Convert.ToString(cycle + 1) + @":\", "," + i + @"\")
                                                .Replace("&" + Convert.ToString(cycle + 1) + ":", "&" + i + ";")
                                                .Replace(";;", ";")
                                                .Replace(";:", ";")
                                                .Replace(":;", ";");
                    //Console.WriteLine("Summation Raw Infix New          " + newRawInfix);
                        //Console.WriteLine("NestedSummation             " + j);
                        //nestedSummations++;
                        //Console.WriteLine("ParseString:              " + parseString);
                    //}

                    //{
                    //\E,0,2\[\E,0,4\[&i&j]]
                    //\E,0,4\[&i]
                    //}
                    //Console.WriteLine("Value added");
                    addValue = new Infix(newRawInfix).ToPostfix().SolvedExpression(cycle + 1);

                    //\E,0,4\[\E,0,5\[&i^&j]]
                    //if (a.Contains("&1"))
                    //{
                    //    //Console.WriteLine(a);
                    //    string newRawInfix = a  .Replace(",&1,", "," + i + ",")
                    //                            .Replace(@",&1\", "," + i + @"\")
                    //                            .Replace("&1", "&" + i + ";");
                    //    //Console.WriteLine(newRawInfix);
                    //    //Console.WriteLine(newRawInfix);
                    //    addValue = new Infix(newRawInfix).ToPostfix().SolvedExpression();
                    //}

                    returnValue += addValue;
                }

                return returnValue;
            }
            else
            {
                List<int> ns = new List<int>();
                List<int> tops = new List<int>();
                List<int> index = new List<int>();
                for (int c = 0; c < cycles; c++)
                {
                    //{E,0,({I,0,#p}[{sin}&i]) }     [&i^3]
                    //Console.WriteLine("Testing:               " + nonOpComponents[2 * c]);
                    ns.Add((int)tokenToDouble(nonOpComponents[2 * c]));
                    tops.Add((int)tokenToDouble(nonOpComponents[2 * c]));
                }
                //\E,1,2,1,2\|&1&2|

                string parsedInfix = a;
                //Console.WriteLine("Parsed Infix Recursive              " + parsedInfix);
                for (int i = cycles + 1; i > 0; i--)
                {
                    parsedInfix = parsedInfix.Replace("&" + i, "&" + i + ":");
                }
                
                returnValue = RecursiveSummation(parsedInfix, ns, tops, /*index,*/ 0, cycles, cycle);
                //if (!a.Contains("&"))
                //{
                //    addValue = new Infix(a).ToPostfix().SolvedExpression();
                //}

                //for (int c = 0; c < cycles; c++)
                //{
                //    ns.Add(Convert.ToInt32(nonOpComponents[2 * c]));
                //    tops.Add(Convert.ToInt32(nonOpComponents[2 * c + 1]));
                //    string currentCycle = c.ToString();

                //    int currentN = ns[c];
                //    int currentTop = tops[c];

                //    if (!a.Contains("&" + c))
                //    {
                //        addValue = new Infix(a).ToPostfix().SolvedExpression();
                //    }

                //    for (; currentN <= currentTop; currentN++)
                //    {
                //        if (a.Contains("&" + c))
                //        {
                //            string newRawInfix = a.Replace("&" + c, "&" + currentN + "&");

                //        }
                //    }
                //}

                //returnValue = 1;
            }

            return returnValue;
        }

        private double RecursiveSummation(string workingInfix, List<int> ns, List<int> tops, /*List<int> index, List<int> limit,*/ int level, int cycles, int cycle = 0)
        {
            double returnValue = 0;

            if (level == cycles)
            {
                //if (!workingInfix.Contains("&" + (level + 1)))
                //{
                //Console.WriteLine("Done:                " + workingInfix);
                returnValue += new Infix(workingInfix).ToPostfix().SolvedExpression(cycle);
                //Console.WriteLine(returnValue);
                //}
            }
            else
            {
                int currentN = ns[level];
                
                //Console.WriteLine();
                //Console.WriteLine("You good? No              " + workingInfix);
                //Console.WriteLine();
                //for (index[level] = ns[level]; index[level] < tops[level]; index[level]++)
                for (; currentN <= tops[level]; currentN++)
                {
                    //Console.WriteLine("Current N:               " + currentN);
                    //Console.WriteLine("Level:              " + (level + 1));//"Loop:              " + workingInfix);
                    string newRawInfix = workingInfix.Replace("&" + (level + 1) + ":", "&" + currentN + ";").Replace(";;",";");//.Replace("&&&", "&&");
                    //Console.WriteLine(newRawInfix);
                    returnValue += RecursiveSummation(newRawInfix, ns, tops, /*index,*/ level + 1, cycles, cycle+1);
                    //Console.WriteLine(returnValue);
                }



                //Console.WriteLine();
                //Console.WriteLine("You good?              " + workingInfix);
                //Console.WriteLine();

                //\E,1,2,1,2\|&1&2|
            }

            return returnValue;
        }
        #endregion Summation

        #region Integral
        private double Integral(List<string> nonOpComponents, string a, int cycle = 0)
        {
            double returnValue = 0;
            //int cycles = nonOpComponents.Count / 2;
            //double bottom = Convert.ToDouble(nonOpComponents[0]);
            //double top = Convert.ToDouble(nonOpComponents[1]);
            double bottom = tokenToDouble(nonOpComponents[0], cycle);
            double top = tokenToDouble(nonOpComponents[1], cycle);
            //Console.WriteLine("top: " + top);
            double w = 5000;
            double n = 2*w;
            double h = (top + bottom) / n;

            for (int j = 1; j <= Regex.Matches(Regex.Escape(a), Regex.Escape(@"\" + TokenLibrary.tokenLibrary["summation"])).Count + 1; j++)
            {
                a = a.Replace("&" + j, "&" + j + ":");
            }

            //Console.WriteLine("h: " + h);
            double fa = new Infix(a.Replace("&1:", "(" + bottom + ")")).ToPostfix().SolvedExpression();
            double fb = new Infix(a.Replace("&1:", "(" + top + ")")).ToPostfix().SolvedExpression();
            //Console.WriteLine(fa);
            //Console.WriteLine("Fb:" + fb);

            List<string> twoSummation = new List<string>() { "1", Convert.ToString(n/2 - 1) };
            List<string> fourSummation = new List<string>() { "1", Convert.ToString(n / 2) };
            //foreach(string s in twoSummation)
            //{
            //    Console.WriteLine("test: " + s);
            //}
            //\I,0,3\[&x]
            string twoSummationInfix = a.Replace("&1:", "(" + bottom + "+" + "2*&1*" + h + ")");
            string fourSummationInfix = a.Replace("&1:", "(" + bottom + "+" + "(2*&1-1)*" + h + ")");
            //Console.WriteLine(summationInfix);
            returnValue = (h / 3) * (fa + fb + 2 * Summation(twoSummation, twoSummationInfix) + 4 * Summation(fourSummation, fourSummationInfix));

            return returnValue;
        }

        //private double RecursiveSummation(string workingInfix, List<int> bottoms, List<int> tops, int level, int cycles)
        //{

        //}
        #endregion Integral

        private double tokenToDouble(string token, int cycle = 0)
        {
            return new Infix(token).ToPostfix().SolvedExpression(cycle);
        }


        private double DegreesToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
