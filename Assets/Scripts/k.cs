using UnityEngine;
using System.IO;
using System.Windows.Forms; // Asegúrate de tener esto

public class WindowsFileExplorer : MonoBehaviour
{
    public string loadedFilePath { get; private set; }
    public string loadedFileContent { get; private set; }

    // Método para abrir el explorador y seleccionar un archivo
    public void OpenFileDialog()
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        
        // Configura el filtro de extensiones (ej: .txt, .pw)
        fileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Archivos PW (*.pw)|*.pw|Todos los archivos (*.*)|*.*";
        fileDialog.FilterIndex = 1; // Filtro predeterminado
        
        // Abre el diálogo
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            loadedFilePath = fileDialog.FileName;
            loadedFileContent = File.ReadAllText(loadedFilePath);
            
            Debug.Log($"Archivo cargado: {loadedFilePath}");
            Debug.Log($"Contenido:\n{loadedFileContent}");
            
            // Opcional: Cargar el contenido en un InputField de Unity
            // FindObjectOfType<TMP_InputField>().text = loadedFileContent;
        }
    }
}