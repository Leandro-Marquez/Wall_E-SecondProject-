
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

class Assistant
{
    //metodo principal para evaluar la funcion spawn q solo inicializa a Wall_E en la posicion indicada
    public static object EvaluateSpawn(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar Spawn");
        //si no se tienen 2 paramentros hay un error en tiempo de ejecucion 
        if(Params.Count != 2)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Spawn()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;
        }
        else //en caso contrario 
        {
            //guardar los valores X y Y correspondientes
            var x = Params[0].Evaluate(false);// X
            var y = Params[1].Evaluate(false);// Y
            if(x is int xValue && y is int yValue) //si ambos son enteros no se tiene error, por lo q se ejecutara correctamente 
            {
                //si son positivos y menos q las dimensiones definidas del canvas no hay problema 
                if(xValue >= 0 && xValue < Context.canvasSize && yValue >= 0 && yValue < Context.canvasSize)
                {
                    Context.wallEPosition.x = (int)x;
                    Context.wallEPosition.y = (int)y;    
                    Context.wallEPositionChanged = true;
                }
                //en caso contrario se lanza un error en tiempo de ejecucion
                else Error.errors.Add((ErrorType.Run_Time_Error, "Spawn() parameters must be positive and less than Canvas'Size" + $" Error_Line : {Context.indexOfEvaluation}"));
            }
            else //si al menos uno no es entero
            {
                // Mensaje de error detallado
                string errorMsg = "Invalid types in 'Spawn': ";
                if (x is not int) errorMsg += $"X must be int, but got '{x?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (y is not int) errorMsg += $"Y must be int, but got '{y?.GetType().Name}'." + $" Error_Line : {Context.indexOfEvaluation}";
                Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
            }
        }
        return null;// retorna null ya q es un metodo void
    }
    //metodo principal para canbiarle el color al pincel de Wall_E
    public static object EvaluateColor(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar Color");    
        //si tiene mas o menos de un parametro se tiene un error en tiempo de ejecucion
        if(Params.Count != 1)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Color()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;
        }
        else //en caso contrario
        {
            var a = Params[0].Evaluate(false);//guardar el valor del parametro
            System.Drawing.Color color; //crear un color auxiliar
            //si no es una cadena de texto hay un error en tiempo de ejecucion 
            if(a is not string aValue)
            {
                Error.errors.Add((ErrorType.Run_Time_Error, $"Color's Method must receive Int's Type and recibe {a.GetType()}" + $" Error_Line : {Context.indexOfEvaluation}"));
                return null;
            }
            else //en caso contrario verificar q color es el q se pide para cambiar el color 
            {
                switch (a) //switch case con cada color
                {
                    case "Red":
                        Context.brushColor = a.ToString();//cambiar el color en el contexto         
                        color = System.Drawing.Color.Red; //cambiarlo a drawing                      
                        Context.brushColorColor = color;  //y finalmente cambiarlo en el contexto
                        break;
                    case "Blue":
                        Context.brushColor = a.ToString();//cambiar el color en el contexto      
                        color = System.Drawing.Color.Blue;//cambiarlo a drawing                  
                        Context.brushColorColor = color;  //y finalmente cambiarlo en el contexto
                        break;
                    case "Green":
                        Context.brushColor = a.ToString(); //cambiar el color en el contexto      
                        color = System.Drawing.Color.Green;//cambiarlo a drawing                  
                        Context.brushColorColor = color;   //y finalmente cambiarlo en el contexto
                        break;
                    case "Yellow":
                        Context.brushColor = a.ToString();  //cambiar el color en el contexto      
                        color = System.Drawing.Color.Yellow;//cambiarlo a drawing                  
                        Context.brushColorColor = color;    //y finalmente cambiarlo en el contexto
                        break;
                    case "Orange":
                        Context.brushColor = a.ToString();  //cambiar el color en el contexto      
                        color = System.Drawing.Color.Orange;//cambiarlo a drawing                  
                        Context.brushColorColor = color;    //y finalmente cambiarlo en el contexto
                        break;
                    case "Purple":
                        Context.brushColor = a.ToString();  //cambiar el color en el contexto       
                        color = System.Drawing.Color.Purple;//cambiarlo a drawing                  
                        Context.brushColorColor = color;    //y finalmente cambiarlo en el contexto
                        break;
                    case "Black":
                        Context.brushColor = a.ToString(); //cambiar el color en el contexto      
                        color = System.Drawing.Color.Black;//cambiarlo a drawing                  
                        Context.brushColorColor = color;   //y finalmente cambiarlo en el contexto
                        break;
                    case "White":
                        Context.brushColor = a.ToString(); //cambiar el color en el contexto      
                        color = System.Drawing.Color.White;//cambiarlo a drawing                  
                        Context.brushColorColor = color;   //y finalmente cambiarlo en el contexto
                        break;
                    case "Transparent":
                        Context.brushColor = a.ToString(); //cambiar el color en el contexto      
                        color = System.Drawing.Color.White;//cambiarlo a drawing                  
                        Context.brushColorColor = color;   //y finalmente cambiarlo en el contexto
                        break;
                    default: //en otro caso se tiene un error en tiempo de ejecucion, color invalido
                        Error.errors.Add((ErrorType.Run_Time_Error, $"{a} is not a valid Color Type" + $" Error_Line : {Context.indexOfEvaluation}"));
                        break;
                }
            }
        }
        return null; //retorna null ya q es un metodo void 
    }

    //metodo principal para pedir las dimensiones del canvas
    public static object EvaluateGetCanvasZize(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar GetCanvasZize");
        //si se tienen parametros se tiene un error en tiempo de ejecucion
        if(Params.Count > 0)
        {
            Error.errors.Add((ErrorType.Run_Time_Error , "GetCanvasZize() does not contains parameters" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null; //retornar null ya q hubo error
        }
        else return Context.canvasSize; //en cualquier otro caso retornar las dimensiones originales del canvas 
    }
    //metodo princpal para pedir la X de la posicion de Wall_E 
    public static object EvaluateGetActualX(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar GetActualX");
        //si se tienen parametros se tiene un error en tiempo de ejecucion
        if(Params.Count > 0)
        {
            Error.errors.Add((ErrorType.Run_Time_Error, "GetActualX() does not contains parameters" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else return Context.wallEPosition.x;//en cualquier otro caso retornar la posicion X de Wall_E
    }
    //metodo princpal para pedir la Y de la posicion de Wall_E 
    public static object EvaluateGetActualY(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar GetActualY");
        //si se tienen parametros se tiene un error en tiempo de ejecucion
        if(Params.Count > 0)
        {
            Error.errors.Add((ErrorType.Run_Time_Error, "GetActualY() does not contains parameters" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else return Context.wallEPosition.y;//en cualquier otro caso retornar la posicion Y de Wall_E
    }
    //metodo principal para pintar una linea en el canvas
    public static object EvaluateDrawLine(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar DrawLine");
        //si se tienen menos o mas de 3 paraemetros se tiene un error en tiempo de ejecucion
        if (Params.Count != 3)
        {
            Error.errors.Add((ErrorType.Run_Time_Error, "There is no argument given that corresponds to the required parameter of DrawLine()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        //lista de enteros donde se tendran los parametros
        List<int> ints = new List<int>();
        int check = Error.errors.Count;
        for (var i = 0; i < Params.Count; i++)
        {
            var a = Params[i].Evaluate(false); //guardar el parametro
            // Debug.Log(a.GetType());
            //chequear si es entero sino se tiene un error en tiempo de ejecucion
            if (a is not int && a is not null)
            {
                Error.errors.Add((ErrorType.Run_Time_Error, $"DrawLine's Method must receive Int's Type and recibe {a.GetType()}" + $" Error_Line : {Context.indexOfEvaluation}"));
                check += 1;//aumentar cantidad de errores para posterior uso 
                continue;//continuar la revision de los parametros aunque este no sea valido
            }
            else if (a is not null) ints.Add((int)a); //en caso contrario, osea correcto, agregar el valor del parametro
        } 
        if(check != Error.errors.Count) return null;//si se tuvieron errores de tipo detener la ejecucion
        (int dx, int dy) direction = (ints[0], ints[1]);//guardar en una tupla la direccion que se tiene
        int distances = ints[2]; // Cantidad de píxeles a pintar en la dirección establecida
        int brushSizes = Context.pincelZize; // Grosor del pincel
        // Validar que el grosor y la distancia sean positivos si se tiene grosor o distancia invalidos se tiene un error en tiempo de ejecucion
        if (distances <= 0 || brushSizes <= 0)
        {
            Error.errors.Add((ErrorType.Run_Time_Error, "Distance and brush size must be positive integers" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        int halfBrushs = brushSizes / 2;
        // Pintar cada píxel en la dirección dada, con el grosor del pincel
        for (int step = 0; step < distances; step++)
        {
            // Calcular la nueva posición en la dirección dada
            int newX = Context.wallEPosition.x + direction.dx;
            int newY = Context.wallEPosition.y + direction.dy;
            // Verificar límites del canvas
            if (newX < 0 || newY < 0 || newX >= Context.canvasSize || newY >= Context.canvasSize) break; // Si se sale del canvas, terminar
            // Actualizar posición del robot
            Context.wallEPosition.x = newX;
            Context.wallEPosition.y = newY;
            // Pintar un área cuadrada alrededor del punto central (según brushSize)
            for (int offsetX = -halfBrushs; offsetX <= halfBrushs; offsetX++)
            {
                for (int offsetY = -halfBrushs; offsetY <= halfBrushs; offsetY++)
                {
                    int paintX = Context.wallEPosition.x + offsetX;
                    int paintY = Context.wallEPosition.y + offsetY;
                    // Verificar que el píxel a pintar esté dentro del canvas
                    if (paintX >= 0 && paintY >= 0 && paintX < Context.canvasSize && paintY < Context.canvasSize) Context.Paint(paintX, paintY);//pintar el pixel especificado
                }
            }
        }
        return null; //retornar null ya q es un metodo void 
    }
    //metodo principal para cambiar el ancho del picnel
    public static object EvaluateSize(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar Size");
        //si se tiene mas o menos de unn paraemtro se tiene un error en tiempo de ejecucion
        if(Params.Count != 1)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of Size()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            var a = Params[0].Evaluate(false); //guardar el valor del parametro
            if(a is int) //si es entero
            {
                if((int)a == 0)
                {
                    Error.errors.Add((ErrorType.Run_Time_Error, "BrushSize's must be a positive integer" + $" Error_Line : {Context.indexOfEvaluation}"));
                    return null;//retornar null ya q hubo error
                }
                int auxiliar = 0;
                if((int)a % 2 == 0) auxiliar = (int)a - 1; //si es par asignar el 
                else auxiliar = (int)a;
                Context.pincelZize = auxiliar;  //cambiar el ancho del pincel en el contexto
            }  
            //en caso contrario se tiene un error en tiempo de ejecucion
            else Error.errors.Add((ErrorType.Run_Time_Error,"Size's Method must recibe Int's Type" + $" Error_Line : {Context.indexOfEvaluation}"));
        }
        return null; //retornar null ya q es un metodo void

    }
    //metodo principal para chequear si es o no es el ancho de la brocha
    public static object EvaluateIsBrushSize(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar IsBrushSize");
        //si se tiene mas o menos de un parametro se tiene un error
        if(Params.Count != 1)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsBrushSize()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            var a = Params[0].Evaluate(false);//guardar el valor del parametro
            if(a is int) //si es entero
            {
                if(Context.pincelZize == (int)a) return 1; //si es el mismo ancho de brocha retornar 1
                else return 0; //en caso contrario retornar 0
            }  
            //si no es entero se tiene un error en tiempo de ejecucion
            else Error.errors.Add((ErrorType.Run_Time_Error,"IsBrushSize's Method must recibe Int's Type" + $" Error_Line : {Context.indexOfEvaluation}"));
        }
        return null;//si no retorno nada hasta aqui es porque hubo error, retornar null
    }
    //metodo principal para verificar si es o no es el color de la brocha
    public static object EvaluateIsBrushColor(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar IsBrushColor");
        bool ret = false;
        //si se tiene mas o menos de un parametro se tiene un error en tiempo de ejecucion
        if(Params.Count != 1)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsBrushColor()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            var a = Params[0].Evaluate(false); //guardar el valor del parametro
            if(a is string) //si es una cadena de texto
            {
                switch (a)
                {
                    case "Red":
                        ret = Context.brushColor == (string)a; //hacer el booleano igual a la verificacion del color
                        break;
                    case "Blue":
                        ret = Context.brushColor == (string)a; ///.
                        break;
                    case "Green":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "Yellow":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "Orange":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "Purple":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "Black":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "White":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    case "Transparent":
                        ret = Context.brushColor == (string)a;///.
                        break;
                    default: //en cualquier otro caso es q se tuvo un color invalido
                        Error.errors.Add((ErrorType.Run_Time_Error, $"{a} is not a valid Color Type" + $" Error_Line : {Context.indexOfEvaluation}"));
                        return null;//retornar null ya q hubo error
                }
            }
            //si no es una cadena de texto se tiene un error en tiempo de ejecucion 
            else Error.errors.Add((ErrorType.Run_Time_Error,"IsBrushSize's Method must recibe String's Type" + $" Error_Line : {Context.indexOfEvaluation}"));
        }
        if(ret) return 1; //si se tuvo coincidencia retornar 1
        else return 0; //en caso contrario retornar 0
    }
    //metodo principal para evaluar el metodo Fill (rellenar) 
    public static object EvaluateFill(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar Fill");
        //si se tiene algun paramtro se tiene un error en tiempo de ejecucion
        if(Params.Count != 0)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"Fill's Method does not contains params" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            //pedir el color q se tiene en el pixxel actual
            string color = ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(Context.wallEPosition.x,Context.wallEPosition.y));
            List<(int x , int y)> values = new List<(int x , int y)> (); //lista de tuplas osea posiciones q se van a tener durante el BFS
            values.Add((Context.wallEPosition.x,Context.wallEPosition.y)); //agregar la posicin actual de Wall_E como inicio del recorrido
         
            Context.Paint(Context.wallEPosition.x,Context.wallEPosition.y); //pintar la posicion actual del color de brocha actual
            for (var i = 0; i < values.Count ; i++) //mientras que se tengan valor en la lista
            {
                //arrays direccionales para el movimiento de Wall_E
                int [] dirx = { 1 , -1 , 0 ,  0 };
                int [] diry = { 0 ,  0 , 1 , -1 };
                for (var j = 0 ; j < dirx.Length ; j++)
                {
                    //nuevas coordenas de Wall_E
                    int newX = values[i].Item1 + dirx[j];
                    int newY = values[i].Item2 + diry[j];
                    //si las nuevas coordenas estan en rango, no se han visitado, y coincide con el color en el que estaba Wall_E al incio
                    if(newX >= 0 && newX < Context.canvasSize && newY >= 0 && newY < Context.canvasSize && color == ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(newX,newY)) && !values.Contains((newX,newY)))
                    {
                        values.Add((newX,newY));//agregar las nuevas coordenas para evitar recorrer celdas q ya se visitaron
                        Context.Paint(newX,newY); //pintar las nuevas coordenadas
                    }
                }
            }
        } 
        return null; //retornar null ya q es un metodo void
    }
    //metodo principal para verificar cuantas casillas tiene el color target en el rango marcado
    public static object EvaluateGetColorCount(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar GetColorCount");
        //si se tienen menos o mas de 6 parametros se tiene un error en tiempo de ejecucion
        if(Params.Count != 5)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of GetColorCount()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            //obtener el resultado de la evaluacion de cada parametro
            var color = Params[0].Evaluate(false);
            var x1 = Params[1].Evaluate(false);
            var y1 = Params[2].Evaluate(false);
            var x2 = Params[3].Evaluate(false);
            var y2 = Params[4].Evaluate(false);
            //verificar los tipos correspondientes para cada valor
            if(x1 is int x1Value && y1 is int y1Value && x2 is int x2Value && y2 is int y2Value && color is string colorValue)
            {
                //si son los tipos correctos, verificar q esten en rango
                if(x1Value >= 0 && x1Value < Context.canvasSize && y1Value >= 0 && y1Value < Context.canvasSize && x2Value >= 0 && x2Value < Context.canvasSize && y2Value >= 0 && y2Value < Context.canvasSize)
                {
                    bool validColor = false;
                    switch (color) //hacer switch con cada color para verificar si es un color 
                    {
                        case "Red":
                            validColor = true;
                            break;
                        case "Blue":
                            validColor = true;
                            break;
                        case "Green":
                            validColor = true;
                            break;
                        case "Yellow":
                            validColor = true;
                            break;
                        case "Orange":
                            validColor = true;
                            break;
                        case "Purple":
                            validColor = true;
                            break;
                        case "Black":
                            validColor = true;
                            break;
                        case "White":
                            validColor = true;
                            break;
                        case "Transparent":
                            validColor = true;
                            break;
                        default: //si se llega aqui es q no se tiene un color valido
                            Error.errors.Add((ErrorType.Run_Time_Error,$"{color} is not a valid Color Type" + $" Error_Line : {Context.indexOfEvaluation}"));
                            return null;//retornar null ya q hubo error
                    }
                    if(validColor) //si se tiene un color valido
                    {
                        int n = 0;//cantidad de casillas con el color objetivo
                        for (var i = (int)x1 ; i <= (int)x2 ; i++)//iterar por el rectangulo q definen los parametros
                        {
                            for (var j = (int)y1 ; j <= (int)y2 ; j++)
                            {
                                //si coincide el color aumentar el contador
                                if(ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel(i,j)) == (string)color) n += 1; 
                            }
                        }
                        return n; //retornar la cantidad de celdas q se encontraron 
                    }
                }
                //en caso de q no esten en rango se tiene un error en tiempo de ejecucion
                else Error.errors.Add((ErrorType.Run_Time_Error, "GetColorCount() parameters must be positive and less than Canvas'Size" + $" Error_Line : {Context.indexOfEvaluation}"));
            }
            else //en cualquier otro caso, se tiene un error de tipos
            {
                string errorMsg = "Invalid types in GetColorCount: ";
                if (x1 is not int) 
                    errorMsg += $"X1 must be int, but got '{x1?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (y1 is not int) 
                    errorMsg += $"Y1 must be int, but got '{y1?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (x2 is not int) 
                    errorMsg += $"X2 must be int, but got '{x2?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (y2 is not int) 
                    errorMsg += $"Y2 must be int, but got '{y2?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (color is not string) 
                    errorMsg += $"Color must be String, but got '{color?.GetType().Name}'." + $" Error_Line : {Context.indexOfEvaluation}";
                Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
            } 
        }
        return null;//si se llega aqui es por error, retornar null
    }
    //metodo principal para verificar si la celda objetivo tiene el color target
    public static object EvaluateIsCanvasColor(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar IsCanvasColor");
        //si se tienen mas o menos de 3 parametros se tiene un error en tiempo de ejecucion
        if(Params.Count != 3)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of IsCanvasColor()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            //obtener los valores de la evaluacion de los parametros
            var color = Params[0].Evaluate(false);
            var x = Params[1].Evaluate(false);
            var y = Params[2].Evaluate(false);
            //verificar los tipos de los paraemtros
            if(color is string colorValue && x is int xValue && y is int yValue)
            {
                //si son correctos verificar q esten en rango
                if((int)x >= 0 && (int)x < Context.canvasSize && (int)y >= 0 && (int)y < Context.canvasSize)
                {
                    //obtener el color de la celda obetivo
                    string colorAux = ColorConverter.ToDrawingColor(PixelCanvasController.instance.GetPixel((int) y ,(int) x));
                    if(colorAux == (string)color) return 1; //si coincide retornar 1
                    else return 0; //en caso de q no coincida retornar 0
                }
                //en caso contrario se tiene un error en tiempo de ejecucion
                else Error.errors.Add((ErrorType.Run_Time_Error,"IsCanvasColor() parameters [x,y] must be positive and less than Canvas'Size" + $" Error_Line : {Context.indexOfEvaluation}"));
            }
            else //en cualquier otro caso, se tiene un error de tipos
            {
                string errorMsg = "Invalid types in IsCanvasColor: ";
                if (x is not int) 
                    errorMsg += $"X must be int, but got '{x?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (y is not int) 
                    errorMsg += $"Y must be int, but got '{y?.GetType().Name}'." + $" Error_Line : {Context.indexOfEvaluation}";
                if (color is not string) 
                    errorMsg += $"Color must be string, but got '{color?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
            }
        }
        return null;//si se llega aqui es por error, retornar null
    }
    //metodo principal para pintar un circulo
    public static object EvaluateDrawCircle(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar DrawCircle");
        //si se tienen mas o menos de 3 parametros se tiene un error en tiempo de ejecucion
        if(Params.Count != 3)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of DrawCircle()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            int auxiliar = Context.pincelZize;//obtener el tama;o del pincel
            //obtener los valores de la evaluacion correspondiente de los parametros
            var x = Params[0].Evaluate(false);
            var y = Params[1].Evaluate(false);
            var radius = Params[2].Evaluate(false);
            //si se tienen los tipos correctos se procede a pintar
            if(x is int xValue && y is int yValue && radius is int radiusValue && (int)radius > 0)
            { 
                int radio = (int)radius;//guardar el valor del radio para no tener q castear mucho
                // obetener la posicion de Wall-E
                int xc = Context.wallEPosition.x;
                int yc = Context.wallEPosition.y;
                //verificar si son valores de direccion validos
                if(((int)x == 1 || (int)x == -1 || (int)x == 0 ) && ((int)y == 1 || (int)y == -1 ||(int)y == 0))
                {
                    //verificar si moviendo a Wall_E esa cantidad de casillas en esa direccion cae dentro del canvas
                    if(radio * (int)x +  xc >= 0 && radio * (int)y + yc >= 0 && radio * (int)x +  xc < Context.canvasSize && radio * (int)y + yc < Context.canvasSize)
                    {
                        xc += radio * (int)x;//mover la posicion en x
                        yc += radio * (int)y;//mover la posicion en y
                    }
                    else //en caso contrario se tiene un error en tiempo de ejecucion
                    {
                        Error.errors.Add((ErrorType.Run_Time_Error , $"Wall_E position would be out of range" + $" Error_Line : {Context.indexOfEvaluation}"));
                        return null;//se tuvo un error retornar desde aqui
                    }
                }

                while(auxiliar >= 1) //pintar el ancho del pincel hacia arriba para mantener el radio correctamente
                {

                    int xPos = 0;
                    int yPos = (int)radio;
                    int d = 3 - 2 * (int)radio;
                    // Dibujar los puntos iniciales
                    DrawCirclePoints(xc, yc, xPos, yPos);
                    while (yPos >= xPos)
                    {
                        xPos++;
                        // Aplicar el algoritmo del punto medio
                        if (d > 0)
                        {
                            yPos--;
                            d = d + 4 * (xPos - yPos) + 10;
                        }
                        else d = d + 4 * xPos + 6;
                        DrawCirclePoints(xc, yc, xPos, yPos);
                    }
                    // Mover Wall_E al centro del círculo
                    Context.wallEPosition.x = xc;
                    Context.wallEPosition.y = yc;
                    auxiliar -= 1;
                    radio += 1;
                }
            } 
            else //en caso contrario se tiene un error de tipos
            {
                string errorMsg = "Invalid types in DrawCircle: ";
                if (x is not int) 
                    errorMsg += $"dirX must be int, but got '{x?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (y is not int) 
                    errorMsg += $"dirY must be int, but got '{y?.GetType().Name}'." + $" Error_Line : {Context.indexOfEvaluation}";
                if (radius is not int) 
                    errorMsg += $"radius must be int, but got '{radius?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (radius is int && (int)radius < 0)
                    errorMsg += "radius must be positive and less than Canvas's Size" + $" Error_Line : {Context.indexOfEvaluation}";
                Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
            }
            
        }
        return null; //retornar null ya que es un metodo void
    }
    //metodo auxiliar para dibujar los puntos criticos el circulo
    private static void DrawCirclePoints(int xc, int yc, int x, int y)
    {
        // Dibujar los 8 puntos simétricos del círculo
        Context.Paint(xc + x, yc + y);
        Context.Paint(xc - x, yc + y);
        Context.Paint(xc + x, yc - y);
        Context.Paint(xc - x, yc - y);
        Context.Paint(xc + y, yc + x);
        Context.Paint(xc - y, yc + x);
        Context.Paint(xc + y, yc - x);
        Context.Paint(xc - y, yc - x);
    }
    //metodo principal para pintar el rectangulo
    public static object EvaluateDrawRectangle(List<ASTNode> Params)
    {
        Debug.Log("Se llamo a evaluar DrawRectangle");
        //si se tienen mas o menos de 5 parametros se tiene un error en tiempo de ejecucion
        if(Params.Count != 5)
        {
            Error.errors.Add((ErrorType.Run_Time_Error,"There is no argument given that corresponds to the required parameter of DrawRectangule()" + $" Error_Line : {Context.indexOfEvaluation}"));
            return null;//retornar null ya q hubo error
        }
        else //en caso contrario
        {
            //obtener los valores de la evaluacion de los parametros
            var dirX = Params[0].Evaluate(false);
            var dirY = Params[1].Evaluate(false);
            var distance = Params[2].Evaluate(false);
            var width = Params[3].Evaluate(false);
            var heigth = Params[4].Evaluate(false);
            //verificar q los 5 sean del tipo q se quiere 
            if(dirX is int dirXValue && dirY is int dirYValue && distance is int distanceValue && width is int widthValue && heigth is int heightValue && (int)distance >= 0 && (int)width > 0 && (int)heigth > 0 && 
            (((int)dirX == -1 || (int)dirX == 0 || (int)dirX == 1) && ((int)dirY == -1 || (int)dirY == 0 || (int)dirY == 1)))
            { 
                //Obtener la posición actual de Wall-E
                int actualX = Context.wallEPosition.x;
                int actualY = Context.wallEPosition.y;

                //Calcular centro del rectángulo
                int centerX = actualX + (int)dirX * (int)distance;
                int centerY = actualY + (int)dirY * (int)distance;
                        
                int auxiliar = Context.pincelZize;
                int widthh = (int)width;
                int heigthh = (int)heigth;
                while(auxiliar >= 1) //pintar el ancho del pincel hacia arriba para mantener el radio
                {
                    //Calcular bordes absolutos
                    int left = centerX - widthh / 2;
                    int right = left + widthh - 1;  // -1 para mantener dimensiones exactas
                    int top = centerY - heigthh / 2;
                    int bottom = top + heigthh - 1;

                    //Obtener tamaño del pincel
                    int brushSize = Context.pincelZize;
                    int halfBrush = brushSize / 2;

                    //Dibujar rectángulo
                    for (int x = left; x <= right; x++)
                    {
                        for (int y = top; y <= bottom; y++)
                        {
                            // Dibujar solo los bordes
                            bool isBorder = (x == left || x == right || y == top || y == bottom);

                            if (isBorder && x >= 0 && x < Context.canvasSize && y >= 0 && y < Context.canvasSize)
                            {
                                // Aplicar grosor del pincel
                                for (int bx = -halfBrush; bx <= halfBrush; bx++)
                                {
                                    for (int by = -halfBrush; by <= halfBrush; by++)
                                    {
                                        int px = x + bx;
                                        int py = y + by;
                                        //si esta dentro de los rangos, pintar el pixel
                                        if (px >= 0 && px < Context.canvasSize && py >= 0 && py < Context.canvasSize) Context.Paint(px, py);
                                    }
                                }
                            }
                        }
                    }

                    // Actualizar la posición de Wall-E al centro del rectángulo
                    Context.wallEPosition.x = centerX;
                    Context.wallEPosition.y = centerY;

                    auxiliar -= 1;
                    widthh += 1;
                    heigthh += 1;
                }
            }  
            else //en otro caso dar un mensaje de error detallado
            {
                string errorMsg = "Invalid types in DrawCircle: ";
                if (dirX is not int) 
                    errorMsg += $"dirX must be int, but got '{dirX?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if (dirX is int && (int)dirX != 1  && (int)dirX != -1  && (int)dirX != 0 ) 
                    errorMsg += $"any valid direction value must be 1,-1,0" + $" Error_Line : {Context.indexOfEvaluation}";
                if (dirY is not int) 
                    errorMsg += $"dirY must be int, but got '{dirY?.GetType().Name}'." + $" Error_Line : {Context.indexOfEvaluation}";
                if (dirY is int && (int)dirY != 1  && (int)dirY != -1  && (int)dirY != 0 ) 
                    errorMsg += $"any valid direction value must be 1,-1,0" + $" Error_Line : {Context.indexOfEvaluation}";
                if (distance is not int) 
                    errorMsg += $"Distance must be int, but got '{distance?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if(distance is int && (int)distance < 0)
                    errorMsg += "Distance must be positive and less than Canvas's Size" + $" Error_Line : {Context.indexOfEvaluation}";
                if (width is not int) 
                    errorMsg += $"Width must be int, but got '{width?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if(width is int && (int)width < 0)
                    errorMsg += "Widht must be positive and less than Canvas's Size" + $" Error_Line : {Context.indexOfEvaluation}";
                if (heigth is not int) 
                    errorMsg += $"Height must be int, but got '{heigth?.GetType().Name}'. " + $" Error_Line : {Context.indexOfEvaluation}";
                if(heigth is int && (int)heigth < 0)
                    errorMsg += "Heigth must be positive and less than Canvas's Size" + $" Error_Line : {Context.indexOfEvaluation}";
                Error.errors.Add((ErrorType.Run_Time_Error, errorMsg));
            }
        }
        return null;//retornar null como es un metodo void
    }
    //metodo principal para evaluar operaciones binarias
    public static object EvaluateBinaryOperationNode(ASTNode LeftMember , Token Operator , ASTNode RightMember)
    {
        return Operator.Value switch//hacerle switch al operador porque cada operador se trabaja de una manera
        {
            //retornar directamente el operando izquierdo operado con el operando derecho en su forma particular, un metodo para cada operador 
            "+" => Add(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "-" => Subtract(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "*" => Multiply(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "/" => Divide(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "%" => Modulo(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "**" => Power(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "==" => AreEqual(LeftMember.Evaluate(false), RightMember.Evaluate(false),Operator),
            "!=" => !AreEqual(LeftMember.Evaluate(false), RightMember.Evaluate(false),Operator),
            ">=" => GreaterOrEqual(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            ">" => GreaterThan(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "<=" => LessOrEqual(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "<" => LessThan(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "||" => Or(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            "&&" => And(LeftMember.Evaluate(false), RightMember.Evaluate(false)),
            _ => throw new Exception($"Unknown operator: {Operator.Value}")
        };
    }
    // Métodos auxiliares para con las operaciones aritméticas 
    private static object Add(object a, object b) //metodo auxiliar para sumar
    {
        if (a is int aInt && b is int bInt) return aInt + bInt; //si ambos son enteros retornar la suma 
        if (a is string || b is string) return $"{a}{b}";//si ambos son strings retornar la concatenacion 
        
        Debug.Log(a.GetType() +  " yyyyyyyyyyyyyy " + b.GetType());
        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '+' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        Debug.Log("Se retornooo null");
        return null;//si se llega aqui es por error retornar null
    }

    private static object Subtract(object a, object b)//metodo auxiliar para restar
    {
        if (a is int aInt && b is int bInt) return aInt - bInt;//si ambos son enteros retornar la resta 
        
        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '-' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object Multiply(object a, object b)//metodo auxiliar para multiplicar
    {
        if (a is int aInt && b is int bInt) return aInt * bInt;//si ambos son enteros retornar el producto
        
        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '*' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object Divide(object a, object b)//metodo auxiliar para dividir
    {
        if (a is int aInt && b is int bInt) //si ambos son enteros
        {
            if (bInt == 0) //verificar si por quien se quiere dividir es por cero
            {
                Error.errors.Add((ErrorType.Run_Time_Error, "Division by zero" + $" Error_Line : {Context.indexOfEvaluation}"));
                return null;
            }
            return aInt / bInt;//en el otro caso retornar el cociente
        }

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '/' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object Modulo(object a, object b)//metodo auxiliar para operacion de modulo
    {
        if (a is int aInt && b is int bInt) return aInt % bInt;//si ambos son enteros retornar el modulo

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '%' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object Power(object a, object b)//metodo auxiliar para operacion de potencia
    {
        if (a is int aInt && b is int bInt) return (int)Math.Pow(aInt, bInt);//si ambos son enteros retornar la potencia
        if (a is int aI && b is double bD) return Math.Pow(aI, bD);//si ambos son enteros retornar la potencia

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '**' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object GreaterThan(object a, object b)//metodo auxiliar para operacion de >
    {
        if (a is int aInt && b is int bInt) return aInt > bInt;//si ambos son enteros retornar a > b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '>' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object GreaterOrEqual(object a, object b)//metodo auxiliar para operacion de >=
    {
        if (a is int aInt && b is int bInt) return aInt >= bInt;//si ambos son enteros retornar a >= b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '>=' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object LessThan(object a, object b)//metodo auxiliar para operacion de <
    {
        if (a is int aInt && b is int bInt) return aInt < bInt;//si ambos son enteros retornar a < b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '<' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object LessOrEqual(object a, object b)//metodo auxiliar para operacion de <=
    {
        if (a is int aInt && b is int bInt) return aInt <= bInt;//si ambos son enteros retornar a <= b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '<=' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    // Métodos auxiliares para con las operaciones lógicas
    private static object And(object a, object b)//metodo auxiliar para operacion de &&
    {
        if (a is bool aBool && b is bool bBool) return aBool && bBool;//si ambos son bool retornar a && b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error,  $"Operator '&&' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    private static object Or(object a, object b)//metodo auxiliar para operacion de ||
    {
        if (a is bool aBool && b is bool bBool) return aBool || bBool;//si ambos son bool retornar a || b

        //cualquier otro caso se tiene un error en tiempo de ejecucion
        Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '||' cannot be applied to operands of type {GetTypeName(a)} and {GetTypeName(b)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return null;//si se llega aqui es por error retornar null
    }

    // Método auxiliar para nombres de tipos más legibles
    private static string GetTypeName(object obj) 
    {
        if (obj == null) return "null"; //si es null retornar null
        return obj.GetType().Name; //retornar el nombre especifico del tipo 
    }
    private static bool AreEqual(object left, object right , Token Operator) //metodo auxiliar para verificar si se tiene los mismos tipos en los operandos
    {
        //si son tipos diferentes se tiene un error en tiempo de ejecucion 
        if (left.GetType() != right.GetType()) Error.errors.Add((ErrorType.Run_Time_Error, $"Operator '{Operator.Value}' cannot be applied to operands of type {GetTypeName(left)} and {GetTypeName(right)}" + $" Error_Line : {Context.indexOfEvaluation}"));
        return object.Equals(left, right); //en cualquier caso retornar si son o no sonn iguales
    }
}