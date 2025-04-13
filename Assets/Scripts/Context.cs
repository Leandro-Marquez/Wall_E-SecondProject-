using System.Collections.Generic;

class Context
{
    public static Dictionary<string , ASTNode > variablesValues = new Dictionary<string , ASTNode >();
    public static Dictionary<string , int > labels = new Dictionary<string,int> ();
    public static (int x , int y) wallEPosition;
    public static string brushColor = "Transparent";
    public static int pincelZize = 1;
    public static string [,] canvas;
    public static int canvasSize;






}