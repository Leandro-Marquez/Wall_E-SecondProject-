using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;


public class Error : MonoBehaviour
{
    public TextMeshProUGUI errorsVisual;
    static int count = 0;
    public static List<(ErrorType,string)> errors = new List<(ErrorType, string)>();
    public void Update()
    {
        if(errors.Count != count)
        {
            count += 1;
            errorsVisual.text += errors[errors.Count-1].Item1.ToString() + " : " + errors[errors.Count-1].Item2.ToString();
            // Debug.Log("Se agregoooooo");
        }
    }

}
public enum ErrorType{Syntax_Error , Semantic_Error , Run_Time_Error}