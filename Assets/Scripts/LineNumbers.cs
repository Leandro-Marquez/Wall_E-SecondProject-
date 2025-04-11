using TMPro;
using UnityEngine;

public class LineNumberSync : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI lineNumbersText;

    void Start()
    {
        inputField.onValueChanged.AddListener(UpdateLineNumbers);
        UpdateLineNumbers(inputField.text);
    }

    void Update()
    {
        // Sincroniza la posición vertical de los números con el texto
        lineNumbersText.rectTransform.anchoredPosition = new Vector2(
            lineNumbersText.rectTransform.anchoredPosition.x,
            inputField.textComponent.rectTransform.anchoredPosition.y
        );
    }

    void UpdateLineNumbers(string text)
    {
        int lineCount = text.Split('\n').Length;
        string numbers = "";

        for (int i = 1; i <= lineCount; i++)
            numbers += i + "\n";

        lineNumbersText.text = numbers;
    }
}