using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;//para regex

public class Lexer
{
    string input;//cadena de entrada a tokenizar
    public List<Token> tokens { get; set; }//lista de tokens resultantes

    public Lexer(string input)//constructor de la clase 
    {
        this.input = input;//asignar la entrada
        tokens = new List<Token>();//inicializar la lista
    }

    public void Tokenize()//método principal de tokenización
    {
        input = input.Trim();//limpiar espacios al inicio/final en primer momento

        //definición de patrones regex para cada tipo de token
        string ReservedKeyword = @"GoTo";
        string Bool = @"true|false";
        string ArithmeticOperator = @"(?:[+\-*/%\*])";
        string Identifier = @"^[a-zA-Z_][a-zA-Z0-9-_]*";
        string Delimiter = @"[\(\)\{\}\[\]]";
        string ComparisonOperator = @"(?:==|!=|>=|<=|>|<)";
        string AssignmentOperator = @"(<-)";
        string LogicOperator = @"(?:&&|\|\||!)";
        string String = @"""((?:[^""\\]|\\.)*)""";
        string Number = @"\d+";
        string Comma = @",";

        //mapeo de patrones a tipos de token
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

        while (!string.IsNullOrEmpty(input))//procesar mientras la entrada no sea nula ni vacia
        {
            // --- LÓGICA PARA  MANEJAR LOS NÚMEROS NEGATIVOS
            bool isNegativeNumber = input.StartsWith("-") && (tokens.Count == 0 || tokens.Last().Type == TokenType.Delimiter || tokens.Last().Type == TokenType.Comma || tokens.Last().Type == TokenType.ArithmeticOperator);

            if (isNegativeNumber) //si se trata de una posibilidad de un numero negativo
            {
                var numberMatch = Regex.Match(input, @"^-(\d+)"); //modificar la expresion regex para numeros negaticos
                if (numberMatch.Success)//si se hace succes es q es un patron de numero negativo
                {
                    tokens.Add(new Token(TokenType.Number, "-" + numberMatch.Groups[1].Value));//crear el token 
                    input = input.Substring(numberMatch.Length).Trim();//saltar lo q consumimos y limpiar valores vacios al inicio
                    continue; // Saltamos a la siguiente iteracion para evitar duplicados
                }
            }
            // ---

            string bestToken = null;//mejor token encontrado
            TokenType bestTokenType = TokenType.ReservedKeyword;//tipo del mejor token encontrado 
            int bestLength = 0;//longitud del mejor match

            foreach (var item in Diccionario)//buscar coincidencias
            {
                var match = Regex.Match(input, item.Key);//buscar coincidencia con el patrón actual
                if (match.Success && match.Index == 0 && match.Length > bestLength)//si encontró al inicio y es más largo que el mejor actual
                {
                    bestToken = match.Value;//actualizar mejor token
                    bestTokenType = item.Value;//actualizar tipo
                    bestLength = match.Length;//actualizar longitud
                }
            }

            if (bestToken == null)//si no encontró token válido
            {
                input = input.Substring(1).Trim();//avanzar 1 caracter
                continue;
            }
            //manejo especial para strings
            if (bestTokenType == TokenType.String) bestToken = Regex.Match(bestToken, @"""((?:[^""\\]|\\.)*)""").Groups[1].Value;//extraer contenido sin comillas
            
            tokens.Add(new Token(bestTokenType, bestToken));//agregar token a la lista
            input = input.Substring(bestLength).Trim();//avanzar en la entrada
            
            //manejo de operador especial de potencia ( ** )
            if (bestTokenType == TokenType.ArithmeticOperator && bestToken == "*" && !string.IsNullOrEmpty(input) && input.StartsWith("*"))
            {
                tokens[tokens.Count - 1].Value = "**";//convertir a operador potencia
                input = input.Substring(1).Trim();//avanzar un caracter
            }
        }

        tokens.Add(new Token(TokenType.LineJump, ""));//agregar salto de línea final
    }
}