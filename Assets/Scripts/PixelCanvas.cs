using Unity.VisualScripting;
using UnityEngine;

public class PixelCanvasController : MonoBehaviour
{
    // Configuración (serializada)
    [SerializeField] private int targetResolution = 1080;
    public static int grid; // Público estático para acceso externo

    public static PixelCanvasController instance;

    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private float pixelsPerUnit;
    public static Parser parser;

    private void Awake()
    {
        // Verificar si ya existe una instancia
        if (instance == null)
        {
            instance = this; // Asignar la instancia
            DontDestroyOnLoad(gameObject); //no destruir en nuevas escenas
        }
        else Destroy(gameObject); // Destruir el duplicado
    }
    public static bool gotoBoolean = false;
    void Start()
    {
        grid = Cover.canvasSize;
        InitializeCanvas();
        while(Context.indexOfEvaluation < parser.aSTNodes.Count)
        {
            // parser.aSTNodes[Context.indexOfEvaluation].Print();
            parser.aSTNodes[Context.indexOfEvaluation].Evaluate();
            if(!gotoBoolean)Context.indexOfEvaluation += 1;
        }
        // Debug.Log(parser.aSTNodes[parser.aSTNodes.Count-1].Evaluate() + " siiiii");
        // Debug.Log(Context.brushColor);
    } 

    void InitializeCanvas()
    {
        pixelsPerUnit = targetResolution / (float)grid;
        spriteRenderer = GetComponent<SpriteRenderer>();

        texture = new Texture2D(grid, grid, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        ClearCanvas();
        AdjustCamera();
    }

    void AdjustCamera()
    {
        if (Camera.main == null) return;

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = grid / (2f * pixelsPerUnit);
    }

    public void ClearCanvas()
    {
        Color[] pixels = new Color[grid * grid];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        spriteRenderer.sprite = Sprite.Create(texture,new Rect(0, 0, grid, grid),Vector2.one * 0.5f,pixelsPerUnit);
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < grid && y >= 0 && y < grid)
        {
            // (0,0) = esquina superior izquierda
            texture.SetPixel(x, grid - 1 - y, color);  // Invierte Y para que el (0,0) esté arriba
            texture.Apply();
        }
    }

    public Color GetPixel(int x, int y)
    {
        if (x >= 0 && x < grid && y >= 0 && y < grid) return texture.GetPixel(x, grid - 1 - y);
        else return Color.clear; // O puedes usar: throw new System.IndexOutOfRangeException();
    }
}