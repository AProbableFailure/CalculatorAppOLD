using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    class Operator : IOperator
    {
        public string Token { get; set; }
        public string OperatorToken
        {
            get { return Token; }
        }

        public Operator(string op)
        {
            Token = op;
        }

        public Operator(List<string> operatorElements)
        {
            Token = string.Join("", operatorElements);
        }
    }
}
