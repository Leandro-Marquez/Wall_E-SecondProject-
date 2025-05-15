using UnityEngine;
using System.Drawing;
using System.Collections.Generic;

public class ColorConverter
{
    // convertir de System.Drawing.Color a UnityEngine.Color
    public static UnityEngine.Color ToUnityColor(System.Drawing.Color drawingColor) => new UnityEngine.Color(drawingColor.R / 255f,drawingColor.G / 255f,drawingColor.B / 255f,drawingColor.A / 255f);
    // convertir de UnityEngine.Color a System.Drawing.Color
    public static string ToDrawingColor(UnityEngine.Color unityColor) => GetColorName(System.Drawing.Color.FromArgb((int)(unityColor.a * 255), (int)(unityColor.r * 255),(int)(unityColor.g * 255),(int)(unityColor.b * 255)));
    
    public static string GetColorName(System.Drawing.Color color)
    {
        // Comparar con colores conocidos -> ToArgb para simplificar
        if (color.ToArgb() == System.Drawing.Color.Red.ToArgb()) return "Red";
        if (color.ToArgb() == System.Drawing.Color.Blue.ToArgb()) return "Blue";
        if (color.ToArgb() == System.Drawing.Color.Green.ToArgb()) return "Green";
        if (color.ToArgb() == System.Drawing.Color.Yellow.ToArgb()) return "Yellow";
        if (color.ToArgb() == System.Drawing.Color.Orange.ToArgb()) return "Orange";
        if (color.ToArgb() == System.Drawing.Color.Purple.ToArgb()) return "Purple";
        if (color.ToArgb() == System.Drawing.Color.Black.ToArgb()) return "Black";
        if (color.ToArgb() == System.Drawing.Color.White.ToArgb()) return "White";
        if (color.A == 0) return "Transparent"; // Cualquier color con Alpha=0 es transparente

        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"; // Si no coincide con ninguno, devolver el código ARGB en formato hexadecimal
    }
}