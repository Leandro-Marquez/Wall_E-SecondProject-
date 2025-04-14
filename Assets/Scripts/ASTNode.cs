using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public abstract class ASTNode
{
    public abstract void Print(string indent = "", bool last = true);
    public abstract object Evaluate();
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

    public override object Evaluate()
    {
        //manejas los casos de funciones especiales
        if(this.Name == "Spawn")
        {
            if(this.Params.Count != 2) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Spawn()"));
            else 
            {
                var x = Params[0].Evaluate();
                var y = Params[1].Evaluate();
                if(x is int xValue && y is int yValue)
                {
                    if(xValue >= 0 && xValue < Context.canvasSize && yValue >= 0 && yValue < Context.canvasSize)
                    {
                        Context.wallEPosition.x = (int)x;
                        Context.wallEPosition.y = (int)y;    
                        Context.wallEPositionChanged = true;
                    }
                    else Error.errors.Add((ErrorType.Run_Time_Error, "Spawn() parameters must be positive and less than Canvas'Size"));
                }
                else
                {
                    // Mensaje de error detallado
                    string errorMsg = "Invalid types in 'Spawn': ";
                    if (x is not int) 
                        errorMsg += $"X must be int, but got '{x?.GetType().Name}'. ";
                    if (y is not int) 
                        errorMsg += $"Y must be int, but got '{y?.GetType().Name}'.";
                    Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                }
            }
        }
        if(this.Name == "Color")
        {
            if(this.Params.Count != 1) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Color()"));
            else
            {
                var a = Params[0].Evaluate();
                if(a is not string aValue) Error.errors.Add((ErrorType.Run_Time_Error, "Color() parameter must be String Type"));
                switch (a)
                {
                    case "Red":
                        Context.brushColor = a.ToString();
                        break;
                    case "Blue":
                        Context.brushColor = a.ToString();
                        break;
                    case "Green":
                        Context.brushColor = a.ToString();
                        break;
                    case "Yellow":
                        Context.brushColor = a.ToString();
                        break;
                    case "Orange":
                        Context.brushColor = a.ToString();
                        break;
                    case "Purple":
                        Context.brushColor = a.ToString();
                        break;
                    case "Black":
                        Context.brushColor = a.ToString();
                        break;
                    case "White":
                        Context.brushColor = a.ToString();
                        break;
                    case "Transparent":
                        Context.brushColor = a.ToString();
                        break;
                    default:
                        Error.errors.Add((ErrorType.Run_Time_Error, "Current expresion is not valid like a Color Type"));
                        break;
                }
            }
        }
        return null;
        // throw new System.NotImplementedException(); //implementarrrrrrrrrrrrrrrrrr
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

    public override object Evaluate()
    {
       return Value.Evaluate();
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

    public override object Evaluate()
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

    public override object Evaluate()
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

    public override object Evaluate()
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

    public override object Evaluate()
    {
        throw new System.NotImplementedException(); //implementarrrrrrrrrrrrrrrr
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

    public override object Evaluate()
    {
        throw new System.NotImplementedException();//implementarrrrrrrrrrrrrrrrrr
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

    public override object Evaluate()
    {
        return Operator.Value switch
        {
            "+"  => (int)LeftMember.Evaluate() + (int)RightMember.Evaluate(),
            "-"  => (int)LeftMember.Evaluate() - (int)RightMember.Evaluate(),
            "*"  => (int)LeftMember.Evaluate() * (int)RightMember.Evaluate(),
            "/"  => (int)LeftMember.Evaluate() / (int)RightMember.Evaluate(),
            "%"  => (int)LeftMember.Evaluate() % (int)RightMember.Evaluate(),
            "**" => (int)Math.Pow((int)RightMember.Evaluate(),(int)LeftMember.Evaluate()),/*Convert.ToInt32(Math.Pow((double)LeftMember.Evaluate() , (double)RightMember.Evaluate()))*/
            "==" => AreEqual(LeftMember.Evaluate(), RightMember.Evaluate()),
            "!=" => AreEqual(LeftMember.Evaluate(), RightMember.Evaluate()),
            ">=" => (int)LeftMember.Evaluate() >= (int)RightMember.Evaluate(),
            ">"  => (int)LeftMember.Evaluate() > (int)RightMember.Evaluate(),
            "<=" => (int)LeftMember.Evaluate() <= (int)RightMember.Evaluate(),
            "<"  => (int)LeftMember.Evaluate() < (int)RightMember.Evaluate(),
            "||" => (bool)LeftMember.Evaluate() || (bool)RightMember.Evaluate(),
            "&&" => (bool)LeftMember.Evaluate() && (bool)RightMember.Evaluate(),
        };
    }
    private bool AreEqual(object left, object right) //mejorarrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr con los erroressssssssssssss
    {
        if (left.GetType() != right.GetType())
            throw new InvalidOperationException($"No se pueden comparar {left.GetType().Name} y {right.GetType().Name}");

        return object.Equals(left, right);
    }
}