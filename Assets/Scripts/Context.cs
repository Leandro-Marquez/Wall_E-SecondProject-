using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using DrawingColor = System.Drawing.Color; // Alias para evitar conflicto

class Context
{
    public static Dictionary<string , ASTNode > variablesValues = new Dictionary<string , ASTNode >(); //diccionario para guardar cada variable con su respectivo valor 
    public static Dictionary<string , int > labels = new Dictionary<string,int> (); //lista de tuplas donde se tendra la etiqueta y su indice para cuando se evalue llegar volver a donde se estaba evaluando 
    public static (int x , int y) wallEPosition; //posicion actual de Wall_E (X,Y)
    public static bool wallEPositionChanged = false;
    public static string brushColor = "Transparent"; //color actual de la brocha, iniciarlo con transparente por defecto 
    public static int pincelZize = 1; //ancho actual del pincel , iniciarlo con 1 por defecto
    public static int canvasSize; //tamano actual del canvas
    public static void Paint(int x , int y)
    {
        System.Drawing.Color color1 = System.Drawing.Color.White;
        switch (brushColor)
        {
            case "Red":
                color1 = System.Drawing.Color.Red;
                break;
            case "Blue":
                color1 = System.Drawing.Color.Blue;
                break;
            case "Green":
                color1 = System.Drawing.Color.Green;
                break;
            case "Yellow":
                color1 = System.Drawing.Color.Yellow;
                break;  
            case "Orange":
                color1 = System.Drawing.Color.Orange;
                break;
            case "Purple":
                color1 = System.Drawing.Color.Purple;
                break;
            case "Black":
                color1 = System.Drawing.Color.Black;
                break;
            case "White":
                color1 = System.Drawing.Color.White;
                break;
            case "Transparent":
                color1 = System.Drawing.Color.White;
                break;
        } 
        PixelCanvasController.instance.SetPixel(x , y , ColorConverter.ToUnityColor(color1));
        // Debug.Log($"X: {x} ,Y: {y} = {ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(x, y))}");
    }

}