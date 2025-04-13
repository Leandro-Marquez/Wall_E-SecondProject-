using System.Collections.Generic;

public class Error
{
    public static List<(ErrorType,string)> errors = new List<(ErrorType, string)>();
}
public enum ErrorType{Syntax_Error , Semantic_Error , Run_Time_Error}