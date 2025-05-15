using TMPro;//importa el namespace para TextMeshPro
using UnityEngine;//importa el namespace básico de Unity

public class LineNumberSync : MonoBehaviour//clase para sincronizar números de línea
{
    public TMP_InputField inputField;//referencia al campo de entrada de texto en la escena 
    public TextMeshProUGUI lineNumbersText;//referencia al texto para mostrar números en la escena 

    void Start()//inicialización al comenzar
    {
        inputField.onValueChanged.AddListener(UpdateLineNumbers);//añadir listener para cambios
        UpdateLineNumbers(inputField.text);//actualizar números inicialmente
    }

    void Update()//ejecutar en cada frame
    {
        //sincronizar las posiciones de los numeros en cada frame
        lineNumbersText.rectTransform.anchoredPosition = new Vector2(lineNumbersText.rectTransform.anchoredPosition.x,inputField.textComponent.rectTransform.anchoredPosition.y);
    }

    void UpdateLineNumbers(string text)//actualizar los números de línea
    {
        int lineCount = text.Split('\n').Length;//contar líneas dividiendo por saltos
        string numbers = "";//cadena para almacenar números

        for (int i = 1; i <= lineCount; i++)//iterar desde 1 hasta el total de líneas
            numbers += i + "\n";//concatenar número y salto de línea

        lineNumbersText.text = numbers;//asignar los números al texto en la escena
    }
}