using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    class SpecialOperator : IOperator
    {
        public string Token { get; set; }
        public List<string> TokenComponents
        {
            get
            {
                //return Token
                //    .Split(new char[]{','})
                //    .ToList();
                return SplitComponents(Token);
            }
        }
        //public string OperatorComponent
        //{
        //    get { return TokenComponents[0]; }
        //}

        public string OperatorToken
        {
            get { return TokenComponents[0]; }
        }

        public SpecialOperator(string op)
        {
            Token = op;
        }

        public SpecialOperator(List<string> operatorElements)
        {
            Token = string.Join("", operatorElements);
        }

        //\E,0,(\I,0,#p\[\sin\&i]) \     [&i^3]
        //{E,0,({I,0,#p}[{sin}&i]) }     [&i^3]

        List<string> SplitComponents (string token)
        {
            List<string> rawSplit = token.Split(new char[] { ',' }).ToList();
            //Console.WriteLine(rawSplit.Count);
            List<string> returnList = new List<string>();
            int closeBuffer = 0;
            for (int i = 0; i < rawSplit.Count; i++)
            {
                string currentString = rawSplit[i];
                if (currentString.Contains("(")) //&& i != rawSplit.Count - 1)
                {
                    string parsedString = "";// = new List<string>();
                    closeBuffer++; //1
                    for (int j = i; j < rawSplit.Count; j++)
                    {
                        string parsingString = rawSplit[j];
                        parsedString += parsingString;//.Add(parsingString);
                        if (parsingString.Contains("("))
                        {
                            closeBuffer += parsingString.Count(c => c == '(');
                        }
                        if (parsingString.Contains(")"))
                        {
                            closeBuffer += parsingString.Count(c => c == ')');
                        }
                        if (closeBuffer == 0)
                        {
                            i = j + 1;
                            break;
                        }
                    }
                    //{E,0,({I,0,#p}[{sin}&i])}[&i]
                    //returnList.Add(parsedString);
                    returnList.Add((new Infix(parsedString).ToPostfix().SolvedExpression()).ToString());
                }
                else
                    returnList.Add(rawSplit[i]);
            }

            return returnList;

        }


    }
}
