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
    public void Parse() //metodo principal que lo parsea todo 
    {
        while(currentIndex < tokens.Count) //mientras haya tokens por analizar se continua parseando
        {
            if(tokens[currentIndex].Type == TokenType.LineJump) //si se tiene un salto de linea continuar parseando lo proximo 
            {
                currentIndex += 1;
                continue;
            }
            //si se trata de una invocacion de funcion 
            if(currentIndex + 1 < tokens.Count && tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex+1].Value == "(")
            {
                FunctionNode functionNode = ParseFunction(); //llamar a parsear funcion y guardar el nodo funcion 
                aSTNodes.Add(functionNode); //agregar a la lista de ASTNodes finales 
            }
            //si se trataa de una asignacion a Variable 
            else if(currentIndex + 1 < tokens.Count && tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex + 1].Value == "<-")
            {
                VariableNode variableNode = ParseVariable();
                aSTNodes.Add(variableNode);
                if(Context.variablesValues.ContainsKey(variableNode.Name))
                {   
                    Context.variablesValues[variableNode.Name] = variableNode.Value;
                }
                else Context.variablesValues.Add(variableNode.Name,variableNode.Value);
                //definir la variable en el contextooooooooooooooooooooooooo
            }
            else currentIndex += 1; //si no es una funcion ni salto de linea, se avanza
            
        }
    }
    private FunctionNode ParseFunction() //metodo principal para parsear la invocacion de funcion 
    {
        if(currentIndex >= tokens.Count) return new FunctionNode("",new List<ASTNode>());//en caso de que se salga de los limites de la lista de tokens(extremo)
        
        string functionName = tokens[currentIndex].Value; //guardar el nombre de la funcion 
        currentIndex += 2; // Saltar nombre de función y '('
    
        var functionNode = new FunctionNode(functionName, new List<ASTNode>());//inicializar el nodo de funcion
    
        while (currentIndex < tokens.Count && tokens[currentIndex].Value != ")") //mientras se este en rango y no se tenga parentesis cerrado 
        {
            if(tokens[currentIndex].Value == ",") //si es una coma 
            {
                currentIndex += 1; //aumentar el indice y continuar con la proxima iteracion
                continue;
            }
    
            var param = ParseParams(); //parsear parametros hasta el momento
            //si no es nulo, significa que se tiene parametros, agregar y continuar 
            if(param != null) functionNode.Params.Add(param);
        }
    
        if(currentIndex < tokens.Count && tokens[currentIndex].Value == ")") //si se llega al final de la invocacion, saltar el parentesis 
        {
            currentIndex += 1; // Saltar el ')'
        }
        return functionNode; //retornar el nodo funcion parseado 
    }
    private ASTNode ParseParams() //metodo principal para parsear parametros 
    {
        List<object> inFix = new List<object>(); //lista de elementos en notacion infija 
        if(tokens[currentIndex].Value == ",") currentIndex += 1; //si llega encima de una coma, saltarla 
        //si se esta en rango, no es una coma, ni un parentesis cerrado ni un salto de linea 
        while (currentIndex < tokens.Count && tokens[currentIndex].Value != "," && tokens[currentIndex].Value != ")" && tokens[currentIndex].Type != TokenType.LineJump)
        {
            //si trata de una invocacion de funcion, parsear la funcion anidada
            if(tokens[currentIndex].Type == TokenType.Identifier && currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Value == "(")
            {
                inFix.Add(ParseFunction());
                //no se incrementa currentIndex aquí porque ParseFunction ya lo hace
            }
            else
            {
                inFix.Add(tokens[currentIndex]); //de lo contrario agregar a lista de elementos en notacion infija 
                currentIndex += 1; //aumentar el indice para continuar con el proximo elemento 
            }
        }
        
        List<object> postFix = ConvertPostFix(inFix); //convertir a notacion postfija todo lo que se tenia en notacion infija 
        return ParsePostFix(postFix); //retornar el nodo que se obtiene a partir de parsear la notacion postfija
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
    private VariableNode ParseVariable()
    {
        if(currentIndex >= tokens.Count) return new VariableNode("",null);//en caso de que se salga de los limites de la lista de tokens(extremo)

        string variableName = tokens[currentIndex].Value; //guardar el nombre de la funcion 
        currentIndex += 2; // Saltar nombre de la variable y '<-'
        var variableNode = new VariableNode(variableName,null);

        if(currentIndex + 1 < tokens.Count )
        {
            List<object> Infix = new List<object>();
            /*tokens[currentIndex].Type != TokenType.LineJump*/
            while(currentIndex + 1 < tokens.Count && (tokens[currentIndex].Type == TokenType.ArithmeticOperator || tokens[currentIndex].Type == TokenType.LogicOperator || tokens[currentIndex].Type == TokenType.ComparisonOperator
            || tokens[currentIndex + 1].Type == TokenType.ArithmeticOperator || tokens[currentIndex + 1].Type == TokenType.LogicOperator || tokens[currentIndex + 1].Type == TokenType.ComparisonOperator))
            {
                if(tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex + 1].Value == "(")
                {
                    FunctionNode functionNode = ParseFunction(); //llamar a parsear funcion y guardarlo en el nodo de funcion 
                    Infix.Add(functionNode);
                }
                else
                {
                    Infix.Add(tokens[currentIndex]);
                    currentIndex += 1;
                }
            }
            if(tokens[currentIndex].Type == TokenType.Identifier && tokens[currentIndex + 1].Value == "(")
            {
                FunctionNode functionNode = ParseFunction(); //llamar a parsear funcion y guardarlo en el nodo de funcion 
                Infix.Add(functionNode);
                List<object> post = ConvertPostFix(Infix);
                variableNode.Value = ParsePostFix(post);
                return variableNode;
            }
            Infix.Add(tokens[currentIndex]);
            List<object> postFix = ConvertPostFix(Infix);
            variableNode.Value = ParsePostFix(postFix);
            return variableNode;

        }
        return null;
    }
}