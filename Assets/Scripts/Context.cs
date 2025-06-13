using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using DrawingColor = System.Drawing.Color;
using Unity.VisualScripting; // Alias para evitar conflicto

class Context
{
    //todos los metodos y propiedades de esta clase son estaticos para q sin importar la instancia el contexto permanezca intacto
    public static Dictionary<string , object > variablesValues = new Dictionary<string , object >(); //diccionario para guardar cada variable con su respectivo valor 
    public static List<VariableNode> variableNodes = new List<VariableNode>(); //lista de nodos variables 
    public static Dictionary<string , int > labels = new Dictionary<string,int> (); //diccionario donde se tendra la etiqueta y su indice para cuando se evalue llegar volver a donde se estaba evaluando 
    public static (int x , int y) wallEPosition; //posicion actual de Wall_E (X,Y)
    public static bool wallEPositionChanged = false;
    public static string brushColor = "Transparent"; //color actual de la brocha, iniciarlo con transparente por defecto 
    public static System.Drawing.Color brushColorColor = System.Drawing.Color.White;
    public static int pincelZize = 1; //ancho actual del pincel , iniciarlo con 1 por defecto
    public static int canvasSize; //tamano actual del canvas
    public  static int indexOfEvaluation = 0; //indice de evaluacion para con el GoTo
    public static void Paint(int x , int y) //metodo estatico para poder pintar a traves de cualquier instancia
    {
        //                                     <<<<<<<<<<<<<convertir color a de System.Color a RGB de UnityEngine.Color>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        PixelCanvasController.instance.SetPixel(x , y , ColorConverter.ToUnityColor(brushColorColor)); //pintar pixel especifico a traves del metodo de pintar 
    }

}