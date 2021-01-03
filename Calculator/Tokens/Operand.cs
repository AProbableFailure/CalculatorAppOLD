using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    class Operand : IOperand
    {
        public string Token { get; set; }
        //public bool Negative { get; set; }
        public double TokenValue
        {
            get { return Convert.ToDouble(Token); } //* (Convert.ToDecimal(!Negative) * 2 - 1); }
        }

        public Operand(double _operand)//, bool _negative = false)
        {
            Token = _operand.ToString();
            //Negative = _negative;
        }

        public Operand(List<string> _operandElements)//, bool _negative = false)
        {
            Token = string.Join("", _operandElements);
            //Negative = _negative;
        }
    }
}
