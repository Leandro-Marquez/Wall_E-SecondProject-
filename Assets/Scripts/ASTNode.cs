using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public abstract class ASTNode //clase abstracta para tratar a todo tipo de nodo como uno mismo
{
    public abstract void Print();  
}

public class FunctionNode : ASTNode //nodo de funcion
{
    public string Name { get; set; } //nombre de la funcion 
    public List<ASTNode> Params { get; set; } //lista de parametros de la funcion
    public FunctionNode (string name, List<ASTNode> Params) //constructr de la clase 
    {
        Name = name;
        this.Params = Params;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class VariableNode : ASTNode //nodo de variable 
{
    public string Name {get; set;} //nombre de la variable
    public ASTNode Value { get; set;} //valor de la variable 
    public VariableNode(string name, ASTNode value) //constructor de la clase 
    {
        Name = name;
        Value = value;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class NumberLiteralNode : ASTNode //nodo literal numerico 
{
    public int Value { get; set; } //valor del nodo
    public NumberLiteralNode(int value) //constructor de la clase 
    {
        Value = value;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class StringLiteralNode : ASTNode //nodo literal de string
{
    public string Value { get; set; } //valor de la variable 
    public StringLiteralNode(string value) //constructor de la clase 
    {
        Value = value;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class BooleanLiteralNode : ASTNode //nodo literal booleano 
{
    public bool Value { get; set; } //valor del nodo
    public BooleanLiteralNode(bool value) //constructor de la clase 
    {
        Value = value;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class LabelNode : ASTNode //nodo de etiqueta 
{
    public string Label {get; set;} //nombre o valor de la etiqueta 
    public LabelNode(string label) //constructor de la clase 
    {
        Label = label;
    }
    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class GoToNode : ASTNode //nodo del GoTo
{
    public string Label { get; set; } //etiqueta a donde llevara la ejecucion
    public ConditionNode Condition { get; set; } //condicion que se debe cumplir para la llevar la ejecucion a la etiqueta 
    public GoToNode(string label, ConditionNode condition) //consrtructor de la clase 
    {
        Label = label;
        Condition = condition;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class ConditionNode : ASTNode //nodo de condicion
{
    public bool Value {get;} //valor final de la condicion 
    public Token Operator {get; set;} //operador, puede ser de comparacion o logico 
    public ASTNode LeftMember {get; set;} //miembro izquierdo 
    public ASTNode RightMember {get; set;} //miembro derecho 
    public ConditionNode(bool value, Token Operator , ASTNode leftMember, ASTNode rightMember) //constructor de la clase 
    {
        Value = value;
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class BinaryOperationNode : ASTNode //nodo de operacion binaria
{
    public Token Operator {get; set;} //operador de la operacion binaria 
    public ASTNode LeftMember { get; set;} //miembro izquierdo
    public ASTNode RightMember { get; set;} //miembro derecho
    public BinaryOperationNode(Token Operator, ASTNode leftMember, ASTNode rightMember) //constructor de la clase 
    {
        this.Operator = Operator;
        LeftMember = leftMember;
        RightMember = rightMember;
    }

    public override void Print()
    {
        throw new NotImplementedException();
    }
}