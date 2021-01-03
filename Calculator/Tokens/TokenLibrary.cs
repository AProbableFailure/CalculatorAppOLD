using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tokens
{
    public static class TokenLibrary
    {
        public static Dictionary<string, string> tokenLibrary = new Dictionary<string, string>()
        {
            //{ "^", "^" },
            //{ "*", "*" },
            //{ "/", "/" },
            //{ "+", "+" },
            //{ "-", "-" },

            { "negative", "neg" },

            { "sine", "sin" },
                { "sine degrees", "sind" },
            { "cosine", "cos" },
                { "cosine degrees", "cosd" },
            { "tangent", "tan" },
                { "tangent degrees", "tand" },

            { "square root", "sqrt" },

            { "summation", "E" },
            { "integral", "I" },



            { "operand pi", "pi" },
                { "single operand pi", "p" },
            { "operand e", "e" },
                { "single operand e", "e" },
            { "operand golden ratio", "gr" },
                { "single operand golden ratio", "g" },

            { "operand square root 2", "s2" },
                { "single operand square root 2", "2" },
            { "operand square root 3", "s3" },
                { "single operand square root 3", "3" }
        };
        //public static string tokenLibrary(string raw)
        //{
        //    switch (raw)
        //    {
        //        case "negative": return "neg";

        //        case "sine": return "sin";
        //            case "sine degrees": return "sind";
        //        case "cosine": return "cos";
        //            case "cosine degrees": return "cosd";
        //        case "tangent": return "tan";
        //            case "tangent degrees": return "tan";

        //        case "square root": return "sqrt";

        //        case "operand pi": return "pi";
        //            case "singe operand pi": return "p";
        //        case "operand e": return "e";
        //            case "singe operand e": return "e";
        //        default: return "";
        //    }
        //}
    }
}
