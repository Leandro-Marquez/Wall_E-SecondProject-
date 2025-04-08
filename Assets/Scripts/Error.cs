public class Error
{
    public ErrorType errorType{ get; set; }
    public string message{ get; set; }
    public Error(ErrorType errorType, string message)
    {
        this.errorType = errorType;
        this.message = message;
    }
}
public enum ErrorType {Syntax_Error , Semantic_Error , RunTime_Error};