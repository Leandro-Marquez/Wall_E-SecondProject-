public class Parser
{
    Dictionary<string , int > operatorPrecedence = new Dictionary<string , int>()
    {
        {"+" , 1},
        {"-" , 1},
        {"*" , 2},
        {"/" , 2},
        {"%" , 2},
        {"**", 3},
    };
    
    private int currentIndex;
    public List<Token> tokens = new List<Token>();
    public List<ASTNode> aSTNodes = new List<ASTNode>();
    public Parser(List<Token> tokens)
    {
        currentIndex = 0;
        this.tokens = tokens;
    }
    public void Parse()
    {
        for (int i = 0 ; i < tokens.Count-1 ; i++)
        {
            if(tokens[i].Type == TokenType.Identifier && tokens[i+1].Value == "(" ) //manejar el caso en que sea un llamado a una funcion 
            {
                currentIndex = i;
                FunctionNode functionNode = ParseFunction();
                aSTNodes.Add(functionNode);
                i = currentIndex-1;
            }
            else if(tokens[i].Type == TokenType.Identifier && tokens[i+1].Type == TokenType.AssignmentOperator) //manejar el caso de que sea una asignacion de variable 
            {

            }
            
        }
    }
    private FunctionNode ParseFunction()
    {
        FunctionNode functionNode = new FunctionNode(tokens[currentIndex].Value,new List<ASTNode>());
        currentIndex += 2;
        while (tokens[currentIndex].Value != ")")
        {
            functionNode.Params.Add(ParseParams());
            currentIndex += 1;
        }
        return functionNode;
    }
    private ASTNode ParseParams()
    {
        List<Token> inFix = new List<Token>();
        if(tokens[currentIndex].Value == ",") currentIndex += 1;
        while (tokens[currentIndex].Value != "," && tokens[currentIndex].Value != ")")
        {
            inFix.Add(tokens[currentIndex]);
            currentIndex += 1;
        }
        List<Token> postFix = new List<Token>(); //recordar que tengo el indice sobre la ultima coma 
        postFix = ConvertPostFix(inFix);
        List<ASTNode> nodes = new List<ASTNode>(); 
        for (int i = 0; i < postFix.Count ; i++)
        {
            if(postFix[i].Type != TokenType.Number) nodes.Add(new NumberLiteralNode(int.Parse(postFix[i].Value)));
            else if(postFix[i].Type == TokenType.String) nodes.Add(new StringLiteralNode(postFix[i].Value));
            else if(postFix[i].Type == TokenType.Bool) nodes.Add(new BooleanLiteralNode(bool.Parse(postFix[i].Value)));
            else if(postFix[i].Type == TokenType.Identifier) nodes.Add(new VariableNode(postFix[i].Value, null)); //manejar nodos de variablessssssssssssssssssssssssssssssssssssss
            else if(postFix[i].Type == TokenType.ArithmeticOperator || postFix[i].Type == TokenType.LogicOperator)
            {
                BinaryOperationNode binaryOperationNode = new BinaryOperationNode(postFix[i] , nodes[nodes.Count-2] , nodes[nodes.Count-1]);
                nodes.RemoveAt(nodes.Count-1);
                nodes.RemoveAt(nodes.Count-1);
                nodes.Add(binaryOperationNode);
            }
            // System.Console.Write(postFix[i].Value + " ");
        }
        return nodes[0];
    }
    private List<Token> ConvertPostFix(List<Token> inFix)
    {
        List<Token> outPut = new List<Token>();
        List<Token> stackOperators = new List<Token>();

        for (int i = 0 ; i < inFix.Count ; i++)
        {
            if(inFix[i].Type != TokenType.ArithmeticOperator) outPut.Add(inFix[i]);
            else if(stackOperators.Count == 0) stackOperators.Add(inFix[i]);
            else 
            {
                for (int j = stackOperators.Count-1 ; j >= 0 ; j--)
                {
                    if(operatorPrecedence[stackOperators[j].Value.ToString()] >= operatorPrecedence[inFix[i].Value.ToString()])
                    {
                        outPut.Add(stackOperators[j]);
                        stackOperators.Remove(stackOperators[j]);
                    }
                }
                stackOperators.Add(inFix[i]);
            }
        }
        for (int i = stackOperators.Count - 1; i >= 0 ; i--)
        {
            outPut.Add(stackOperators[i]);
        }
        return outPut;
    }
}