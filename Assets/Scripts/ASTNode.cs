using UnityEngine;
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Function: {Name}");

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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Variable: {Name}");
        
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Number: {Value}");
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"String: \"{Value}\"");
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Boolean: {Value}");
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Label: {Label}");
    }
}

public class GoToNode : ASTNode
{
    public LabelNode Label { get; set; }
    public ASTNode Condition { get; set; }

    public GoToNode(LabelNode label, ASTNode condition)
    {
        Label = label;
        Condition = condition;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + "GoTo");
        
        // Imprimir la etiqueta (si existe)
        indent += last ? "   " : "│  ";
        if (Label != null)
        {
            Debug.Log(indent + "├──" + $"Label: {Label.Label}");
        }
        else
        {
            Debug.Log(indent + "├──" + "Label: (null)");
        }
    
        // Imprimir la condición (si existe)
        Debug.Log(indent + "└──" + "Condition:");
        Condition?.Print(indent + "   ", true);
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
        Debug.Log(indent + (last ? "└──" : "├──") + $"Binary Operation (Operator: {Operator?.Value})");
        
        LeftMember?.Print(indent + (last ? "   " : "│  "), false);
        RightMember?.Print(indent + (last ? "   " : "│  "), true);
    }
}