using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public abstract class ASTNode
{
    public abstract void Print(string indent = "", bool last = true);
    public abstract object Evaluate(bool booleano);
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

    public override object Evaluate(bool booleano)
    {
        //manejas los casos de funciones especiales
        string aux = this.Name;
        switch (aux)
        {
            case "Spawn":
                return Assistant.EvaluateSpawn(this.Params);
            case "Color":
                return Assistant.EvaluateColor(this.Params);
            case "GetCanvasZize":  
                return Assistant.EvaluateGetCanvasZize(this.Params);
            case "GetActualX":
                return Assistant.EvaluateGetActualX(this.Params);
            case "GetActualY":  
                return Assistant.EvaluateGetActualY(this.Params);
            case "DrawLine":
                return Assistant.EvaluateDrawLine(this.Params);
            case "Size":
                return Assistant.EvaluateSize(this.Params);
            case "IsBrushSize":
                return Assistant.EvaluateIsBrushSize(this.Params);
            case "IsBrushColor":
                return Assistant.EvaluateIsBrushColor(this.Params);
            case "Fill":
                return Assistant.EvaluateFill(this.Params);
            case "GetColorCount" :
                return Assistant.EvaluateGetColorCount(this.Params);
            case "IsCanvasColor": 
                return Assistant.EvaluateIsCanvasColor(this.Params);
            case "DrawCircle":
                return Assistant.EvaluateDrawCircle(this.Params);
            case "DrawRectangle":
                return Assistant.EvaluateDrawRectangle(this.Params);
        }
        Error.errors.Add((ErrorType.Semantic_Error,$"There is not a definition for method {aux}"));
        return null;
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

    public override object Evaluate(bool booleano)
    {
        if(booleano) Context.variablesValues[Name] = Value.Evaluate(false);
        else return Context.variablesValues[Name];
        return null;
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

    public override object Evaluate(bool booleano)
    {
        return Value;
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

    public override object Evaluate(bool booleano)
    {
        return Value;
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

    public override object Evaluate(bool booleano)
    {
        return Value;
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

    public override object Evaluate(bool booleano)
    {
        Debug.Log("Etiqueta: " + Label + " Indice : " + Context.labels[Label]);
        return Label;
    }
}

public class GoToNode : ASTNode
{
    public LabelNode Label { get; set; }
    public BinaryOperationNode Condition { get; set; }

    public GoToNode(LabelNode label, BinaryOperationNode condition)
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

    public override object Evaluate(bool booleano)
    {
        PixelCanvasController.gotoBoolean = true;
        var condition = Condition.Evaluate(false);
        if(condition is bool)
        {
            if((bool)condition) Context.indexOfEvaluation = Context.labels[Label.Label];
            else PixelCanvasController.gotoBoolean = false;
        }
        else Error.errors.Add((ErrorType.Run_Time_Error ,"GoTo's Condition must evaluate a Boolean Value"));
        return null;
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

    public override object Evaluate(bool booleano)
    {
        return Assistant.EvaluateBinaryOperationNode(LeftMember, Operator, RightMember);
    }

}