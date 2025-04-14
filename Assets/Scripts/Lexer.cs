using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Lexer
{
    string input;
    public List<Token> tokens { get; set; }

    public Lexer(string input)
    {
        this.input = input;
        tokens = new List<Token>();
    }

    public void Tokenize()
    {
        input = input.Trim(); // Eliminar espacios al inicio/final

        // Expresiones regulares actualizadas
        string ReservedKeyword = @"GoTo";
        string Bool = @"true|false";
        string ArithmeticOperator = @"(?:[+\-*/%\*])";
        string Identifier = @"^[a-zA-Z_][a-zA-Z0-9_]*";
        string Delimiter = @"[\(\)\{\}\[\]]";
        string ComparisonOperator = @"(?:==|!=|>=|<=|>|<)";
        string AssignmentOperator = @"(<-)";
        string LogicOperator = @"(?:&&|\|\||!)";
        string String = @"""((?:[^""\\]|\\.)*)""";  // Grupo de captura sin comillas
        string Number = @"\d+";
        string Comma = @",";

        var Diccionario = new Dictionary<string, TokenType>
        {
            { ReservedKeyword,    TokenType.ReservedKeyword },
            { Bool,              TokenType.Bool },
            { ArithmeticOperator, TokenType.ArithmeticOperator },
            { Identifier,        TokenType.Identifier },
            { Delimiter,         TokenType.Delimiter },
            { ComparisonOperator,TokenType.ComparisonOperator },
            { AssignmentOperator,TokenType.AssignmentOperator },
            { LogicOperator,     TokenType.LogicOperator },
            { String,           TokenType.String },
            { Number,           TokenType.Number },
            { Comma,            TokenType.Comma },
        };

        while (!string.IsNullOrEmpty(input))
        {
            string bestToken = null;
            TokenType bestTokenType = TokenType.ReservedKeyword;
            int bestLength = 0;

            foreach (var item in Diccionario)
            {
                var match = Regex.Match(input, item.Key);
                if (match.Success && match.Index == 0 && match.Length > bestLength)
                {
                    bestToken = match.Value;
                    bestTokenType = item.Value;
                    bestLength = match.Length;
                }
            }

            if (bestToken == null)
            {
                input = input.Substring(1).Trim();
                continue;
            }

            // Manejo especial para strings (eliminar comillas)
            if (bestTokenType == TokenType.String)
            {
                bestToken = Regex.Match(bestToken, @"""((?:[^""\\]|\\.)*)""").Groups[1].Value;
            }

            tokens.Add(new Token(bestTokenType, bestToken));
            input = input.Substring(bestLength).Trim();

            // Manejo de operador ** (potencia)
            if (bestTokenType == TokenType.ArithmeticOperator && bestToken == "*" && !string.IsNullOrEmpty(input) && input.StartsWith("*"))
            {
                tokens[tokens.Count - 1].Value = "**";
                input = input.Substring(1).Trim();
            }
        }

        tokens.Add(new Token(TokenType.LineJump, ""));
    }
}