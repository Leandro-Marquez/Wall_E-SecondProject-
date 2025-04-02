public class Token 
{
    public string Value {get ; set ;}
    public TokenType Type {get ;}
    public Token(TokenType type ,string value)
    {
        Value = value;
        Type = type;
    }
}
public enum TokenType{Identifier , Delimiter , ReservedKeyword , LogicOperator , AssignmentOperator , ArithmeticOperator , ComparisonOperator , String , Number , Comma , Bool}