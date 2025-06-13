using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
class Cover : MonoBehaviour
{ 
    public static bool turnBack = false;
    public GameObject runButton,exportButton;//botones de acción Correr y Exportar
    public static string input;//referencia estatica al texto de entrada del usuario
    public static string errors;
    public TMP_InputField usersInput;//referencia al campo de entrada en la escena
    public TMP_InputField errorsInPanel;//referencia al campo de entrada en la escena
    public AudioSource errorSound;//sonido de error para reproducir cuando no haya dimension alguna
    public GameObject editor;//referencia al panel del editor en la escena
    public GameObject errorsPanel;//referencia al panel de errores en la escena 
    public GameObject runAndImportButtons;//referencia al objeto q los contiene para q mientras no se haya introducido las dimensiones del canvas no se encienda nada en la escena 
    public GameObject backButton;//referencia al botón de retroceso de la escena de Wall_E a la escena del editor
    public GameObject backToSceneButton;//referencia al botón de retroceso de la escena de Wall_E a la escena del editor
    public GameObject canvasSizeInputEntireObject;//referencia al objeto para tamaño del canvas en la escena
    public TMP_InputField canvasSizeInputField;//referencia al campo de entrada para el tamaño del canvas en la escena
    public GameObject logWarningObject;//referencia al objeto q contiene el texto con la advertencia en la escena
    public static int canvasSize;//referencia estatica al tamaño actual del canvas

    // Recuperar input al cargar la escena (en Start o Awake)
    void Start()
    {
        // Elimina el código de PlayerPrefs y usa directamente PixelCanvasController.usersInput
        if (!string.IsNullOrEmpty(PixelCanvasController.usersInput))
        {
            input = PixelCanvasController.usersInput;
            if (usersInput != null) usersInput.text = input;
        }
        else input = "";
        canvasSize = 0;
    }

    public void OnOKKButtonIsPressed()//al confirmar tamaño
    {
        string text = canvasSizeInputField.text;
        if(text == "")//si no hay texto
        {
            errorSound.Play();//reproducir sonido de error
            return; //parar la ejecucion del programa
        }
        else canvasSize = int.Parse(text.ToString());//asignar tamaño
        logWarningObject.SetActive(true);//mostrar advertencia
        canvasSizeInputEntireObject.SetActive(false);//ocultar el input en la escena
    }

    public void OnAcceptButtonIsPressed()//al aceptar advertencia
    {
        logWarningObject.SetActive(false);//descativar objeto de advertencia
        editor.SetActive(true);//activar editor
        errorsPanel.SetActive(true);//activar panel de errores
        runAndImportButtons.SetActive(true);//activar los botones
        backButton.SetActive(true);//activar el boton retroceso
    }

    public void OnCancelButtonIsPressed()//al cancelar advertencia
    {
        logWarningObject.SetActive(false); //apagar el objeto de advertencia
        canvasSizeInputEntireObject.SetActive(true);//volver a activar el input
    }

