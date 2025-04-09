using System;
using System.Collections.Generic;

public abstract class ASTNode
{
    public abstract void Print(string indent = "", bool last = true);
}

public class FunctionNode : ASTNode
{
    public string Name { get; set; }
    public List<ASTNode> Params { get; set; }

    public FunctionNode(string name, List<ASTNode> Params)
    {
        Name = name;
        this.Params = Params;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Function: {Name}");

        indent += last ? "   " : "│  ";
        
        for (int i = 0; i < Params.Count; i++)
        {
            Params[i].Print(indent, i == Params.Count - 1);
        }
    }
}

public class VariableNode : ASTNode
{
    public string Name { get; set; }
    public ASTNode Value { get; set; }

    public VariableNode(string name, ASTNode value)
    {
        Name = name;
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Variable: {Name}");
        
        Value?.Print(indent + (last ? "   " : "│  "), true);
    }
}

public class NumberLiteralNode : ASTNode
{
    public int Value { get; set; }

    public NumberLiteralNode(int value)
    {
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Number: {Value}");
    }
}

public class StringLiteralNode : ASTNode
{
    public string Value { get; set; }

    public StringLiteralNode(string value)
    {
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"String: \"{Value}\"");
    }
}

public class BooleanLiteralNode : ASTNode
{
    public bool Value { get; set; }

    public BooleanLiteralNode(bool value)
    {
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Boolean: {Value}");
    }
}

public class LabelNode : ASTNode
{
    public string Label { get; set; }

    public LabelNode(string label)
    {
        Label = label;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Label: {Label}");
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

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"GoTo: {Label}");
        
        Condition?.Print(indent + (last ? "   " : "│  "), true);
    }
}

public class ConditionNode : ASTNode
{
    public bool Value { get; }
    public Token Operator { get; set; }
    public ASTNode LeftMember { get; set; }
    public ASTNode RightMember { get; set; }

    public ConditionNode(bool value, Token Operator, ASTNode leftMember, ASTNode rightMember)
    {
        Value = value;
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Condition (Operator: {Operator?.Value})");
        
        LeftMember?.Print(indent + (last ? "   " : "│  "), false);
        RightMember?.Print(indent + (last ? "   " : "│  "), true);
    }
}

public class BinaryOperationNode : ASTNode
{
    public Token Operator { get; set; }
    public ASTNode LeftMember { get; set; }
    public ASTNode RightMember { get; set; }

    public BinaryOperationNode(Token Operator, ASTNode leftMember, ASTNode rightMember)
    {
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Console.Write(indent);
        Console.Write(last ? "└──" : "├──");
        Console.WriteLine($"Binary Operation (Operator: {Operator?.Value})");
        
        LeftMember?.Print(indent + (last ? "   " : "│  "), false);
        RightMember?.Print(indent + (last ? "   " : "│  "), true);
    }
}