using System.Linq.Expressions;

public abstract class ASTNode{}

public class FunctionNode : ASTNode
{
    public string Name { get; set; }
    public List<ASTNode> Parameters { get; set; }
    public FunctionNode (string name, List<ASTNode> parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}
public class VariableNode : ASTNode
{
    public string Name {get; set;}
    public ASTNode Value { get; set;}
    public VariableNode(string name, ASTNode value)
    {
        Name = name;
        Value = value;
    }
}
public class NumberLiteralNode : ASTNode
{
    public int Value { get; set; }
    public NumberLiteralNode(int value)
    {
        Value = value;
    }
}
public class StringLiteralNode : ASTNode
{
    public string Value { get; set; }
    public StringLiteralNode(string value)
    {
        Value = value;
    }
}
public class BooleanLiteralNode : ASTNode
{
    public bool Value { get; set; }
    public BooleanLiteralNode(bool value)
    {
        Value = value;
    }
}
public class GoToNode : ASTNode
{
    public string Label { get; set; }
    public ConditionNode Condition { get; set; }
    public GoToNode(string label, ConditionNode condition)
    {
        Label = label;
        Condition = condition;
    }
}
public class ConditionNode : ASTNode
{
    public bool Value {get;}
    public Token Operator {get; set;}
    public ASTNode LeftMember {get; set;}
    public ASTNode RightMember {get; set;}
    public ConditionNode(bool value, Token Operator , ASTNode leftMember, ASTNode rightMember)
    {
        Value = value;
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }
}
public class ArithmeticNode : ASTNode
{
    public string Operator {get; set;}
    public ASTNode LeftMember { get; set;}
    public ASTNode RightMember { get; set;}
    public ArithmeticNode(string Operator, ASTNode leftMember, ASTNode rightMember)
    {
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }
}