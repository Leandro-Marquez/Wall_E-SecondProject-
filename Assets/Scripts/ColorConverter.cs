using UnityEngine;
using System.Drawing;
using System.Collections.Generic;

public class ColorConverter
{
    // Convierte System.Drawing.Color -> UnityEngine.Color
    public static UnityEngine.Color ToUnityColor(System.Drawing.Color drawingColor) => new UnityEngine.Color(drawingColor.R / 255f,drawingColor.G / 255f,drawingColor.B / 255f,drawingColor.A / 255f);
}