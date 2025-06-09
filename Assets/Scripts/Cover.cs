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
    public GameObject runButton,exportButton;//botones de acción Correr y Exportar
    public static string input;//referencia estatica al texto de entrada del usuario
    public TMP_InputField usersInput;//referencia al campo de entrada en la escena
    public AudioSource errorSound;//sonido de error para reproducir cuando no haya dimension alguna
    public GameObject editor;//referencia al panel del editor en la escena
    public GameObject errorsPanel;//referencia al panel de errores en la escena 
    public GameObject runAndImportButtons;//referencia al objeto q los contiene para q mientras no se haya introducido las dimensiones del canvas no se encienda nada en la escena 
    public GameObject backButton;//referencia al botón de retroceso de la escena de Wall_E a la escena del editor
    public GameObject canvasSizeInputEntireObject;//referencia al objeto para tamaño del canvas en la escena
    public TMP_InputField canvasSizeInputField;//referencia al campo de entrada para el tamaño del canvas en la escena
    public GameObject logWarningObject;//referencia al objeto q contiene el texto con la advertencia en la escena
    public static int canvasSize;//referencia estatica al tamaño actual del canvas

    public void Start()//en el primer momento inicializar el canvas con valor 0
    {
        canvasSize = 0;//valor por defecto
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

    public void OnRunButtonIsPressed()//al ejecutar
    {
        if(canvasSize == 0)//validar el tamaño
        {
            Error.errors.Add((ErrorType.Semantic_Error,"Any valid Canvas's Size must be positive "));
            return; //detener el flujo de ejecucion
        }
        else Context.canvasSize = canvasSize;//actualizar las dimensiones del canvas en el contexto
        PixelCanvasController.grid = canvasSize;//configurar el grid del canvas su respectiva clase
        
        input = usersInput.text; //obtener la input y actualizar el campo estatico para posterior procesamiento
        Lexer lexer = new Lexer(input); //crear una instancia de la clase lexer con la entrada actual
        lexer.Tokenize(); //tokenizar la entrada a traves de la instancia
        List<Token> tokens = lexer.tokens; //obtener la lista de tokens despues de tokenizar a travez de la lista de tokens 
 
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
    }
    
    void Update()
    {   
        if(usersInput is not null ) input = usersInput.text;//actualizar la input
        if(string.IsNullOrEmpty(input))//si no hay entrada 
        {
            runButton.SetActive(false);//descativar boton de correr
            exportButton.SetActive(false);//descativar boton de exportar
        }
        else //en caso contrario
        {
            runButton.SetActive(true);//activar boton de correr
            exportButton.SetActive(true);//activar boton de exportar
        }
    }
} 

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



// Spawn(0, 0)
// Color("Purple")
// n <- 5

// loop-1

// DrawLine(0, 1, 4)

// DrawLine(1, 0, 4)

// n <- n - 1

// GoTo[loop-1](n > 0)
// Color("Green")
// DrawCircle(1,1,15)


// Color("Yellow")
// Fill()
// Color("Red")
// DrawLine(1,1,40)
// Color("Purple")
// DrawCircle(0 , 0, 7)
// DrawLine( 0 - 1 , 0,12)
// Color("Green")
// DrawRectangle( 1, 1, 3, 7, 10)
 




// Spawn(128, 128)
// Color("Yellow")    
// Fill()         

// Color("Black")

// DrawLine(0 - 1, 1, 100)
// DrawLine(1, 1, 100) 
// DrawLine(1, 0, 200) 
// DrawLine(0 - 1, 0, 200)




// Spawn(0, 0)         
// Color("Blue")        
// DrawLine(1,0,100)
// DrawLine(0,1,100)
// DrawLine(0 - 1,0,100)
// DrawLine(0,0- 1,100)
// DrawLine(1,1,100)
// DrawLine(0 - 1,0,100)
// DrawLine(1,0 - 1,100)






// Spawn(31, 31)
// Color("Green")   
// Size(10)         
// DrawCircle(0, 0, 10)
// DrawLine(1,0,25)
// Color("Red")
// Fill()

// DrawLine(1,0 - 1, 200)   
// DrawCircle(0, 0, 30)   


// DrawLine(0 - 1, 1, 100)   
// Color("Black")
// Size(15)               


// DrawLine(1,0 - 1, 50)    
// DrawLine(1, 0, 200)     
// DrawLine(1, 1, 50)    




// Spawn(31,31)
// Color("Purple")
// Size(4)
// DrawLine(1 ,1,23)

// DrawRectangle( 0, 0, 0, 20, 25)





// Spawn(128, 128)
// Color("Yellow")
// Size(100)
// Fill()
// Color("Black")
// DrawCircle(0, 0, 100)
// Size(6)
// DrawLine(0 - 1,0 - 1,35)
// DrawCircle(0,0,6) 
// Color("Purple")
// Fill()
// DrawLine(1,0 - 1,70)
// DrawCircle(0,0,6)
// DrawLine(0,1,40)
// DrawLine(0 - 1,0,25)
// DrawLine(1,1,12)