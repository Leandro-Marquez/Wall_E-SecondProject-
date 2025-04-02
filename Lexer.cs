using System.Text.RegularExpressions;

public class Lexer
{
    string input;
    public List<Token> tokens = new List<Token>();
    public Lexer(string input)
    {
        this.input = input;
    }
    public void Tokenize()
    {
        input.Trim(); // eliminar los espacios en blanco antes y despues del ultimo caracter respectivamente 
        
        // definiendo las expresiones regulares 
        string ReservedKeyword = @"GoTo";
        string Bool = @"true|false";
        string ArithmeticOperator = @"(?:[+\--*/%\***])";
        string Identifier = @"^[a-z-A-Z-_][a-z-A-Z-0-9-_]*";
        string Delimiter = @"[\(\)\{\}\[\]]";
        string ComparisonOperator = @"(?:==|>=|<=|>|<)";
        string AssignmentOperator =  @"(<-)";
        string LogicOperator = @"(?:&&|\|\||!)";
        string String = @"""(([^""\\]|\.)*?)""";
        string Number = @"\d+";
        string Comma = @",";

        var Diccionario = new Dictionary<string, TokenType>
        {
            { ReservedKeyword    , TokenType.ReservedKeyword },
            { Bool               , TokenType.Bool },
            { ArithmeticOperator , TokenType.ArithmeticOperator },
            { Identifier         , TokenType.Identifier },
            { Delimiter          , TokenType.Delimiter},
            { ComparisonOperator , TokenType.ComparisonOperator },
            { AssignmentOperator , TokenType.AssignmentOperator },
            { LogicOperator      , TokenType.LogicOperator },
            { String             , TokenType.String },
            { Number             , TokenType.Number },
            { Comma              , TokenType.Comma },
        };

        while(!string.IsNullOrEmpty(input)) // mientras que la entrada no sea nula ni vacia 
        {
            string betterToken = null!; // guardar el mejor token 
            TokenType betterTokenType = TokenType.ReservedKeyword; // guardar el mejor tipo de token 
            int betterIndex = 0; // guardar la mejor longitud de token 
            foreach (var item in Diccionario) // iterar por el diccionario 
            {
                var token = Regex.Match(input , item.Key);
                if (token.Success && token.Index == 0 && token.Length > betterIndex) // verificar si hay algun token para la E.R actual, si el token se encuentra al inicio de la entrada, y si tiene mejor longitud que el mejor hasta el momento 
                {
                    betterToken = token.Value; // guardar el mejor tokn hasta ahora 
                    betterTokenType = item.Value; // asignarle a dicho token su respectivo tipo
                    betterIndex = token.Length; // guardar la longitud del token con el que se hizo match
                }
            }

            if(betterToken is null) // en caso de que sea nulo
            {
                input = input.Substring(1).Trim(); // saltar el primer caracter y limpiar caracteres vacios al inicio 
                continue; // continuar con el ciclo 
            }
            // si llego a este punto significa que el mejor token no es nulo
            tokens.Add(new Token(betterTokenType , betterToken)); // agregar un nuevo token a la lista de tokens 

            input = input.Substring(betterIndex).Trim(); // saltar el token completo y limpiar caracteres vacios para seguir analizando 

            if (betterTokenType == TokenType.ArithmeticOperator && !string.IsNullOrEmpty(input)) 
            {
                var NextToken = Regex.Match(input, ArithmeticOperator);

                if (NextToken.Success && NextToken.Index == 0)
                {
                    tokens[tokens.Count - 1].Value += NextToken.Value;
                    input = input.Substring(NextToken.Length).Trim();
                }
            } 
        }
    }
}