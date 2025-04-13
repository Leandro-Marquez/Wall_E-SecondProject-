using System.Collections.Generic;

class Context
{
    public static Dictionary<string , ASTNode > variablesValues = new Dictionary<string , ASTNode >();
    public static Dictionary<string , int > labels = new Dictionary<string,int> ();
    public static (int x , int y) wallEPosition;
    public static string brushColor;
    public static int pincelZize;
    public static string [,] canvas;
}