using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

public class Parser
{
    Dictionary<string , int > operatorPrecedence = new Dictionary<string , int>()  
    { //diccionario para guardar los operador con sus respectivas precedencias 
        {"+" , 2},
        {"-" , 2},
        {"*" , 3},
        {"/" , 3},
        {"%" , 3},
        {"**", 4},
        {"&&", 1},
        {"||", 0},
        {"==", 0},
        {">=", 0},
        {"<=", 0},
        {"<" , 0},
        {">" , 0},
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
        while(currentIndex < tokens.Count && Error.errors.Count == 0) //mientras haya tokens por analizar se continua parseando
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
                Context.variableNodes.Add(variableNode);
            } 
            //si se trata de una etiqueta 
            else if(tokens[currentIndex].Type == TokenType.Identifier && ((currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Type != TokenType.ComparisonOperator && tokens[currentIndex + 1].Type != TokenType.ArithmeticOperator && tokens[currentIndex + 1].Type != TokenType.LogicOperator) || tokens[currentIndex+1].Type == TokenType.LineJump))
            {
                aSTNodes.Add(new LabelNode(tokens[currentIndex].Value));
                Context.labels.Add(tokens[currentIndex].Value,aSTNodes.Count-1);
                currentIndex += 1;
            }
            //si se trata de un GoTo
            else if(tokens[currentIndex].Type == TokenType.ReservedKeyword && currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Value == "[")
            {
                aSTNodes.Add(ParseGoTo());
                currentIndex += 1;
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
            //si se trata de una variable 
            else if(tokens[currentIndex].Type == TokenType.Identifier) //manejar erroresssssssssssssssssssssssssssssss
            {
                if(Context.variableNodes.Count == 0) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                else
                {
                    int auxCounter = 0;
                    for (int i = Context.variableNodes.Count - 1 ; i >= 0 ; i--)
                    {
                        if(Context.variableNodes[i].Name == tokens[currentIndex].Value)
                        {
                            inFix.Add(Context.variableNodes[i]);
                            currentIndex += 1;
                            break;
                        }
                        else auxCounter += 1;
                    }
                    if(auxCounter == Context.variableNodes.Count) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                }
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
                var a = inFix[i];
                outPut.Add(a); //mientras que no sea un operador aritmetico significa que puede ser una variable o una llamada a un metodo 
                // else if()
            }
            else if(aux is not null && aux.Type != TokenType.ArithmeticOperator && aux.Type != TokenType.LogicOperator && aux.Type != TokenType.ComparisonOperator)
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
            // else if(aux is not null && aux.Type == TokenType.Identifier) nodes.Add(new VariableNode(aux.Value, (ASTNode)Context.variablesValues[aux.Value])); //manejar nodos de variablessssssssssssssssssssssssssssssssssssss
            else if(aux is null) //si es una variable o un metodoooooo
            {
                // var a = postFix[i];
                // FunctionNode a = (FunctionNode)postFix[i];
                nodes.Add((ASTNode)postFix[i]);
            } 
            //si el tipo de token actual es un operador aritmetico o logico se debe crear un nodo de operacion bineria con el los ultimos dos nodos como hijos derecho e izquierdo   
            else if(aux is not null && (aux.Type == TokenType.ArithmeticOperator || aux.Type == TokenType.LogicOperator || aux.Type == TokenType.ComparisonOperator))
            {
                BinaryOperationNode binaryOperationNode = new BinaryOperationNode(aux , nodes[nodes.Count-2] , nodes[nodes.Count-1]); //crear nodo con el operador actual y con los respectivos dos ultimos nodos como hijos 
                nodes.RemoveAt(nodes.Count-1); //una vez se creo el nodo elimnar los dos ultimos nodos hojas
                nodes.RemoveAt(nodes.Count-1); //...
                nodes.Add(binaryOperationNode); //agregar a la lista de nodos el nuevo nodo ya creado 
                if(i == postFix.Count-1) return binaryOperationNode;
            }
        }
        ASTNode [] list = nodes.ToArray();//convertir la lista de nodos a array para poder indexar correctamente sin tener problemas 
        if(list.Length > 0)return list[0]; //retornar el primer nodo y unico
        else return null;
    }
    private VariableNode ParseVariable() //metodo prinicpla para parsear asignaciones de variable 
    {
        if (currentIndex >= tokens.Count) return new VariableNode("", null); //si no se esta en rangos salir, se llego aqui por error(extremo)

        string variableName = tokens[currentIndex].Value; //guardar el nombre correctamente para asignarselo al nodo variable correctamete 
        currentIndex += 2; // Saltar nombre y '<-'
        var variableNode = new VariableNode(variableName, null); //crear el nodo variable con nombre ya 

        List<object> infix = new List<object>();//lista de elementos en notacion infija 

        while (currentIndex < tokens.Count && tokens[currentIndex].Type != TokenType.LineJump) //mientras se tengan tokens por consumir y no sea unn salto de linea continuar 
        {
            //si se trata de una de una funcion 
            if (tokens[currentIndex].Type == TokenType.Identifier && currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Value == "(" && tokens[currentIndex-1].Type == TokenType.ArithmeticOperator ||  tokens[currentIndex-1].Type == TokenType.LogicOperator ||  tokens[currentIndex-1].Type == TokenType.ComparisonOperator)
            {
                infix.Add(ParseFunction()); //agregar el nodo funcion paraseado correctamente 
            }
            //en cualquier otro caso agregar a la lista en notacion infija 
            else if (tokens[currentIndex].Type == TokenType.String ||tokens[currentIndex].Type == TokenType.Bool ||tokens[currentIndex].Type == TokenType.Number || tokens[currentIndex].Type == TokenType.ArithmeticOperator ||  tokens[currentIndex].Type == TokenType.LogicOperator ||  tokens[currentIndex].Type == TokenType.ComparisonOperator) // <- Incluye Arithmetic, Logic, Comparison
            {
                infix.Add(tokens[currentIndex]); //agregar a la lista en notacion infija 
                currentIndex++; //aumentar el indice
            } 
            //variables
            else if (tokens[currentIndex].Type == TokenType.Identifier && (currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Type == TokenType.ArithmeticOperator|| tokens[currentIndex + 1].Type == TokenType.LogicOperator || tokens[currentIndex + 1].Type == TokenType.ComparisonOperator) )
            {
                // if(Context.variablesValues.ContainsKey(tokens[currentIndex].Value)) infix.Add(Context.variablesValues[tokens[currentIndex].Value]);
                // else Error.errors.Add((ErrorType.Semantic_Error , $"Current variable [ {tokens[currentIndex].Value} ] does not existe in the current context "));
                if(Context.variableNodes.Count == 0) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                else
                {
                    int auxCounter = 0;
                    for (int i = Context.variableNodes.Count - 1 ; i >= 0 ; i--)
                    {
                        if(Context.variableNodes[i].Name == tokens[currentIndex].Value)
                        {
                            infix.Add(Context.variableNodes[i]);
                            currentIndex += 1;
                            break;
                        }
                        else auxCounter += 1;
                    }
                    if(auxCounter == Context.variableNodes.Count) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                } 
                // currentIndex ++;
            }
            //variables
            else if(tokens[currentIndex].Type == TokenType.Identifier && currentIndex - 1 >= 0 && tokens[currentIndex-1].Type == TokenType.ArithmeticOperator|| tokens[currentIndex + 1].Type == TokenType.LogicOperator || tokens[currentIndex + 1].Type == TokenType.ComparisonOperator)
            {
                // if(Context.variablesValues.ContainsKey(tokens[currentIndex].Value)) infix.Add(Context.variablesValues[tokens[currentIndex].Value]);
                // else Error.errors.Add((ErrorType.Semantic_Error , $"Current variable [ {tokens[currentIndex].Value} ] does not existe in the current context "));
                if(Context.variableNodes.Count == 0) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                else
                {
                    int auxCounter = 0;
                    for (int i = Context.variableNodes.Count - 1 ; i >= 0 ; i--)
                    {
                        if(Context.variableNodes[i].Name == tokens[currentIndex].Value)
                        {
                            infix.Add(Context.variableNodes[i]);
                            currentIndex += 1;
                            break;
                        }
                        else auxCounter += 1;
                    }
                    if(auxCounter == Context.variableNodes.Count) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                } 
                // currentIndex ++;
            }
            else break; // Si no es parte de la expresión, salir
        }
        if (infix.Count > 0) //si se tiene al menos un elemento en notacion infija 
        {
            List<object> postFix = ConvertPostFix(infix); //guardar los elementos convertidos a notacion postfija
            variableNode.Value = ParsePostFix(postFix); //asignarle el valor del resultado de parsear la notacion postfija al nodo de variable  
        }
        return variableNode; //retornar correctamente el nodo de variable  
    }

    private GoToNode ParseGoTo()
    {
        var goToNode = new GoToNode(new LabelNode(tokens[currentIndex + 2].Value ),null);
        currentIndex += 5; //se posiciona sobre el primer item de la condicion 

        List<object> infix = new List<object>();//lista de elementos en notacion infija 

        while (currentIndex < tokens.Count && tokens[currentIndex].Type != TokenType.LineJump && tokens[currentIndex].Type != TokenType.ReservedKeyword && tokens[currentIndex].Value != ")") //mientras se tengan tokens por consumir y no sea unn salto de linea continuar 
        {
            //si se trata de una de una funcion 
            if (tokens[currentIndex].Type == TokenType.Identifier && currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Value == "(")
            {
                infix.Add(ParseFunction()); //agregar el nodo funcion paraseado correctamente 
            }
            //variables
            else if (tokens[currentIndex].Type == TokenType.Identifier && (currentIndex + 1 < tokens.Count && tokens[currentIndex + 1].Type == TokenType.ArithmeticOperator|| tokens[currentIndex + 1].Type == TokenType.LogicOperator || tokens[currentIndex + 1].Type == TokenType.ComparisonOperator) )
            {
                // if(Context.variablesValues.ContainsKey(tokens[currentIndex].Value)) infix.Add(Context.variablesValues[tokens[currentIndex].Value]);
                // else Error.errors.Add((ErrorType.Semantic_Error , $"Current variable [ {tokens[currentIndex].Value} ] does not existe in the current context "));
                if(Context.variableNodes.Count == 0) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                else
                {
                    int auxCounter = 0;
                    for (int i = Context.variableNodes.Count - 1 ; i >= 0 ; i--)
                    {
                        if(Context.variableNodes[i].Name == tokens[currentIndex].Value)
                        {
                            infix.Add(Context.variableNodes[i]);
                            currentIndex += 1;
                            break;
                        }
                        else auxCounter += 1;
                    }
                    if(auxCounter == Context.variableNodes.Count) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                } 
                // currentIndex ++;
            }
            //variables
            else if(tokens[currentIndex].Type == TokenType.Identifier && currentIndex - 1 >= 0 && tokens[currentIndex-1].Type == TokenType.ArithmeticOperator|| tokens[currentIndex + 1].Type == TokenType.LogicOperator || tokens[currentIndex + 1].Type == TokenType.ComparisonOperator)
            {
                // if(Context.variablesValues.ContainsKey(tokens[currentIndex].Value)) infix.Add(Context.variablesValues[tokens[currentIndex].Value]);
                // else Error.errors.Add((ErrorType.Semantic_Error , $"Current variable [ {tokens[currentIndex].Value} ] does not existe in the current context "));
                if(Context.variableNodes.Count == 0) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                else
                {
                    int auxCounter = 0;
                    for (int i = Context.variableNodes.Count - 1 ; i >= 0 ; i--)
                    {
                        if(Context.variableNodes[i].Name == tokens[currentIndex].Value)
                        {
                            infix.Add(Context.variableNodes[i]);
                            currentIndex += 1;
                            break;
                        }
                        else auxCounter += 1;
                    }
                    if(auxCounter == Context.variableNodes.Count) Error.errors.Add((ErrorType.Semantic_Error,$"The name {tokens[currentIndex].Value} does not exist in the current context"));
                } 
                // currentIndex ++;
            }
            else //en cualquier otro caso agregar a la lista en notacion infija 
            {
                infix.Add(tokens[currentIndex]); //agregar a la lista en notacion infija 
                currentIndex++; //aumentar el indice
            }
        }
        if (infix.Count > 0) //si se tiene al menos un elemento en notacion infija 
        {
            List<object> postFix = ConvertPostFix(infix); //guardar los elementos convertidos a notacion postfija
            goToNode.Condition = (BinaryOperationNode)ParsePostFix(postFix); //asignarle el valor del resultado de parsear la notacion postfija al nodo de variable  
        }

        return goToNode;
    } 
}