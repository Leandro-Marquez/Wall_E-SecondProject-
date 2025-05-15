public class Token //clase token para poder guardar organizadamente todos los pedazos de codigo afiliados a un mismo tipo
{
    public string Value {get ; set ;} //valor del token
    public TokenType Type {get ;} //tipo de token
    public Token(TokenType type ,string value) //constructor de la clase 
    {
        //iniciarlizar el Token con su valor y su tipo
        Value = value;
        Type = type;
    }
}

//enum para tener los tipos de TOKENS mas organizados y su facil manejo 
public enum TokenType{Identifier , Delimiter , ReservedKeyword , LogicOperator , AssignmentOperator , ArithmeticOperator , ComparisonOperator , String , Number , Comma , Bool , LineJump}