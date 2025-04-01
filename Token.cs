public class Token 
{
    public object Value {get ; set ;}
    public TokenType Type {get ;}
    public Token(TokenType type ,object value)
    {
        Value = value;
        Type = type;
    }
}
public enum TokenType{Identifier , Delimiter , ReservedKeyword , LogicOperator , AssignmentOperator , ArithmeticOperator , ComparisonOperator , String , Number , Comma , Bool}