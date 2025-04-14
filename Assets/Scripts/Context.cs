using System.Collections.Generic;

class Context
{
    public static Dictionary<string , ASTNode > variablesValues = new Dictionary<string , ASTNode >(); //diccionario para guardar cada variable con su respectivo valor 
    public static Dictionary<string , int > labels = new Dictionary<string,int> (); //lista de tuplas donde se tendra la etiqueta y su indice para cuando se evalue llegar volver a donde se estaba evaluando 
    public static (int x , int y) wallEPosition; //posicion actual de Wall_E (X,Y)
    public static bool wallEPositionChanged = false;
    public static string brushColor = "Transparent"; //color actual de la brocha, iniciarlo con transparente por defecto 
    public static int pincelZize = 1; //ancho actual del pincel , iniciarlo con 1 por defecto
    public static int canvasSize; //tamano actual del canvas
}