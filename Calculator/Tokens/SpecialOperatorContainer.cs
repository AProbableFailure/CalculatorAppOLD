using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    class SpecialOperatorContainer : IOperand
    {
        public string Token { get; set; }
        public double TokenValue { get { return Convert.ToDouble(Token); } }

        public SpecialOperatorContainer(string _token)
        {
            Token = _token;
        }
    }
}
