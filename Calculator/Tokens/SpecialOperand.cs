using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    class SpecialOperand : IOperand
    {
        public string Token { get; set; }
        public double TokenValue
        {
            get { return TokenToValue(Token); } 
        }
        private bool isNumber = false;

        public SpecialOperand(double _operand)
        {
            Token = Convert.ToString(_operand);
            isNumber = true;
        }

        public SpecialOperand(string _operand)
        {
            Token = _operand;
        }

        public SpecialOperand(List<string> _operandElements)
        {
            Token = string.Join("", _operandElements);
        }

        private double TokenToValue(string token)
        {

            //switch (token.ToLower())
            //{
            //    case TokenLibrary.tokenLibrary["operand pi"]:
            //        return (decimal)Math.PI;
            //    case "e":
            //        return (decimal)Math.E;
            //    default:
            //        return 0;

            //    //case "pi":
            //    //    return (decimal)Math.PI;
            //    //case "e":
            //    //    return (decimal)Math.E;
            //    //default:
            //    //    return 0;
            //}
            if (isNumber)
                return Convert.ToDouble(token);
            else
            {
                if (token == TokenLibrary.tokenLibrary["operand pi"])
                    return Math.PI;
                else if (token == TokenLibrary.tokenLibrary["operand e"])
                    return Math.E;
                else if (token == TokenLibrary.tokenLibrary["operand golden ratio"])
                    return (1 + Math.Sqrt(5)) / 2;
                else if (token == TokenLibrary.tokenLibrary["operand square root 2"])
                    return Math.Sqrt(2);
                else if (token == TokenLibrary.tokenLibrary["operand square root 3"])
                    return Math.Sqrt(3);
                else
                    return 0;
            }
        }

        public static Dictionary<string, string> SingleToToken = new Dictionary<string, string>()
        {
            { TokenLibrary.tokenLibrary["single operand pi"], TokenLibrary.tokenLibrary["operand pi"] },
            { TokenLibrary.tokenLibrary["single operand e"], TokenLibrary.tokenLibrary["operand e"] },
            { TokenLibrary.tokenLibrary["single operand golden ratio"], TokenLibrary.tokenLibrary["operand golden ratio"] },
            { TokenLibrary.tokenLibrary["single operand square root 2"], TokenLibrary.tokenLibrary["operand square root 2"] },
            { TokenLibrary.tokenLibrary["single operand square root 3"], TokenLibrary.tokenLibrary["operand square root 3"] },
            //{ "p", "pi" },
            //{ "e", "e" }
        };
    }
}
