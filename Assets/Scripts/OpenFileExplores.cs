using UnityEngine;
using System.IO;
using System.Windows.Forms;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using UnityEditor;

public class WindowsFileExplorer : MonoBehaviour
{
    public string loadedFilePath { get; private set; } //ruta del archivo seleccionado para importarlo 
    public string loadedFileContent { get; private set; } //cadena de texto del archivo cargado 

    public TMP_InputField textFileContent; //inputField en la escena en Unity 
    public void OpenFileDialog() //metodo para abrir el explorador y seleccionar un archivo
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        
        //configura el diálogo para mostrar solo archivos con extensiones específicas
        fileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Archivos PW (*.pw)|*.pw|Todos los archivos (*.*)|*.*";
        //establece el primer filtro como predeterminado
        fileDialog.FilterIndex = 1;
        
        if (fileDialog.ShowDialog() == DialogResult.OK) //abrir el cuadro de dialogo 
        {
            loadedFilePath = fileDialog.FileName;//almacenar la ruta completa del archivo seleccionado
            loadedFileContent = File.ReadAllText(loadedFilePath);//leer todo el contenido del archivo como texto

            textFileContent.text = loadedFileContent;//imprimir el contenido en el inputField de la UI
            Cover.input += textFileContent.text; //concatenar el contenido al input global
        }
    }
    public void SaveTextToFileExplorer()
    {
        if (textFileContent == null || string.IsNullOrEmpty(textFileContent.text))
        {
            Debug.LogError("¡No hay texto para guardar!");
            return;
        }

        // Abre el explorador de archivos para guardar
        string defaultFileName = "archivo.txt";
        string filePath = EditorUtility.SaveFilePanel(
            "Guardar como .txt",
            "", // Directorio inicial (vacío = predeterminado)
            defaultFileName,
            "txt");

        if (!string.IsNullOrEmpty(filePath))
        {
            // Guarda el texto en el archivo
            File.WriteAllText(filePath, textFileContent.text);
            Debug.Log("Archivo guardado en: " + filePath);
        }
    }
}