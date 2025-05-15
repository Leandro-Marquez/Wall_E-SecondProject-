using TMPro;
using UnityEngine;

public class TMPCursorPositionTracker : MonoBehaviour//clase para rastrear la posición del cursor
{
    public TMP_InputField tmpInputField;//referencia al campo de texto a monitorear en la escena
    public TextMeshProUGUI positionDisplayl; //referencia al texto q mostrara la fila en la escena
    public TextMeshProUGUI positionDisplayc; //referencia al texto q mostrara la columna en la escena
    
    void Update()//ejecutar en cada frame la posicion del cursor
    {
        if (tmpInputField.isFocused)//si el campo tiene foco
        {
            int cursorPos = tmpInputField.caretPosition;//obtener posición del cursor
            string textBeforeCursor = tmpInputField.text.Substring(0, cursorPos);//guardar texto antes del cursor
            
            int lineNumber = textBeforeCursor.Split('\n').Length;//calcular la línea actual
            int columnNumber = cursorPos - textBeforeCursor.LastIndexOf('\n') - 1;//calcular la columna actual
            
            if (columnNumber < 0) columnNumber = cursorPos;//ajustar para primera línea en casos negativosXXXXXXXXXXXXXXXXX
            
            if (positionDisplayl is not null && positionDisplayc is not null)//si hay displays asignados
            {
                positionDisplayl.text = lineNumber.ToString();//mostrar la línea en el texto asignado de la escena 
                positionDisplayc.text = columnNumber.ToString();//mostrar la columna en el texto asignado de la escena 
            }
        }
    }
}