using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using DrawingColor = System.Drawing.Color;
using Unity.VisualScripting; // Alias para evitar conflicto

class Context
{
    public static Dictionary<string , ASTNode > variablesValues = new Dictionary<string , ASTNode >(); //diccionario para guardar cada variable con su respectivo valor 
    public static Dictionary<string , int > labels = new Dictionary<string,int> (); //lista de tuplas donde se tendra la etiqueta y su indice para cuando se evalue llegar volver a donde se estaba evaluando 
    public static (int x , int y) wallEPosition; //posicion actual de Wall_E (X,Y)
    public static bool wallEPositionChanged = false;
    public static string brushColor = "Transparent"; //color actual de la brocha, iniciarlo con transparente por defecto 
    public static System.Drawing.Color brushColorColor = System.Drawing.Color.White;
    public static int pincelZize = 1; //ancho actual del pincel , iniciarlo con 1 por defecto
    public static int canvasSize; //tamano actual del canvas
    public static void Paint(int x , int y)
    {
        // System.Drawing.Color color1 = System.Drawing.Color.White;

        PixelCanvasController.instance.SetPixel(x , y , ColorConverter.ToUnityColor(brushColorColor));
        // Debug.Log($"X: {x} ,Y: {y} = {ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(x, y))}");
    }

}