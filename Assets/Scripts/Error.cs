using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

public class Error : MonoBehaviour//clase para el manejo de errores
{
    public TextMeshProUGUI errorsVisual;//referencia de texto en la escena para mostrar errores
    private static int count = 0;//contador de errores actuales
    public static List<(ErrorType,string)> errors = new List<(ErrorType, string)>();//lista de errores TipoDeError_____ERROR

    public void Update()//verificar en cada frame
    {
        if(errors.Count != count)//si hay nuevos errores
        {
            count += 1;//actualizar contador
            errorsVisual.text += errors[errors.Count-1].Item1.ToString() + " : " + errors[errors.Count-1].Item2.ToString();//añadir error al texto en la escena 
        }
    }
}

public enum ErrorType{Syntax_Error , Semantic_Error , Run_Time_Error}//enum para llevar los tipos de errores posibles