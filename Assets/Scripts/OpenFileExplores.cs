using UnityEngine;
using System.IO;
using System.Windows.Forms;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class WindowsFileExplorer : MonoBehaviour
{
    public string loadedFilePath { get; private set; } //ruta del archivo seleccionado para importarlo 
    public string loadedFileContent { get; private set; } //cadena de texto del archivo cargado 

    public TMP_InputField textFileContent; //inputField en la escena en Unity 
    public void OpenFileDialog() // Método para abrir el explorador y seleccionar un archivo
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        
        // Configurar el filtro de extensiones (ej: .txt, .pw)
        fileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Archivos PW (*.pw)|*.pw|Todos los archivos (*.*)|*.*";
        fileDialog.FilterIndex = 1; // Filtro predeterminado
        
        if (fileDialog.ShowDialog() == DialogResult.OK) //abrir el cuadro de dialogo 
        {
            loadedFilePath = fileDialog.FileName;
            loadedFileContent = File.ReadAllText(loadedFilePath);

            textFileContent.text = loadedFileContent;
            Cover.input += textFileContent.text;
        }
    }
}