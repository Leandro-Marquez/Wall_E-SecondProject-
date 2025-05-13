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
                        if (x is not int) errorMsg += $"X must be int, but got '{x?.GetType().Name}'. ";
                        if (y is not int) errorMsg += $"Y must be int, but got '{y?.GetType().Name}'.";
                        Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                    }
                }
                break;
            case "Color":
                if(this.Params.Count != 1) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Color()"));
                else
                {
                    var a = Params[0].Evaluate();
                    System.Drawing.Color color;
                    if(a is not string aValue) Error.errors.Add((ErrorType.Run_Time_Error, "Color() parameter must be String Type"));
                    switch (a)
                    {
                        case "Red":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Red;
                            Context.brushColorColor = color;
                            break;
                        case "Blue":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Blue;
                            Context.brushColorColor = color;
                            break;
                        case "Green":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Green;
                            Context.brushColorColor = color;
                            break;
                        case "Yellow":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Yellow;
                            Context.brushColorColor = color;
                            break;
                        case "Orange":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Orange;
                            Context.brushColorColor = color;
                            break;
                        case "Purple":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Purple;
                            Context.brushColorColor = color;
                            break;
                        case "Black":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.Black;
                            Context.brushColorColor = color;
                            break;
                        case "White":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.White;
                            Context.brushColorColor = color;
                            break;
                        case "Transparent":
                            Context.brushColor = a.ToString();
                            color = System.Drawing.Color.White;
                            Context.brushColorColor = color;
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
            case "GetActualX":
                if(this.Params.Count > 0)
                {
                    Error.errors.Add((ErrorType.Run_Time_Error, "GetActualX() does not contains parameters"));
                    return null;
                }
                else return Context.wallEPosition.x;
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
                dir.y = ints[1];
                dir.x = ints[0];
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
                                Error.errors.Add((ErrorType.Run_Time_Error, $"{a} is not a valid Color Type"));
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
                    string color = ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(Context.wallEPosition.x,Context.wallEPosition.y));
                    Queue<(int x , int y)> values = new Queue<(int x , int y)> ();
                    values.Enqueue((Context.wallEPosition.x,Context.wallEPosition.y));
                 
                    Context.Paint(Context.wallEPosition.x,Context.wallEPosition.y);
                    // Debug.Log(values.Count + " siiiiii");
                    bool [,] bools = new bool[Context.canvasSize,Context.canvasSize];
                    while (values.Count > 0)
                    {
                        (int , int ) auxi = values.Dequeue();
                        // if(bools[auxi.Item1,auxi.Item2]) continue;
                        bools[auxi.Item1,auxi.Item2] = true;

                        int [] dirx = { 1 , -1 , 0 , 0 };
                        int [] diry = { 0 ,  0 , 1 ,-1 };
                        for (var j = 1 ; j < dirx.Length ; j++)
                        {
                            int newX = auxi.Item1 + dirx[j];
                            int newY = auxi.Item2 + diry[j];
                            if(newX >= 0 && newX < Context.canvasSize && newY >= 0 && newY < Context.canvasSize && color == ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(newX,newY)))
                            {
                                if(!bools[newX,newY])
                                {
                                    values.Enqueue((newX,newY));
                                    Context.Paint(newX,newY);
                                }
                                
                            }
                        }
                    }
                } 
                break;
            case "GetColorCount" :
                if(this.Params.Count != 5) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of GetColorCount()"));
                else
                {
                    var color = Params[0].Evaluate();
                    var x1 = Params[1].Evaluate();
                    var y1 = Params[2].Evaluate();
                    var x2 = Params[3].Evaluate();
                    var y2 = Params[4].Evaluate();
                    if(x1 is int x1Value && y1 is int y1Value && x2 is int x2Value && y2 is int y2Value && color is string colorValue)
                    {
                        if(x1Value >= 0 && x1Value < Context.canvasSize && y1Value >= 0 && y1Value < Context.canvasSize && x2Value >= 0 && x2Value < Context.canvasSize && y2Value >= 0 && y2Value < Context.canvasSize)
                        {
                            bool validColor = false;
                            switch (color)
                            {
                                case "Red":
                                    validColor = true;
                                    break;
                                case "Blue":
                                    validColor = true;
                                    break;
                                case "Green":
                                    validColor = true;
                                    break;
                                case "Yellow":
                                    validColor = true;
                                    break;
                                case "Orange":
                                    validColor = true;
                                    break;
                                case "Purple":
                                    validColor = true;
                                    break;
                                case "Black":
                                    validColor = true;
                                    break;
                                case "White":
                                    validColor = true;
                                    break;
                                case "Transparent":
                                    validColor = true;
                                    break;
                                default:
                                    Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to ColorType"));
                                    break;
                            }
                            if(validColor)
                            {
                                int n = 0;
                                for (var i = (int)x1 ; i <= (int)x2 ; i++)
                                {
                                    for (var j = (int)y1 ; j <= (int)y2 ; j++)
                                    {
                                        if(ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(i,j)) == (string)color) n += 1; 
                                    }
                                }
                                return n;
                            }
                        }
                        else Error.errors.Add((ErrorType.Run_Time_Error, "GetColorCount() parameters must be positive and less than Canvas'Size"));
                    }
                    else
                    {
                        string errorMsg = "Invalid types in GetColorCount: ";
                        if (x1 is not int) 
                            errorMsg += $"X1 must be int, but got '{x1?.GetType().Name}'. ";
                        if (y1 is not int) 
                            errorMsg += $"Y1 must be int, but got '{y1?.GetType().Name}'. ";
                        if (x2 is not int) 
                            errorMsg += $"X2 must be int, but got '{x2?.GetType().Name}'. ";
                        if (y2 is not int) 
                            errorMsg += $"Y2 must be int, but got '{y2?.GetType().Name}'. ";
                        if (color is not string) 
                            errorMsg += $"Color must be String, but got '{color?.GetType().Name}'.";
                        Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                    } 
                }
                break;
            case "IsCanvasColor": 
                if(this.Params.Count != 3) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsCanvasColor()"));
                else
                {
                    var color = Params[0].Evaluate();
                    var x = Params[1].Evaluate();
                    var y = Params[2].Evaluate();
                    if(color is string colorValue && x is int xValue && y is int yValue)
                    {
                        if((int)x >= 0 && (int)x < Context.canvasSize && (int)y >= 0 && (int)y < Context.canvasSize)
                        {
                            string colorAux = ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel((int) y ,(int) x));
                            if(colorAux == (string)color) return 1;
                            else return 0;
                        }
                        else Error.errors.Add((ErrorType.Run_Time_Error,"IsCanvasColor() parameters [x,y] must be positive and less than Canvas'Size "));
                    }
                    else
                    {
                        string errorMsg = "Invalid types in IsCanvasColor: ";
                        if (x is not int) 
                            errorMsg += $"X must be int, but got '{x?.GetType().Name}'. ";
                        if (y is not int) 
                            errorMsg += $"Y must be int, but got '{y?.GetType().Name}'.";
                        if (color is not string) 
                            errorMsg += $"Color must be string, but got '{color?.GetType().Name}'. ";
                        Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                    }
                }
                break;
            case "DrawCircle":
                if(this.Params.Count != 3) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of DrawCircle()"));
                else
                {
                    var x = Params[0].Evaluate();
                    var y = Params[1].Evaluate();
                    var radius = Params[2].Evaluate();
                    if(x is int xValue && y is int yValue && radius is int radiusValue && (int)radius > 0)
                    { 
                        // Posición inicial de Wall-E (centro del círculo)
                        int xc = Context.wallEPosition.x;
                        int yc = Context.wallEPosition.y;
                        
                        int xPos = 0;
                        int yPos = (int)radius;
                        int d = 3 - 2 * (int)radius;
                        
                        // Dibujar los puntos iniciales
                        DrawCirclePoints(xc, yc, xPos, yPos);
                        
                        while (yPos >= xPos)
                        {
                            xPos++;
                            
                            // Aplicar el algoritmo del punto medio
                            if (d > 0)
                            {
                                yPos--;
                                d = d + 4 * (xPos - yPos) + 10;
                            }
                            else d = d + 4 * xPos + 6;
                            DrawCirclePoints(xc, yc, xPos, yPos);
                        }
                        
                        // Mover Wall_E al centro del círculo
                        Context.wallEPosition.x = xc;
                        Context.wallEPosition.y = yc;
                    } 
                    else
                    {
                        string errorMsg = "Invalid types in DrawCircle: ";
                        if (x is not int) 
                            errorMsg += $"dirX must be int, but got '{x?.GetType().Name}'. ";
                        if (y is not int) 
                            errorMsg += $"dirY must be int, but got '{y?.GetType().Name}'.";
                        if (radius is not int) 
                            errorMsg += $"radius must be int, but got '{radius?.GetType().Name}'. ";
                        if (radius is int && (int)radius < 0)
                            errorMsg += "radius must be positive and less than Canvas's Size";
                        Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                    }
                }
                break;
            case "DrawRectangle":
                if(this.Params.Count != 5) Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of DrawRectangule()"));
                else
                {
                    var dirX = Params[0].Evaluate();
                    var dirY = Params[1].Evaluate();
                    var distance = Params[2].Evaluate();
                    var width = Params[3].Evaluate();
                    var heigth = Params[4].Evaluate();
                    if(dirX is int dirXValue && dirY is int dirYValue && distance is int distanceValue && width is int widthValue && heigth is int heightValue && (int)distance > 0 && (int)width > 0 && (int)heigth > 0)
                    { 
                        // Obtener la posición actual de Wall-E
                        int actualX = Context.wallEPosition.x;
                        int actualY = Context.wallEPosition.y;

                        // 2. Calcular centro del rectángulo
                        int centerX = actualX + (int)dirX * (int)distance;
                        int centerY = actualY + (int)dirY * (int)distance;

                        // 3. Calcular bordes absolutos (corregido)
                        int left = centerX - (int)width / 2;
                        int right = left + (int)width - 1;  // -1 para mantener dimensiones exactas
                        int top = centerY - (int)heigth / 2;
                        int bottom = top + (int)heigth - 1;

                        // 4. Obtener tamaño del pincel
                        int brushSize = Context.pincelZize;
                        int halfBrush = brushSize / 2;

                        // 5. Dibujar rectángulo (versión corregida)
                        for (int x = left; x <= right; x++)
                        {
                            for (int y = top; y <= bottom; y++)
                            {
                                // Dibujar solo los bordes
                                bool isBorder = (x == left || x == right || y == top || y == bottom);

                                if (isBorder && x >= 0 && x < Context.canvasSize && y >= 0 && y < Context.canvasSize)
                                {
                                    // Aplicar grosor del pincel
                                    for (int bx = -halfBrush; bx <= halfBrush; bx++)
                                    {
                                        for (int by = -halfBrush; by <= halfBrush; by++)
                                        {
                                            int px = x + bx;
                                            int py = y + by;
                                            if (px >= 0 && px < Context.canvasSize && py >= 0 && py < Context.canvasSize)
                                            {
                                                Context.Paint(px, py);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Actualizar la posición de Wall-E al centro del rectángulo
                        Context.wallEPosition.x = centerX;
                        Context.wallEPosition.y = centerY;
                    }  
                    else
                    {
                        string errorMsg = "Invalid types in DrawCircle: ";
                        if (dirX is not int) 
                            errorMsg += $"dirX must be int, but got '{dirX?.GetType().Name}'. ";
                        if (dirY is not int) 
                            errorMsg += $"dirY must be int, but got '{dirY?.GetType().Name}'.";
                        if (distance is not int) 
                            errorMsg += $"Distance must be int, but got '{distance?.GetType().Name}'. ";
                        if(distance is int && (int)distance < 0)
                            errorMsg += "Distance must be positive and less than Canvas's Size";
                        if (width is not int) 
                            errorMsg += $"Width must be int, but got '{width?.GetType().Name}'. ";
                        if(width is int && (int)width < 0)
                            errorMsg += "Widht must be positive and less than Canvas's Size";
                        if (heigth is not int) 
                            errorMsg += $"Height must be int, but got '{heigth?.GetType().Name}'. ";
                        if(heigth is int && (int)heigth < 0)
                            errorMsg += "Heigth must be positive and less than Canvas's Size";
                        Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
                    }
                }
                break;
        }
        return null;
    }
    private void DrawCirclePoints(int xc, int yc, int x, int y)
    {
        // Dibujar los 8 puntos simétricos del círculo
        Context.Paint(xc + x, yc + y);
        Context.Paint(xc - x, yc + y);
        Context.Paint(xc + x, yc - y);
        Context.Paint(xc - x, yc - y);
        Context.Paint(xc + y, yc + x);
        Context.Paint(xc - y, yc + x);
        Context.Paint(xc + y, yc - x);
        Context.Paint(xc - y, yc - x);
    }

     private bool [,] bools = new bool[Context.canvasSize,Context.canvasSize];


    private void DFS(int x, int y, string color)
    {
        Debug.Log(x + "  " + y);
        Debug.Log(color);
        bools[x,y] = true;
        Context.Paint(x,y);
        int [] dirx = { 1 , -1 , 0 , 0 , -1 ,-1 , 1 , 1 };
        int [] diry = { 0 ,  0 , 1 ,-1 , -1 , 1 , 1 ,-1 };
                        for (var j = 1 ; j < dirx.Length ; j++)
                        {
                            int newX = x + dirx[j];
                            int newY = y + diry[j];
                            if(newX >= 0 && newX < Context.canvasSize && newY >= 0 && newY < Context.canvasSize && color == ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(newX,newY)))
                            {
                                if(!bools[newX,newY])
                                {
                                    DFS(newX,newY,color);
                                }
                                
                            }
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
        PixelCanvasController.gotoBoolean = true;
        var condition = Condition.Evaluate();
        if(condition is bool)
        {
            if((bool)condition)
            {
                Context.indexOfEvaluation = Context.labels[Label.Label];
            }
            else
            {
                Context.indexOfEvaluation += 1;
            }
        }
        else Error.errors.Add((ErrorType.Run_Time_Error ,"GoTo's Condition must evaluate a boolean value"));
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

    public override object Evaluate()
    {
        // Debug.Log(Operator.Value + " siiii");
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