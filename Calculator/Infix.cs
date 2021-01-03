using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculator.Tokens;

namespace Calculator
{
    class Infix
    {
        public string RawInfix { get; set; }
        public List<IToken> InfixTokens { get; set; }

        const string specialOperatorSymbol = "{"; // "\"
        const string specialOperatorEndSymbol = "}";
        const string specialOperandSingleSymbol = @"#";
        const string specialOperatorContainerSymbol = @"[";
        const string specialOperatorContainerEndSymbol = @"]";
        const string specialOperatorVariableSymbol = @"&";
        const string specialOperatorVariableEndSymbol = @";";

        List<string> operandTokens = new List<string>()
        {
            "1","2","3","4","5","6","7","8","9","0",
            ".", "E"
        };

        //List<string> nonOperandTokens = new List<string>()
        //{
        //    "(",")","+","-","*","/","^",
        //    specialOperatorSymbol, specialOperandSingleSymbol, specialOperatorContainerSymbol, specialOperatorVariableSymbol,
        //    //TokenLibrary.tokenLibrary("negative"),
        //    //TokenLibrary.tokenLibrary("sine"), TokenLibrary.tokenLibrary("cosine"), TokenLibrary.tokenLibrary("tangent"),
        //    //TokenLibrary.tokenLibrary("sine degrees"), TokenLibrary.tokenLibrary("cosine degrees"), TokenLibrary.tokenLibrary("tangent degrees"),
        //    //TokenLibrary.tokenLibrary("square root")
        //    TokenLibrary.tokenLibrary["negative"],
        //    TokenLibrary.tokenLibrary["sine"], TokenLibrary.tokenLibrary["cosine"], TokenLibrary.tokenLibrary["tangent"],
        //    TokenLibrary.tokenLibrary["sine degrees"], TokenLibrary.tokenLibrary["cosine degrees"], TokenLibrary.tokenLibrary["tangent degrees"],
        //    TokenLibrary.tokenLibrary["square root"]
        //    //"negative",
        //    //"sin", "cos", "tan",
        //    //"sind", "cosd", "tand",
        //    //"sqrt"
        //};

        List<string> specialOperators = new List<string>()
        {
            TokenLibrary.tokenLibrary["summation"], TokenLibrary.tokenLibrary["integral"]
        };

        List<string> specialOperands = new List<string>()
        {
            TokenLibrary.tokenLibrary["operand pi"], TokenLibrary.tokenLibrary["operand e"], TokenLibrary.tokenLibrary["operand golden ratio"],
            TokenLibrary.tokenLibrary["operand square root 2"], TokenLibrary.tokenLibrary["operand square root 3"]
            //"pi", "e"
        };

        List<string> specialOperandSingles = new List<string>()
        {
            TokenLibrary.tokenLibrary["single operand pi"], TokenLibrary.tokenLibrary["single operand e"], TokenLibrary.tokenLibrary["single operand golden ratio"],
            TokenLibrary.tokenLibrary["single operand square root 2"], TokenLibrary.tokenLibrary["single operand square root 3"]
            //"p", "e"
        };

        public Infix(string _rawInfix)
        {
            RawInfix = _rawInfix;
            InfixTokens = TokenizeInfix(SplitString(RemoveWhiteSpace(_rawInfix)));
            //foreach (string s in SplitString(RemoveWhiteSpace(_rawInfix)))
            //    Console.Write(s);
            //foreach (IToken token in Tokens)
            //{
            //    Console.WriteLine(token.Token);
            //}
        }

        private string RemoveWhiteSpace(string s)
        {
            return string.Concat(s.Where(c => !char.IsWhiteSpace(c)));
        }

        private List<string> SplitString(string s)
        {
            return s.ToCharArray()
                .Select(c => c.ToString())
                .ToList();
        }

