﻿using UnityEngine;
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





 
// Spawn(100, 100)
// Color("Black")

// n <- 5
// k <- 3 + 3 * 10
// n <- k * 2

// actual-x <- GetActualX() + GetActualY()
// i <- 0
// loop-1
// DrawLine(1, 0, actual-x)
// DrawLine(0, 1, 12)
// Color("Red")
// Size(3)
// DrawRectangle(1, 0, 50, 80, 40)

// Fill()
// DrawLine(1, 1, GetColorCount("Red" , 5 , 4 , 5 , 4))
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
// Size(50)

// DrawLine(0, 0, 0) 
// DrawCircle(0 - 1,0 - 1, 30)

// DrawLine(0, 0, 0) 
// DrawCircle(1 , 0 - 1, 30)

// Color("Black")
// Size(20)
// DrawLine(0, 0, 0) 
// DrawLine(0 - 1, 1, 100)
// DrawLine(1, 1, 100) 
// DrawLine(1, 0, 200) 
// DrawLine(0 - 1, 0, 200)




// Spawn(100, 100);         
// Color("Blue");           
// Size(3);                 
// DrawRectangle(1, 0, 50, 80, 40);



// Spawn(540, 540)
// Color("Black")   
// Size(30)         


// DrawLine(0 - 1,0 - 1, 100) 
// DrawCircle(0, 0, 30)    

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
// DrawRectangle( 1, 1, 3, 12, 10)