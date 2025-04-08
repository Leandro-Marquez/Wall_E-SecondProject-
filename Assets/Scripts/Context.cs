using System.Collections.Generic;

public class Context 
{
    public List<Error> errors{ get; set; }
    public Dictionary<string, object> variables{ get; set;}
    public Context(List<Error> errors)
    {
        this.errors = errors;
        variables = new Dictionary<string, object>();
    }
    public void PrintErrors()
    {
        foreach (var error in errors)
        {
            //implementarrrrr
        }
    }
    public static void AddError(Error error)
    {

    }
}