        private List<IToken> TokenizeInfix(List<string> _infix)
        {
            _infix.Insert(0, "(");
            _infix.Add(")");

            Stack<string> operandHolder = new Stack<string>();
            //Stack<string> operatorHolder = new Stack<string>();

            List<IToken> tokens = new List<IToken>();
            
            //int parenthesisBufferIndex = 0;

            for (int i = 0; i < _infix.Count; i++)
            {
                string c = _infix[i];
                //Console.WriteLine();
                //foreach (string s in operandHolder)
                //{
                //    Console.Write("holder:                " + s);
                //    Console.WriteLine();
                //}
                //if (!nonOperandTokens.Contains(c)) //if c is a number
                //{
                //    operandHolder.Push(c);
                //}
                if (c == specialOperatorSymbol)
                {
                    if (operandHolder.Count > 0)
                    {
                        List<string> operandStringHolder = new List<string>();
                        while (operandHolder.Count > 0)
                        {
                            operandStringHolder.Add(operandHolder.Pop());
                        }
                        operandStringHolder.Reverse();
                        tokens.Add(new Operand(operandStringHolder));
                        //Console.WriteLine("fffff: " + c);
                    }

                    string joinedInfix = string.Join("", _infix);
                    int endIndex = joinedInfix.IndexOf(specialOperatorEndSymbol, i + 1);
                    string op = joinedInfix.Substring(i + 1, endIndex - i - 1);
                    int bracketBuffer = 0;

                    while (Regex.Matches(Regex.Escape(op), Regex.Escape(specialOperatorSymbol)).Count > bracketBuffer)
                    {
                        bracketBuffer++;
                        endIndex = joinedInfix.IndexOf(specialOperatorEndSymbol, endIndex + 1);
                        op = joinedInfix.Substring(i + 1, endIndex - i - 1);
                    }

                    //Console.WriteLine("T                 " + op);
                    if (op != "")
                    {
                        if (specialOperands.Contains(op)) //is a special operand
                        {
                            //operandHolder.Push(op);
                            //Console.WriteLine(tokens.Count - 2);
                            if (tokens.Count > 0)
                            {
                                IToken previousToken = tokens[tokens.Count - 1];

                                //Console.WriteLine("Prev:                " + previousToken.Token);
                                if (previousToken is Operand || previousToken is SpecialOperand)
                                {
                                    tokens.Add(new Operator("*"));
                                }
                            }
                            tokens.Add(new SpecialOperand(op));
                        }
                        else
                        {
                            SpecialOperator testOp = new SpecialOperator(op);

                            if (specialOperators.Contains(testOp.OperatorToken))
                            {
                                tokens.Add(testOp);
                            }
                            else
                            {
                                //Console.WriteLine("special");
                                tokens.Add(new Operator(op));
                            }
                        }
                    }

                    i = endIndex;
                }
                else if (c == specialOperatorContainerSymbol) // if c is "["
                {
                    //tokens.Add(new SpecialOperator(c));
                    string joinedInfix = string.Join("", _infix);
                    int endIndex = joinedInfix.IndexOf(specialOperatorContainerEndSymbol, i + 1);
                    int bracketBuffer = 0;

                    string op = joinedInfix.Substring(i + 1, endIndex - i - 1);
                    //Console.WriteLine(op);

                    while (Regex.Matches(Regex.Escape(op), Regex.Escape(specialOperatorContainerSymbol)).Count > bracketBuffer)
                    {
                        bracketBuffer++;
                        endIndex = joinedInfix.IndexOf(specialOperatorContainerEndSymbol, endIndex + 1);
                        op = joinedInfix.Substring(i + 1, endIndex - i - 1);
                    }


                    tokens.Add(new SpecialOperatorContainer(op));

                    //Console.WriteLine("T                 " + op);
                    //if (specialOperands.Contains(op)) //is a special operand
                    //{
                    //    //operandHolder.Push(op);
                    //    //Console.WriteLine(tokens.Count - 2);
                    //    if (tokens.Count > 0)
                    //    {
                    //        IToken previousToken = tokens[tokens.Count - 1];

                    //        //Console.WriteLine("Prev:                " + previousToken.Token);
                    //        if (previousToken is Operand || previousToken is SpecialOperand)
                    //        {
                    //            tokens.Add(new Operator("*"));
                    //        }
                    //    }
                    //    tokens.Add(new SpecialOperand(op));
                    //}
                    //else
                    //{
                    //    SpecialOperator testOp = new SpecialOperator(op);

                    //    if (specialOperators.Contains(testOp.OperatorComponent))
                    //    {
                    //        tokens.Add(testOp);
                    //    }
                    //    else
                    //    {
                    //        //Console.WriteLine("special");
                    //        tokens.Add(new Operator(op));
                    //    }
                    //}

                    i = endIndex;
                }
                else if (c == specialOperandSingleSymbol)
                {
                    if (operandHolder.Count > 0)
                    {
                        List<string> operandStringHolder = new List<string>();
                        while (operandHolder.Count > 0)
                        {
                            operandStringHolder.Add(operandHolder.Pop());
                        }
                        operandStringHolder.Reverse();
                        tokens.Add(new Operand(operandStringHolder));
                        //Console.WriteLine("fffff: " + c);
                    }

                    //string joindInfix = string.Join("", _infix);

                    //int endIndex = joindInfix.IndexOf(specialOperatorSymbol, i + 1);

                    string op = _infix[++i];//joindInfix.Substring(i + 1, endIndex - i - 1);

                    if (specialOperandSingles.Contains(op)) //is a special operand
                    {
                        
                        //operandHolder.Push(op);
                        //Console.WriteLine(tokens.Count - 2);
                        if (tokens.Count > 0)
                        {
                            IToken previousToken = tokens[tokens.Count - 1];

                            //Console.WriteLine("Prev:                " + previousToken.Token);
                            if (previousToken is Operand || previousToken is SpecialOperand)
                            {
                                tokens.Add(new Operator("*"));
                            }
                        }
                        tokens.Add(new SpecialOperand(SpecialOperand.SingleToToken[op]));
                    }
                }
                else if (c == specialOperatorVariableSymbol)
                {
                    if (operandHolder.Count > 0)
                    {
                        List<string> operandStringHolder = new List<string>();
                        while (operandHolder.Count > 0)
                        {
                            operandStringHolder.Add(operandHolder.Pop());
                        }
                        operandStringHolder.Reverse();
                        tokens.Add(new Operand(operandStringHolder));
                        //Console.WriteLine("fffff: " + c);
                    }

                    string joindInfix = string.Join("", _infix);
                    int endIndex = joindInfix.IndexOf(specialOperatorVariableEndSymbol, i + 1);

                    //Console.WriteLine("Heyyyyyyyyyyyyy      " + joindInfix);//.Substring(i + 1, endIndex - i - 1));
                    //Console.WriteLine("EndIndex                " + Convert.ToString(endIndex - i - 1));
                    double num = Convert.ToDouble(joindInfix.Substring(i + 1, endIndex - i - 1));

                    if (tokens.Count > 0)
                    {
                        IToken previousToken = tokens[tokens.Count - 1];

                        //Console.WriteLine("Prev:                " + previousToken.Token);
                        if (previousToken is Operand || previousToken is SpecialOperand)
                        {
                            tokens.Add(new Operator("*"));
                        }
                    }
                    tokens.Add(new SpecialOperand(num)); //so that you can multiply without having to put the * symbol

                    i = endIndex;
                }
                else if (!operandTokens.Contains(c)) //if not part of a number
                {
                    if (operandHolder.Count > 0)
                    {
                        List<string> operandStringHolder = new List<string>();
                        while (operandHolder.Count > 0)
                        {
                            operandStringHolder.Add(operandHolder.Pop());
                        }
                        operandStringHolder.Reverse();
                        tokens.Add(new Operand(operandStringHolder));
                        //Console.WriteLine("fffff: " + c);
                    }

                    //if (c == specialOperatorSymbol)
                    //{
                    //    string joindInfix = string.Join("", _infix);

                    //    int endIndex = joindInfix.IndexOf(specialOperatorSymbol, i + 1);

                    //    string op = joindInfix.Substring(i + 1, endIndex - i - 1);
                    //    SpecialOperator testOp = new SpecialOperator(op);

                    //    if (specialOperators.Contains(testOp.OperatorComponent))
                    //    {
                    //        tokens.Add(testOp);
                    //    }
                    //    else
                    //    {
                    //        tokens.Add(new Operator(op));
                    //    }

                    //    i = endIndex;
                    //}
                    //else
                    //{
                    if (c == "(" && tokens.Count > 0)
                    {
                        IToken previousToken = tokens[tokens.Count - 1];

                        //Console.WriteLine("Prev:                " + previousToken.Token);
                        if (previousToken is Operand || previousToken is SpecialOperand)
                        {
                            tokens.Add(new Operator("*"));
                        }
                    }

                    tokens.Add(new Operator(c));

                    //if (i!=_infix.Count - 1)
                    //    Console.WriteLine("bazinga" + i +"    " + c + "   " + _infix[i+1]);
                    if (c == "(" && _infix[i + 1] == "-")
                    {
                        //Console.WriteLine("bruh" + i);
                        //tokens.Add(new Operator("negative"));
                        tokens.Add(new Operator(TokenLibrary.tokenLibrary["negative"]));
                        i++;
                    }
                    //}
                    //Console.WriteLine("bbbbb: " + c);
                }
                else //if c is an operand
                {
                    operandHolder.Push(c);

                    if (operandHolder.Count > 0 && tokens.Count > 0)
                    {
                        IToken previousToken = tokens[tokens.Count - 1];
                        //\E,0,2,0,2\[2^\E,0,2\[&i&j&k]]
                        if (previousToken is SpecialOperand)
                        {
                            tokens.Add(new Operator("*"));
                        }
                    }

                    //operandHolder.Push(c);

                    if (c == "E")
                    {
                        string next = _infix[i + 1];
                        if (next == "+" || next == "-")
                        {
                            operandHolder.Push(next);
                            i++; 
                        }
                    }
                }
            }

            //if (operatorHolder.Count > 0)
            //{
            //    List<string> operatorStringHolder = new List<string>();
            //    while (operatorHolder.Count > 0)
            //    {
            //        operatorStringHolder.Add(operatorHolder.Pop());
            //    }
            //    operatorStringHolder.Reverse();
            //    tokens.Add(new Operator(operatorStringHolder));
            //}
            if (operandHolder.Count > 0)
            {
                List<string> operandStringHolder = new List<string>();
                while (operandHolder.Count > 0)
                {
                    operandStringHolder.Add(operandHolder.Pop());
                }
                operandStringHolder.Reverse();
                tokens.Add(new Operand(operandStringHolder));
            }

            return tokens;
        }