    public void OnBackButtonIsPressed()//al retroceder
    {
        canvasSizeInputEntireObject.SetActive(true);//volver a activar el input
        editor.SetActive(false);//ocultar editor
        errorsPanel.SetActive(false);//ocultar panel de errores
        runAndImportButtons.SetActive(false);//ocultar botones
    }
    public void OnBackButtonToSceneIsPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void OnRunButtonIsPressed()//al ejecutar
    {
        Error.errors = new List<(ErrorType, string)>();
        // Guarda el input en PixelCanvasController.usersInput (que persiste por ser estático y DontDestroyOnLoad)
        input = usersInput.text;
        PixelCanvasController.usersInput = input; // <- Aquí se guarda para persistir durante la ejecución
        
        if(canvasSize == 0)//validar el tamaño
        {
            Error.errors.Add((ErrorType.Semantic_Error,"Any valid Canvas's Size must be positive "));
            return; //detener el flujo de ejecucion
        } 
        else Context.canvasSize = canvasSize;//actualizar las dimensiones del canvas en el contexto
        PixelCanvasController.grid = canvasSize;//configurar el grid del canvas su respectiva clase
        
        input = usersInput.text; //obtener la input y actualizar el campo estatico para posterior procesamiento

        // Dividir el texto por saltos de línea
        string[] lines = input.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<Token> tokens = new List<Token>();
        for (var i = 0; i < lines.Length ; i++)
        { 
            Lexer aux = new Lexer(lines[i]);
            aux.Tokenize();
            tokens.AddRange(aux.tokens);
        } 

        Parser parser = new Parser(tokens);//crear una nueva instancia de la clase Parser 
        parser.Parse(); //parsear a traves de la instancia de la clase parser
        
        //si el primer nodo no es un nodo funcion y es la funcion Spawn, se tiene un error de sintaxis
        if(parser.aSTNodes[0] is not FunctionNode) Error.errors.Add((ErrorType.Syntax_Error,"Any valid expression must begin with the Spawn(x,y) command"));
        else if(parser.aSTNodes[0] is FunctionNode) //si es funcion 
        {
            var aux = parser.aSTNodes[0]; //guardar el nodo
            FunctionNode auxx = (FunctionNode)aux; //tratarlo como funcion
            if(auxx.Name != "Spawn") Error.errors.Add((ErrorType.Syntax_Error,"Any valid expression must begin with the Spawn(x,y) command")); //si no tiene nombre Spawn se tiene un error de sintaxis
        } 
        if(Error.errors.Count == 0)
        { 
            SceneManager.LoadScene(1);//cargar la escena si no hay errores                               
            PixelCanvasController.parser = parser;//asignar parser para posterior evaluacion de los nodos
        } 
        else
        {
            string aux = "";
            for (var i = 0; i < Error.errors.Count ; i++)
            {
                aux += "~~";
                aux += Error.errors[i].Item1.ToString();
                aux += " : ";
                aux += Error.errors[i].Item2;
            }
            errors = aux;
        }
    }
    
    void Update()
    {   
        if(turnBack)
        {
            backToSceneButton.SetActive(true);
            editor.SetActive(true);//activar editor
            // usersInput.text = input;
            // Debug.Log(input);
            // Debug.Log(usersInput.text);
            errorsPanel.SetActive(true);//activar panel de errores
            canvasSizeInputEntireObject.SetActive(false);//ocultar el input en la escena
            runAndImportButtons.SetActive(true);//activar los botones
            backButton.SetActive(true);//activar el boton retroceso
            runButton.SetActive(true);//activar boton de correr
            exportButton.SetActive(true);//activar boton de exportar
            turnBack = false;
            return;
        }
        if(usersInput is not null ) input = usersInput.text;//actualizar la input
        if(string.IsNullOrEmpty(usersInput.text))//si no hay entrada 
        {
            runButton.SetActive(false);//descativar boton de correr
            exportButton.SetActive(false);//descativar boton de exportar
        }
        else //en caso contrario
        {
            runButton.SetActive(true);//activar boton de correr
            exportButton.SetActive(true);//activar boton de exportar
            // if(turnBack) usersInput.text = input;
        }
        if(errors != errorsInPanel.text) errorsInPanel.text = errors;
    }
} 
// Spawn(0,0)
// Color("Black")
// DrawLine(0,1,50)
// Color("White")
// DrawLine(1,0 - 1,25)
// Color("Orange")
// DrawCircle(1,1,10)
// Color("Red")
// Fill()


// Spawn(0, 0)
// n <- 6
// Color("Blue")
// Leo
// DrawLine(1,0,n)
// DrawLine(0,1,n)
// n <- n - 1
// GoTo[Leo](n > 1)
// Color("Green")
// DrawLine(1,n,10)

// Spawn(0, 0)
// n <- 6
// Color("Blue")
// Leo
// DrawLine(1,0,n)
// DrawLine(0,1,n)
// n <- n - 1
// GoTo[Leo](n > 1)
// Color("Green")
// DrawLine(1,n,10 + true)

//  Spawn(0, 0)
// n <- 6 
// Color("Blue")
// Leo
// DrawLine(1,0,n)
// DrawLine(0,1,n)
// n <- n - 1
// GoTo[Leo](n > 1)
// Color("Green")
// DrawLine(1,n,10)