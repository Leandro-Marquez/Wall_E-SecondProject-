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
    public GameObject runButton,exportButton;
    public static string input;
    public TMP_InputField usersInput;
    public AudioSource errorSound;
    public GameObject editor;
    public GameObject errorsPanel;
    public GameObject runAndImportButtons;
    public GameObject backButton;
    public GameObject canvasSizeInputEntireObject;
    public TMP_InputField canvasSizeInputField;
    public GameObject logWarningObject;
    public static int canvasSize;
    public void Start()
    {
        canvasSize = 0;
    }
    public void OnOKKButtonIsPressed()
    {
        string text = canvasSizeInputField.text;
        if(text == "")
        {
            errorSound.Play();
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
        if(canvasSize == 0)
        {
            Error.errors.Add((ErrorType.Semantic_Error,"Any valid Canvas's Size must be positive "));
            return;
        }
        else Context.canvasSize = canvasSize;
        PixelCanvasController.grid = canvasSize;
        input = usersInput.text;
        Lexer lexer = new Lexer(input);
        lexer.Tokenize();
        List<Token> tokens = lexer.tokens;
        //imprimirrrrrr
        // for (int i = 0; i < tokens.Count ; i++) Debug.Log(tokens[i].Type + " : " + tokens[i].Value);   
        
        Parser parser = new Parser(tokens);
        parser.Parse();
        
        if(parser.aSTNodes[0] is not FunctionNode) Error.errors.Add((ErrorType.Syntax_Error,"Any valid expression must begin with the Spawn(x,y) command"));
        
        if(Error.errors.Count == 0)
        {
            // Debug.Log("Siiii");
            SceneManager.LoadScene(1);
        }
        PixelCanvasController.parser = parser;
        // for (int i = 0; i < parser.aSTNodes.Count; i++)
        // {
        //     // parser.aSTNodes[i].Print();

        //     parser.aSTNodes[i].Evaluate();
        //     // Debug.Log(i);
        // }


        // Debug.Log("Brush Color : " + Context.brushColor);
        // Debug.Log("Pincel Zize : " + Context.pincelZize);
        // Debug.Log("Canvas Zize : " + Context.canvasSize);
        // Debug.Log("X: " + Context.wallEPosition.x + " , Y: " + Context.wallEPosition.y);
        
        // Debug.Log("======================================" + Context.variablesValues.Count);
        // foreach (var item in Context.variablesValues)
        // {
        //     Debug.Log(item.Key + ": " + item.Value.Evaluate());
        // } 
    }
    
    void Update()
    {   
        if(usersInput is not null ) input = usersInput.text;
        if(string.IsNullOrEmpty(input))
        {
            runButton.SetActive(false);
            exportButton.SetActive(false);
        }
        else
        {
            runButton.SetActive(true);
            exportButton.SetActive(true);
        }
    }
}






// Spawn(3 + 2 * 3 / 6 + 1 % 4 , 4)
// Color("Black")

// n <- 5
// k <- 3 + 3 * 10
// n <- k * 2

// actual-x <- GetActualX() + GetActualY()
// i <- 0
// loop-1
// DrawLine(1, 0, 12)
// DrawLine(0, 1, 12)
// DrawLine(1, 1, 12)
// Color("Red")
// Fill()