        public Postfix ToPostfix()
        {
            //Console.WriteLine("RawInfix                 " + RawInfix);
            //foreach (IToken token in InfixTokens)
            //{
            //    Console.WriteLine("Infix              " + token.Token);
            //}

            Stack<IOperator> operatorStack = new Stack<IOperator>();
            //operatorStack.Push(new Operator("("));
            //InfixTokens.Insert(0, new Operator("("));
            //InfixTokens.Add(new Operator(")"));

            List<IToken> postfixExpression = new List<IToken>();

            foreach (IToken c in InfixTokens)
            {
                //if (c is SpecialOperatorContainer)
                //{
                //    Console.WriteLine("yyyyyy");
                //}
                if (c.Token == "")
                {

                }
                else if (c is Operand || c is SpecialOperand || c is SpecialOperatorContainer)
                {
                    postfixExpression.Add(c);
                }
                else if (c.Token == "(")
                {
                    operatorStack.Push((Operator)c);
                }
                else if (c.Token == ")")
                {
                    IToken x = operatorStack.Pop();
                    while (x.Token != "(")
                    {
                        postfixExpression.Add(x);
                        x = operatorStack.Pop();
                    }
                }
                else if (c is SpecialOperator)
                {
                    //Console.WriteLine("bbbbb");
                    IOperator x = operatorStack.Pop();
                    SpecialOperator temp = (SpecialOperator)c;

                    //string op = 

                    //if (x is SpecialOperator)
                    //{

                    //}

                    while (isOperator(x.OperatorToken) && precedence(x.OperatorToken) >= precedence(((SpecialOperator)c).OperatorToken))
                    {
                        postfixExpression.Add(x);
                        x = operatorStack.Pop();
                    }
                    operatorStack.Push(x);

                    operatorStack.Push((SpecialOperator)c);
                }
                else if (c is Operator)
                {
                    IOperator x = operatorStack.Pop();
                    //Operator temp = (Operator)c;
                    while (isOperator(x.OperatorToken) && precedence(x.OperatorToken) >= precedence(((Operator)c).OperatorToken))
                    {
                        postfixExpression.Add(x);
                        x = operatorStack.Pop();
                    }
                    operatorStack.Push(x);

                    operatorStack.Push((Operator)c);
                }
            }

            return new Postfix(postfixExpression);
        }



