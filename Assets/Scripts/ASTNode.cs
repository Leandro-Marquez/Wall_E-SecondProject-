using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public abstract class ASTNode //clase abstracta para tratar a todos los tipos de nodos como uno mismo
{
    public abstract void Print(string indent = "", bool last = true);
    public abstract object Evaluate(bool booleano);
}

public class FunctionNode : ASTNode //nodo de funcion 
{
    public string Name { get; set; } //nombre de la funcion 
    public List<ASTNode> Params { get; set; } //lista de parametros de la funcion , de tipo ASTNode porque puede ser de cualquier tipo

    public FunctionNode(string name, List<ASTNode> Params) //contructor de la clase
    {
        //inicializar los valores de la instacnia
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

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        //manejas los casos de funciones especiales
        string aux = this.Name;
        switch (aux) //hacer switch por cada tipo de funcion, y llamar al metodo correspondiente para cada una de ellas
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
        //si se sale del switch case es q no se tiene un metodo con el nombre, por lo q se tiene un error semantico en tiempo de ejecucion
        Error.errors.Add((ErrorType.Semantic_Error,$"There is not a definition for method {aux}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null; //si se llega aqui se llega por error, osea q no se tenga una funcion definida con el nombre q se puso retornar null
    }
}

public class VariableNode : ASTNode //nodo de variable
{
    public string Name { get; set; } //nombre de la variable
    public ASTNode Value { get; set; } //valor abstracto porque puede ser de cualquier tipo ASTNode

    public VariableNode(string name, ASTNode value)//constructor de la clase
    {
        //inicializar los valores de la instacnia
        Name = name;
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + $"Variable: {Name}");
        
        Value?.Print(indent + (last ? "   " : "│  "), true);
    }

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        //si se se llamo desde la iteracion principal de evaluacion, actualizar el valor de la variable en el contexto
        if(booleano) Context.variablesValues[Name] = Value.Evaluate(false);
        //en caso contrario retornar el valor de la variable en el contexto
        else return Context.variablesValues[Name];
        return null;//si se llega sera por error(extremo), retornar nul
    }
} 

public class NumberLiteralNode : ASTNode //clase para literal numerico
{
    public int Value { get; set; } //valor numerico

    public NumberLiteralNode(int value) //constructor de la clase 
    {
        //inicializar el valor de la isntancia
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + $"Number: {Value}");
    }

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        return Value; //retornar el valor del literal 
    }
}

public class StringLiteralNode : ASTNode //clase para literal de cadena de texto 
{
    public string Value { get; set; } //valor de cadena de texto

    public StringLiteralNode(string value) //constructor de la clase 
    {
        //inicializar el valor de la isntancia
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + $"String: \"{Value}\"");
    }

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        return Value;//retornar el valor del literal 
    }
}

public class BooleanLiteralNode : ASTNode//clase para literal booleano
{
    public bool Value { get; set; }//valor booleano

    public BooleanLiteralNode(bool value) //constructor de la clase 
    {
        //inicializar el valor de la isntancia
        Value = value;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + $"Boolean: {Value}");
    }

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        return Value;//retornar el valor del literal 
    }
}

public class LabelNode : ASTNode //nodo de etiqueta
{
    public string Label { get; set; } //nombre de la etiqueta

    public LabelNode(string label) //constructor de la clase
    {
        //inicializar el valor de la isntancia
        Label = label;
    }

    public override void Print(string indent = "", bool last = true)
    {
        Debug.Log(indent + (last ? "└──" : "├──") + $"Label: {Label}");
    }

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        return Label;//retornar la etiqueta, solo simbolico no se utiliza en ningun lugar
    }
}

public class GoToNode : ASTNode //nodo de GOTO
{
    public LabelNode Label { get; set; } //referencia a la etiqueta q se hace referencia
    public ASTNode Condition { get; set; }//condicion del GOTO, abstracta ya q puede ser cualquier tipo ASTNode

    public GoToNode(LabelNode label, BinaryOperationNode condition) //constructor de la clase 
    {
        //inicializar el valor de la isntancia
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

    public override object Evaluate(bool booleano)//implementacion del metodo de evaluacion del nodo
    {
        var condition = Condition.Evaluate(false); //obtener el valor de la evaluacion de la condicion 
        if(condition is bool) //si es de tipo bool es del tipo correcto
        {
            if((bool)condition) //si es true
            {
                if(!Context.labels.ContainsKey(Label.Label)) //verificar si la etiqueta no existe
                {
                    //en ese caso se tiene un error semantico ya q no se tiene dicha etiqueta en el codigo
                    Error.errors.Add((ErrorType.Semantic_Error,$"Current Label '{Label.Label}' is not found" + $" Error_Line : {Context.indexOfEvaluation}"));
                    return null; //detener el flujo de evaluacion 
                }
                //en el caso en q si se tenga, actualizar el indice de evaluacion con el indice de dicha etiqueta en el contexto 
                else Context.indexOfEvaluation = Context.labels[Label.Label];
            }
        }
        //sino se tiene un error de tipo en tiempo de ejecucion
        else Error.errors.Add((ErrorType.Run_Time_Error ,"GoTo's Condition must evaluate a Boolean Value" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//retornar null, ya q la evaluacion del GOTO es void
    } 
}

public class BinaryOperationNode : ASTNode //nodo de operacion binaria
{
    public Token Operator { get; set; } //operador 
    public ASTNode LeftMember { get; set; } //miembro izquierdo
    public ASTNode RightMember { get; set; } //miembro 

    public BinaryOperationNode(Token Operator, ASTNode leftMember, ASTNode rightMember) //constructor de la clase
    {
        //inicializar los valores de la instacnia
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

    public override object Evaluate(bool booleano) //implementacion del metodo de evaluacion del nodo
    {
        //llamar a su respectivo metodo en la clase ayudante
        return Assistant.EvaluateBinaryOperationNode(LeftMember, Operator, RightMember); 
    }

}