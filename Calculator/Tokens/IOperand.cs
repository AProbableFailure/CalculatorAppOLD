using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    interface IOperand : IToken
    {
        double TokenValue { get; }
    }
}