        private bool isOperator(string c)
        {
            return (!operandTokens.Contains(c) || specialOperators.Contains(c)) && c != "(" && c != ")";
        }

        private int precedence(string op)
        {
            //switch (op)
            //{
            //    case "negative": 
            //        return 10;
            //    case "sin": case "cos": case "tan":
            //    case "sind": case "cosd": case "tand":
            //    case "sqrt":
            //        return 9;
            //    case "^":
            //        return 3;
            //    case "*": case "/":
            //        return 2;
            //    case "+": case "-":
            //        return 1;
            //    default:
            //        return 0;
            //}
            if (op == TokenLibrary.tokenLibrary["negative"])
                return 10;
            else if (op == "!")
                return 12;
            else if (op == TokenLibrary.tokenLibrary["summation"] || op == TokenLibrary.tokenLibrary["integral"])
                return 11;
            else if (op == TokenLibrary.tokenLibrary["sine"] || op == TokenLibrary.tokenLibrary["cosine"] || op == TokenLibrary.tokenLibrary["tangent"]
                    || op == TokenLibrary.tokenLibrary["sine degrees"] || op == TokenLibrary.tokenLibrary["cosine degrees"] || op == TokenLibrary.tokenLibrary["tangent degrees"]
                    || op == TokenLibrary.tokenLibrary["square root"])
                return 8;
            else if (op == "^")
                return 3;
            else if (op == "*" || op == "/")
                return 2;
            else if (op == "+" || op == "-")
                return 1;
            else
                return 0; 
        }

    }
}
