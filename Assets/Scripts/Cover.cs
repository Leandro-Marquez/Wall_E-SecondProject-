// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class Cover : MonoBehaviour
{
    public void Start()
    {
        Read();
    }
    
    public static void Read()
    {
        string rutaArchivo = @"D:\SCHOOL\PROJECTS FIRST YEAR\Proyecto Unity\WALL_E(Segundo_Proyecto_De_Programacion)\Wall-E_(Second_Project)\Assets\text.txt"; // Cambia esto por tu ruta
        
        string[] lineas = File.ReadAllLines(rutaArchivo); // Leer todas las líneas del archivo y guardarlas en un array

        List<Token> tokens = new List<Token>(); //lista de tokens por linea 
        Lexer test = null; //instancia para tokenizar a traves de ella cada linea

        for (int i = 0; i < lineas.Length ; i++)
        {
            if(string.IsNullOrEmpty(lineas[i])) continue; //si es nulo o vacio puede que sea una linea vacia, saltarla
            test = new Lexer(lineas[i]); //inicializar la instancia con la nueva linea 
            test.Tokenize(); //tokenizar la linea
            List<Token> tokensOfTest = test.tokens; //guardar los tokens de la linea actual 
            tokens.AddRange(tokensOfTest); //agregarlos a la lista de tokens final 
        }
        
        //imprimirrrrrr
        // for (int i = 0; i < tokens.Count ; i++) Debug.Log(tokens[i].Type + " : " + tokens[i].Value);   
        
        Parser parser = new Parser(tokens);
        parser.Parse();
        
    }
}