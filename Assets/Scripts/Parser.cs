using System.Collections.Generic;

public class Parser
{
    Dictionary<string , int > operatorPrecedence = new Dictionary<string , int>()  
    { //diccionario para guardar los operador con sus respectivas precedencias 
        {"+" , 1},
        {"-" , 1},
        {"*" , 2},
        {"/" , 2},
        {"%" , 2},
        {"**", 3},
        {"&&", 1},
        {"||", 0},
    };
    
    private int currentIndex; //guardar el indice del token actual que se esta analizando 
    private List<Token> tokens = new List<Token>(); //lista de tokens proveniente de un analisis lexico 
    public List<ASTNode> aSTNodes = new List<ASTNode>(); //lista de nodos a Evaluar posteriormente 
    public Token currentToken;
    public Parser(List<Token> tokens) //constructor de la clase 
    {
        currentIndex = 0; //inicializar el indice en cero como es logico se comienza por la primera posicion 
        this.tokens = tokens; //inicializar la lista de tokens con la que se le pasa al constructor 
        currentToken = tokens[currentIndex];
    }
    public void Parse()
    {
        while(currentIndex < tokens.Count)
        {
            if(tokens[currentIndex].Type == TokenType.LineJump)
            {
                currentIndex += 1;
                continue;
            }
            if(currentIndex + 1 < tokens.Count && tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex+1].Value == "(")
            {
                FunctionNode functionNode = ParseFunction();
                aSTNodes.Add(functionNode);
            }
            else currentIndex += 1; //si no es una funcion ni salto de linea, se avanza
            
        }
    }

    private FunctionNode ParseFunction()
    {
        if(currentIndex >= tokens.Count) return new FunctionNode("",new List<ASTNode>());
        
        string functionName = tokens[currentIndex].Value;
        currentIndex += 2; // Saltar nombre de función y '('
    
        var functionNode = new FunctionNode(functionName, new List<ASTNode>());
    
        while (currentIndex < tokens.Count && tokens[currentIndex].Value != ")")
        {
            if(tokens[currentIndex].Value == ",")
            {
                currentIndex += 1;
                continue;
            }
    
            var param = ParseParams();
            if(param != null)
            {
                functionNode.Params.Add(param);
            }
        }
    
        if(currentIndex < tokens.Count && tokens[currentIndex].Value == ")")
        {
            currentIndex += 1; // Saltar el ')'
        }
    
        return functionNode;
    }
    private ASTNode ParseParams()
    {
        List<object> inFix = new List<object>();
        if(tokens[currentIndex].Value == ",") currentIndex += 1;
        
        while (currentIndex < tokens.Count && 
               tokens[currentIndex].Value != "," && 
               tokens[currentIndex].Value != ")" && 
               tokens[currentIndex].Type != TokenType.LineJump)
        {
            if(tokens[currentIndex].Type == TokenType.Identifier && 
               currentIndex + 1 < tokens.Count && 
               tokens[currentIndex + 1].Value == "(")
            {
                // Parsear la función anidada
                inFix.Add(ParseFunction());
                // No incrementar currentIndex aquí porque ParseFunction ya lo hace
            }
            else
            {
                inFix.Add(tokens[currentIndex]);
                currentIndex += 1;
            }
        }
        
        List<object> postFix = ConvertPostFix(inFix);
        return ParsePostFix(postFix);
    }
    private List<object> ConvertPostFix(List<object> inFix) //convertir de notacion infija a postfija
    {
        List<object> outPut = new List<object>(); //lista de salida postfija
        List<Token> stackOperators = new List<Token>(); //pila de operadores

        for (int i = 0 ; i < inFix.Count ; i++) //iterar por los elementos infijos 
        {
            Token ?aux = null;
            if(inFix[i] is Token) aux = (Token)inFix[i];
            if(aux is null) 
            {
                FunctionNode a = (FunctionNode)inFix[i];
                outPut.Add(a); //mientras que no sea un operador aritmetico significa que puede ser una variable o una llamada a un metodo 
            }
            else if(aux is not null && aux.Type != TokenType.ArithmeticOperator)
            {
                outPut.Add((Token)aux);
            }
            else if(aux is not null && stackOperators.Count == 0) stackOperators.Add(aux); //si no se tienen operadores en la pila no es necesario ninguna verificacion 
            else //si es un operador y ya se tiene operadores 
            {
                for (int j = stackOperators.Count-1 ; j >= 0 ; j--) //verificar la pila de operadores 
                {
                    //si se tiene alguno con mahyor e igual precedencia que el que se tiene se desapila 
                    if(aux is not null && operatorPrecedence[stackOperators[j].Value.ToString()] >= operatorPrecedence[aux.Value.ToString()])
                    {
                        outPut.Add(stackOperators[j]); //agregar a la salida postfija 
                        stackOperators.Remove(stackOperators[j]); //eliminar el operador agregado 
                    }
                }
                if(aux is not null)stackOperators.Add(aux); //una vez se desapilaron los que se tienen que desapilar, agregar el que se tenia a la pila de operadores 
            }
        }
        //una vez se tiene una notacion postfija agregar los operadores restantes de la pila 
        for (int i = stackOperators.Count - 1; i >= 0 ; i--) //iterar de atras para alante para simular la pila
        {
            outPut.Add(stackOperators[i]); //agregar cada operador a la postfija
        }
        return outPut; //retornar la salida postfija
    }
    private ASTNode ParsePostFix(List<object> postFix) //parsear la notacion postfija 
    {
        List<ASTNode> nodes = new List<ASTNode>(); //lista de nodos que se tendran mientras se vaya parseando 
        for (int i = 0; i < postFix.Count ; i++) //iterar por todos tokens en notacion infija 
        {
            Token ?aux = null;
            if(postFix[i] is Token) aux = (Token)postFix[i];
            if(aux is not null && aux.Type == TokenType.Number) nodes.Add(new NumberLiteralNode(int.Parse(aux.Value)));//si el tipo de token actual es un numero agregar a la lista de nodos un nodo literal numerico 
            else if(aux is not null && aux.Type == TokenType.String) nodes.Add(new StringLiteralNode(aux.Value));//si el tipo de token actual es un string agregar a la lista de nodos un nodo literal de string  
            else if(aux is not null && aux.Type == TokenType.Bool) nodes.Add(new BooleanLiteralNode(bool.Parse(aux.Value)));//si el tipo de token actual es un booleano agregar a la lista de nodos un nodo literal booleano
            else if(aux is not null && aux.Type == TokenType.Identifier)
            {
                nodes.Add(new VariableNode(aux.Value, Context.variablesValues[aux.Value])); //manejar nodos de variablessssssssssssssssssssssssssssssssssssss
            }
            else if(aux is null)
            {
                FunctionNode a = (FunctionNode)postFix[i];
                nodes.Add(a);
            }
            //si el tipo de token actual es un operador aritmetico o logico se debe crear un nodo de operacion bineria con el los ultimos dos nodos como hijos derecho e izquierdo   
            else if(aux is not null && (aux.Type == TokenType.ArithmeticOperator || aux.Type == TokenType.LogicOperator))
            {
                BinaryOperationNode binaryOperationNode = new BinaryOperationNode(aux , nodes[nodes.Count-2] , nodes[nodes.Count-1]); //crear nodo con el operador actual y con los respectivos dos ultimos nodos como hijos 
                nodes.RemoveAt(nodes.Count-1); //una vez se creo el nodo elimnar los dos ultimos nodos hojas
                nodes.RemoveAt(nodes.Count-1); //...
                nodes.Add(binaryOperationNode); //agregar a la lista de nodos el nuevo nodo ya creado 
                if(i == postFix.Count-1) return binaryOperationNode;
            }
        }
        ASTNode [] list = nodes.ToArray();
        return list[0];
    }
    // private VariableNode ParseVariable()
    // {
    //     VariableNode variableNode = new VariableNode(tokens[currentIndex].Value,null);
    //     currentIndex += 2; //estoy sobre el primer valor de la asignacion a la variable 
    //     if(tokens[currentIndex + 1].Type == TokenType.ArithmeticOperator)
    //     {
    //         List<Token> Infix = new List<Token>();
    //         while(tokens[currentIndex].Type != TokenType.LineJump)
    //         {
    //             if(tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex + 1].Value == "(")
    //             {
    //                 FunctionNode functionNode = ParseFunction(); //llamar a parsear funcion y guardarlo en el nodo de funcion 
    //                 aSTNodes.Add(functionNode); //agregar el nodo de funcion a la lista de nodos a evaluar 
    //                 if(currentToken.Type == TokenType.LineJump)
    //                 {
    //                     currentIndex += 1;
    //                     continue;
    //                 }
    //             }
    //             Infix.Add(tokens[currentIndex]);
    //             Infix.Add(tokens[currentIndex + 1]);
    //             currentIndex += 1;
    //         }
    //         Infix.Add(tokens[currentIndex]);
    //         List<Token> postFix = ConvertPostFix(Infix);

    //     }
    //     else if (tokens[currentIndex + 1].Value == "(")
    //     {

    //     }
    //     return null;
    // }
}