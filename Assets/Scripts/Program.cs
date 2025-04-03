// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;

class Program 
{
              
    public static void Main(string[] args)
    {
        string example = @"Spawn(3 + 2 * 3 / 6 ** 3 - 1 % 4 , 0)
                           Color(""Black"")
                           n <- 5
                           k <- 3 + 3 ** 10 / 3 % 4 - 7
                           n <- k * 2
                           
                           actual-x <- GetActualX()
                           i <- 0
                           
                           loop-1
                           DrawLine(1, 0, 1)
                           i <- i + 1
                           is-brush-color-blue <- IsBrushColor(""Blue"")
                           GoTo [loop-ends-here] (is-brush-color-blue == 1)
                           GoTo [loop-1] (i < 10)
                           
                           Color(""Blue"")
                           GoTo [loop-1] (false)
                           
                           loop-ends-here";

        Lexer prueba = new Lexer(example);
        prueba.Tokenize();
        List<Token> tokens = prueba.tokens;
        for (int i = 0; i < tokens.Count ; i++)
        {
            System.Console.Write(tokens[i].Type + " : " + tokens[i].Value);
            System.Console.WriteLine();
        }
        // System.Console.WriteLine("==================================================================");
        // Parser parser = new Parser(tokens);
        // parser.Parse();
    }
}
