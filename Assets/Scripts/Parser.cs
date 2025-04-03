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
    public Parser(List<Token> tokens) //constructor de la clase 
    {
        currentIndex = 0; //inicializar el indice en cero como es logico se comienza por la primera posicion 
        this.tokens = tokens; //inicializar la lista de tokens con la que se le pasa al constructor 
    }
    public void Parse() //metodo principal para parsear la lista de tokens 
    {
        for (int i = 0 ; i < tokens.Count-1 ; i++) //se itera hasta el ultimo token entr " "
        {
            if(tokens[i].Type == TokenType.Identifier && tokens[i+1].Value == "(" ) //manejar el caso en que sea un llamado a una funcion 
            {
                currentIndex = i; //actualizar el indice con la posicion del token actual 
                FunctionNode functionNode = ParseFunction(); //llamar a parsear funcion y guardarlo en el nodo de funcion 
                aSTNodes.Add(functionNode); //agregar el nodo de funcion a la lista de nodos a evaluar 
                i = currentIndex-1; //actualizar con el ultimo token chequeado por los demas metodos menos 1 para cuandpo el incremento caiga en el objetivo 
            }
            else if(tokens[i].Type == TokenType.Identifier && tokens[i+1].Type == TokenType.AssignmentOperator) //manejar el caso de que sea una asignacion de variable 
            {
                currentIndex = i; //actualizar nuevamente el indice con la posicion del token actual 
                VariableNode variableNode = ParseVariable(); //llamar a parsear variable y guardarlo en su respectivo nodo 
                aSTNodes.Add(variableNode); //agregar el nodo a la lista de nodos a evaluar posteriormente 
            }
        }
    }
    private VariableNode ParseVariable()
    {
        VariableNode variableNode = new VariableNode(tokens[currentIndex].Value,null);
        currentIndex += 1; //estoy sobre el primer valor de la asignacion a la variable 
        if(tokens[currentIndex + 1].Type == TokenType.ArithmeticOperator)
        {
            List<Token> Infix = new List<Token>();
            while(tokens[currentIndex+1].Type == TokenType.ArithmeticOperator)
            {
                Infix.Add(tokens[currentIndex]);
                Infix.Add(tokens[currentIndex+1]);
                currentIndex += 1;
            }
            Infix.Add(tokens[currentIndex]);
            List<Token> postFix = ConvertPostFix(Infix);

        }
        else if (tokens[currentIndex + 1].Value == "(")
        {

        }
        return null;
    }
    private FunctionNode ParseFunction() //parsear funcion 
    {
        FunctionNode functionNode = new FunctionNode(tokens[currentIndex].Value,new List<ASTNode>()); //inicializar el nodo de funcion con su nobre correspondiente y sin parametros 
        currentIndex += 2; //incrementar el indice de manera que caiga sobre el o no parametro 
        while (tokens[currentIndex].Value != ")") 
        {
            functionNode.Params.Add(ParseParams());//agregar a la lista de parametros el parametro que se parseara
            currentIndex += 1; //aumentar el indice luego de parseado el parametro 
        }
        return functionNode;
    }
    private ASTNode ParseParams() //parsear parametros 
    {
        List<Token> inFix = new List<Token>(); //lista de elementos en notacion infija 
        if(tokens[currentIndex].Value == ",") currentIndex += 1; //si se llega aqui encima de la coma avanzar una posicion hacia el parametro objetivo 
        while (tokens[currentIndex].Value != "," && tokens[currentIndex].Value != ")") //mientras que no se tenga un parentesis o una coma se esta parseando un parametro 
        {
            inFix.Add(tokens[currentIndex]); //agregar a la lista de elementos en notacion infija 
            currentIndex += 1; //aumentar el indice 
        }
        List<Token> postFix = new List<Token>(); //inicializar la lista de elementos en notacion postfija
        postFix = ConvertPostFix(inFix); //actualizar la lista con la notacion infija ya convertida en notacion postfija
        return ParsePostFix(postFix); //parsear la notacion postfija 
    }
    private ASTNode ParsePostFix(List<Token> postFix) //parsear la notacion postfija 
    {
        List<ASTNode> nodes = new List<ASTNode>(); //lista de nodos que se tendran mientras se vaya parseando 
        for (int i = 0; i < postFix.Count ; i++) //iterar por todos tokens en notacion infija 
        {
            if(postFix[i].Type == TokenType.Number) nodes.Add(new NumberLiteralNode(int.Parse(postFix[i].Value)));//si el tipo de token actual es un numero agregar a la lista de nodos un nodo literal numerico 
            else if(postFix[i].Type == TokenType.String) nodes.Add(new StringLiteralNode(postFix[i].Value));//si el tipo de token actual es un string agregar a la lista de nodos un nodo literal de string  
            else if(postFix[i].Type == TokenType.Bool) nodes.Add(new BooleanLiteralNode(bool.Parse(postFix[i].Value)));//si el tipo de token actual es un booleano agregar a la lista de nodos un nodo literal booleano
            else if(postFix[i].Type == TokenType.Identifier) nodes.Add(new VariableNode(postFix[i].Value, null)); //manejar nodos de variablessssssssssssssssssssssssssssssssssssss
            //si el tipo de token actual es un operador aritmetico o logico se debe crear un nodo de operacion bineria con el los ultimos dos nodos como hijos derecho e izquierdo   
            else if(postFix[i].Type == TokenType.ArithmeticOperator || postFix[i].Type == TokenType.LogicOperator)
            {
                BinaryOperationNode binaryOperationNode = new BinaryOperationNode(postFix[i] , nodes[nodes.Count-2] , nodes[nodes.Count-1]); //crear nodo con el operador actual y con los respectivos dos ultimos nodos como hijos 
                nodes.RemoveAt(nodes.Count-1); //una vez se creo el nodo elimnar los dos ultimos nodos hojas
                nodes.RemoveAt(nodes.Count-1); //...
                nodes.Add(binaryOperationNode); //agregar a la lista de nodos el nuevo nodo ya creado 
            }
        }
        return nodes[0];
    }
    private List<Token> ConvertPostFix(List<Token> inFix) //convertir de notacion infija a postfija
    {
        List<Token> outPut = new List<Token>(); //lista de salida postfija
        List<Token> stackOperators = new List<Token>(); //pila de operadores

        for (int i = 0 ; i < inFix.Count ; i++) //iterar por los elementos infijos 
        {
            if(inFix[i].Type != TokenType.ArithmeticOperator) outPut.Add(inFix[i]); //mientras que no sea un operador aritmetico significa que puede ser una variable 
            else if(stackOperators.Count == 0) stackOperators.Add(inFix[i]); //si no se tienen operadores en la pila no es necesario ninguna verificacion 
            else //si es un operador y ya se tiene operadores 
            {
                for (int j = stackOperators.Count-1 ; j >= 0 ; j--) //verificar la pila de operadores 
                {
                    //si se tiene alguno con mahyor e igual precedencia que el que se tiene se desapila 
                    if(operatorPrecedence[stackOperators[j].Value.ToString()] >= operatorPrecedence[inFix[i].Value.ToString()])
                    {
                        outPut.Add(stackOperators[j]); //agregar a la salida postfija 
                        stackOperators.Remove(stackOperators[j]); //eliminar el operador agregado 
                    }
                }
                stackOperators.Add(inFix[i]); //una vez se desapilaron los que se tienen que desapilar, agregar el que se tenia a la pila de operadores 
            }
        }
        //una vez se tiene una notacion postfija agregar los operadores restantes de la pila 
        for (int i = stackOperators.Count - 1; i >= 0 ; i--) //iterar de atras para alante para simular la pila
        {
            outPut.Add(stackOperators[i]); //agregar cada operador a la postfija
        }
        return outPut; //retornar la salida postfija
    }
}