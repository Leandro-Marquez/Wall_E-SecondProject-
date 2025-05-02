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
        string aux = this.Name;
        switch (aux)
        {
            case "Spawn":
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
                break;
            case "Color":
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
                break;
            case "GetCanvasZize":  
                if(this.Params.Count > 0)
                {
                    Error.errors.Add((ErrorType.Run_Time_Error , "GetCanvasZize() does not contains parameters"));
                    return null;
                }
                else return Context.canvasSize;
                break;
            case "GetActualX":
                if(this.Params.Count > 0)
                {
                    Error.errors.Add((ErrorType.Run_Time_Error, "GetActualX() does not contains parameters"));
                    return null;
                }
                else return Context.wallEPosition.x;
                break;
            case "GetActualY":    
                if(this.Params.Count > 0) Error.errors.Add((ErrorType.Run_Time_Error, "GetActualY() does not contains parameters"));
                else return Context.wallEPosition.y;
                break;
            case "DrawLine":
                if(this.Params.Count != 3) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of DrawLine()"));
                List<int> ints = new List<int>();
                for (var i = 0; i < this.Params.Count ; i++)
                {
                    var a = this.Params[i].Evaluate();
                    if(a is not int) Error.errors.Add((ErrorType.Run_Time_Error,"DrawLine's Method must recibe Int's Type"));
                    else ints.Add((int)a);
                }
                (int x , int y) dir;
                dir.y = ints[0];
                dir.x = ints[1];
                for (var i = 1 ; i <= ints[2] ; i++)
                {
                    int newX = Context.wallEPosition.x + dir.x;
                    int newY = Context.wallEPosition.y + dir.y;
                    if(newX >= 0 && newY >= 0 && newX < Context.canvasSize && newY < Context.canvasSize)
                    {
                        Context.wallEPosition.x = newX;
                        Context.wallEPosition.y = newY;
                        Context.Paint(Context.wallEPosition.x,Context.wallEPosition.y);
                    }
                    else break;
                }
                break;
            case "Size":
                if(this.Params.Count != 1) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Size()"));
                else
                {
                    var a = Params[0].Evaluate();
                    if(a is int) Context.pincelZize = (int)a;    
                    else Error.errors.Add((ErrorType.Run_Time_Error,"Size's Method must recibe Int's Type"));
                }
                break;
            case "IsBrushSize":
                if(this.Params.Count != 1) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsBrushSize()"));
                else
                {
                    var a = Params[0].Evaluate();
                    if(a is int)
                    {
                        if(Context.pincelZize == (int)a) return 1;
                        else return 0;
                    }  
                    else Error.errors.Add((ErrorType.Run_Time_Error,"IsBrushSize's Method must recibe Int's Type"));
                }
                break;
            case "IsBrushColor":
                if(this.Params.Count != 1) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsBrushColor()"));
                else
                {
                    var a = Params[0].Evaluate();
                    if(a is string)
                    {
                        switch (a)
                        {
                            case "Red":
                                return Context.brushColor = (string)a;
                            case "Blue":
                                return Context.brushColor = (string)a;
                            case "Green":
                                return Context.brushColor = (string)a;
                            case "Yellow":
                                return Context.brushColor = (string)a;
                            case "Orange":
                                return Context.brushColor = (string)a;
                            case "Purple":
                                return Context.brushColor = (string)a;
                            case "Black":
                                return Context.brushColor = (string)a;
                            case "White":
                                return Context.brushColor = (string)a;
                            case "Transparent":
                                return Context.brushColor = (string)a;
                            default:
                                Error.errors.Add((ErrorType.Run_Time_Error, "IsBrushColor's Method must recibe a valid Color Type"));
                                break;
                        }
                    }
                    else Error.errors.Add((ErrorType.Run_Time_Error,"IsBrushSize's Method must recibe String's Type"));
                }
                break;
            case "Fill":
                if(this.Params.Count != 0) Error.errors.Add((ErrorType.Run_Time_Error,"Fill's Method does not contains params"));
                else
                {
                    List<(int x , int y)> values = new List<(int x , int y)> ();
                    values.Add((Context.wallEPosition.x,Context.wallEPosition.y));
                    string color = ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(Context.wallEPosition.x,Context.wallEPosition.y));
                    Context.Paint(Context.wallEPosition.x,Context.wallEPosition.y);
                    for (var i = 0; i < values.Count ; i++)
                    {
                        int [] dirx = { 1 , -1 , 0 , 0 , -1 ,-1 , 1 , 1 };
                        int [] diry = { 0 ,  0 , 1 ,-1 , -1 , 1 , 1 ,-1 };
                        for (var j = 1 ; j < dirx.Length ; j++)
                        {
                            int newX = values[i].x + dirx[j];
                            int newY = values[i].y + diry[j];
                            if(newX >= 0 && newX < Context.canvasSize && newY >= 0 && newY < Context.canvasSize && color == ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(newX,newY)) && !values.Contains((newX,newY)))
                            {
                                values.Add((newX,newY));
                                Context.Paint(newX,newY);
                            }
                        }
                    }
                } 
                break;
            // case "DrawCircle":
                
            //     break;
        }
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

    public override object Evaluate()
    {
        
       return Context.variablesValues[Name].Evaluate();
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
    public object Value { get; set; }

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
        return Label;
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
            //hacer chequeo de tipoossssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            "+"  => (int)LeftMember.Evaluate() + (int)RightMember.Evaluate(),
            "-"  => (int)LeftMember.Evaluate() - (int)RightMember.Evaluate(),
            "*"  => (int)LeftMember.Evaluate() * (int)RightMember.Evaluate(),
            "/"  => (int)LeftMember.Evaluate() / (int)RightMember.Evaluate(),
            "%"  => (int)LeftMember.Evaluate() % (int)RightMember.Evaluate(),
            "**" => (int)Math.Pow((int)RightMember.Evaluate(),(int)LeftMember.Evaluate()),
            "==" => AreEqual(LeftMember.Evaluate(), RightMember.Evaluate()),
            "!=" => !AreEqual(LeftMember.Evaluate(), RightMember.Evaluate()),
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