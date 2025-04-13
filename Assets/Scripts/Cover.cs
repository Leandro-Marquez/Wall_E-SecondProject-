// See https://aka.ms/new-console-template for more information
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEditor;

class Cover : MonoBehaviour
{
    public GameObject runButton,exportButton;
    public static string input;
    public TMP_InputField usersInput; //inputField en la escena en Unity 
    public GameObject editor;
    public GameObject errorsPanel;
    public GameObject runAndImportButtons;
    public GameObject backButton;
    public GameObject canvasSizeInputEntireObject;
    public TMP_InputField canvasSizeInputField;
    public GameObject logWarningObject;
    private static int canvasSize;
    public void Start()
    {
        canvasSize = 0;
    }
    public void OnOKKButtonIsPressed()
    {
        string text = canvasSizeInputField.text;
        if(text == "")
        {
            //reproducir sonido de errorrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr
            return;
        }
        else canvasSize = int.Parse(text.ToString());
        logWarningObject.SetActive(true);
        canvasSizeInputEntireObject.SetActive(false);
    }
    public void OnAcceptButtonIsPressed()
    {
        logWarningObject.SetActive(false);
        editor.SetActive(true);
        errorsPanel.SetActive(true);
        runAndImportButtons.SetActive(true);
        backButton.SetActive(true);
    }
    public void OnCancelButtonIsPressed()
    {
        logWarningObject.SetActive(false);
        canvasSizeInputEntireObject.SetActive(true);
    }
    public void OnBackButtonIsPressed()
    {
        canvasSizeInputEntireObject.SetActive(true);
        editor.SetActive(false);
        errorsPanel.SetActive(false);
        runAndImportButtons.SetActive(false);
    }
    public void OnRunButtonIsPressed()
    {
        Lexer lexer = new Lexer(input);
        lexer.Tokenize();
        List<Token> tokens = lexer.tokens;
        //imprimirrrrrr
        // for (int i = 0; i < tokens.Count ; i++) Debug.Log(tokens[i].Type + " : " + tokens[i].Value);   
        
        Parser parser = new Parser(tokens);
        parser.Parse();
        
        if(parser.aSTNodes[0] is not FunctionNode) Error.errors.Add((ErrorType.Syntax_Error,"Any valid expression must begin with the Spawn(x,y) command"));
        for (int i = 0; i < parser.aSTNodes.Count; i++)
        {
            parser.aSTNodes[i].Evaluate();
            // Debug.Log(i);
            // parser.aSTNodes[i].Print();
        }
    }
    void Update()
    {
        if(usersInput is not null ) input = usersInput.text;
        if(string.IsNullOrEmpty(input)) runButton.SetActive(false);
        else
        {
            runButton.SetActive(true);
            exportButton.SetActive(true);
        }
    }
}




    // public static void Read()
    // {
    //     string rutaArchivo = @"D:\SCHOOL\PROJECTS FIRST YEAR\Proyecto Unity\WALL_E(Segundo_Proyecto_De_Programacion)\Wall-E_(Second_Project)\Assets\text.txt"; // Cambia esto por tu ruta
        
    //     string[] lineas = File.ReadAllLines(rutaArchivo); // Leer todas las líneas del archivo y guardarlas en un array

    //     List<Token> tokens = new List<Token>(); //lista de tokens por linea 
    //     Lexer test = null; //instancia para tokenizar a traves de ella cada linea

    //     for (int i = 0; i < lineas.Length ; i++)
    //     {
    //         if(string.IsNullOrEmpty(lineas[i])) continue; //si es nulo o vacio puede que sea una linea vacia, saltarla
    //         test = new Lexer(lineas[i]); //inicializar la instancia con la nueva linea 
    //         test.Tokenize(); //tokenizar la linea
    //         List<Token> tokensOfTest = test.tokens; //guardar los tokens de la linea actual 
    //         tokens.AddRange(tokensOfTest); //agregarlos a la lista de tokens final 
    //     }
    // }