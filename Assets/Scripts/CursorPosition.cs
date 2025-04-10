using TMPro;
using UnityEngine;

public class TMPCursorPositionTracker : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public TextMeshProUGUI positionDisplayl; // Opcional
    public TextMeshProUGUI positionDisplayc; // Opcional
    void Update()
    {
        if (tmpInputField.isFocused)
        {
            int cursorPos = tmpInputField.caretPosition;
            string textBeforeCursor = tmpInputField.text.Substring(0, cursorPos);
            
            int lineNumber = textBeforeCursor.Split('\n').Length;
            int columnNumber = cursorPos - textBeforeCursor.LastIndexOf('\n') - 1;
            
            if (columnNumber < 0) columnNumber = cursorPos;
            
            if (positionDisplayl is not null && positionDisplayc is not null)
            {
                positionDisplayl.text = lineNumber.ToString();
                positionDisplayc.text = columnNumber.ToString();
            }
        }
    }
}