using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelCanvasController : MonoBehaviour
{
    [SerializeField] private int targetResolution = 1080;  // configuración de la resolución objetivo para el canvas 
    public static int grid;  // tamaño de la grilla (público estático para acceso externo)

    public static PixelCanvasController instance; // instancia estática para acceso global a los métodos

    // variables internas para manejar la textura y renderizado
    private Texture2D texture; //...
    private SpriteRenderer spriteRenderer;//...
    private float pixelsPerUnit;//...
    public static Parser parser;  // referencia estatica a Parser para evaluar nodos
    private void Awake()
    {
        // implementación del patrón singleton
        if (instance == null)
        {
            instance = this;
            // persiste entre escenas
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject); //eliminar duplicados 
    }

    public static bool gotoBoolean; // bandera para control de flujo (goto)

    void Start()
    {
        gotoBoolean = false;
        grid = Cover.canvasSize; // obtiene el tamaño del grid a partir del dato entrado en escena 
        InitializeCanvas();

        while(Context.indexOfEvaluation < parser.aSTNodes.Count && Error.errors.Count == 0) // evaluar todos los nodos AST en orden a partir del indice de evaluacion del contexto 
        {
            parser.aSTNodes[Context.indexOfEvaluation].Evaluate(true); //evaluacion
            Context.indexOfEvaluation += 1; // incrementa el índice a menos que haya un goto
        }
        if(Error.errors.Count != 0)
        {
            SceneManager.LoadScene(0);
            string aux = "";
            for (var i = 0; i < Error.errors.Count ; i++)
            {
                aux += "~~";
                aux += Error.errors[i].Item1.ToString();
                aux += " : ";
                aux += Error.errors[i].Item2;
            }
            Cover.errors = aux;
        }
    }

    void InitializeCanvas()// inicializar el canvas con la configuración adecuada
    {
        // calcular la relación píxeles/unidad
        pixelsPerUnit = targetResolution / (float)grid;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // crear una nueva textura con configuración pixel-perfect
        texture = new Texture2D(grid, grid, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        ClearCanvas(); //rellenar el canvas de color blanco
        AdjustCamera(); //ajustar la camara a modo ortografico 
    }

    void AdjustCamera() // ajustar la cámara ortográfica para mostrar todo el canvas
    {
        if (Camera.main == null) return;//si no hay onjeto de camara retornar 

        Camera.main.orthographic = true; //activar la propiedad ortografica de la camara para la perspectiva 

        Camera.main.orthographicSize = grid / (2f * pixelsPerUnit);// calcula el tamaño ortográfico basado en el grid
    }

    public void ClearCanvas() // limpiar el canvas -> pintaro del todo de blanco 
    {
        Color[] pixels = new Color[grid * grid];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white; //a cada pixel en un primer momento le corresponde el color blanco 
        
        texture.SetPixels(pixels); // a traves de la textura establecer el color del canvas
        texture.Apply(); //aplicar la textura en la escena -> en el canvas

        spriteRenderer.sprite = Sprite.Create(texture,new Rect(0, 0, grid, grid),Vector2.one * 0.5f,pixelsPerUnit);// crear un nuevo sprite con la textura
    }

    public void SetPixel(int x, int y, Color color) // establecer el color de un píxel específico
    {
        if (x >= 0 && x < grid && y >= 0 && y < grid) // verificar límites antes de dibujar
        {
            texture.SetPixel(x, grid - 1 - y, color); //cambiarle el color a el pixel especifico a nivel de codigo 
            texture.Apply(); //aplicar el canbio en el canvas 
        }
    }

    public Color GetPixel(int x, int y) // obtener el color de un píxel específico
    {
        if (x >= 0 && x < grid && y >= 0 && y < grid) return texture.GetPixel(x, grid - 1 - y); //devolver el color del pixel objetivo en RGB en caso de ser valido la posicion 
        else return Color.clear; //devolver transparente si esta fuera de los limites, igual se puede devolver un null
    }
}