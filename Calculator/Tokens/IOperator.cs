using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    interface IOperator : IToken
    {
        string OperatorToken { get; }
    }
}
