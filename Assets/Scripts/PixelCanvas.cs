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
    void Start()
    {
        grid = Cover.canvasSize;
        InitializeCanvas();
        for (int i = 0; i < parser.aSTNodes.Count; i++)
        {
            // parser.aSTNodes[i].Print();

            parser.aSTNodes[i].Evaluate();
            // Debug.Log(i);
        }

        // DrawPerfectSmiley();
        // Paint();
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

    void DrawPerfectSmiley()
    {
        Color faceColor = Color.yellow;
        Color eyeColor = Color.black;
        Color mouthColor = Color.black;

        // Cara (centro en X=32 filas, Y=32 columnas)
        for (int fila = 10; fila < 54; fila++) // Filas (X)
        {
            for (int col = 10; col < 54; col++) // Columnas (Y)
            {
                if (Mathf.Pow(fila - 32, 2) + Mathf.Pow(col - 32, 2) <= 22 * 22)
                {
                    SetPixel(fila, col, faceColor);
                }
            }
        }

        // Ojos (X=20 filas, Y=20 y 44 columnas)
        DrawCircle(20, 20, 4, eyeColor);  // Ojo izquierdo
        DrawCircle(20, 44, 4, eyeColor);  // Ojo derecho

        // Boca (X=40 filas, curva en columnas)
        for (int col = 16; col < 48; col++)
        {
            int fila = (int)(32 + 8 * Mathf.Sin((col - 16) * Mathf.PI / 32));
            SetPixel(fila, col, mouthColor);
            SetPixel(fila + 1, col, mouthColor);
        }
    }

    void DrawCircle(int centerX, int centerY, int radius, Color color)
    {
        for (int fila = centerX - radius; fila <= centerX + radius; fila++)
        {
            for (int col = centerY - radius; col <= centerY + radius; col++)
            {
                if (Mathf.Pow(fila - centerX, 2) + Mathf.Pow(col - centerY, 2) <= radius * radius)
                {
                    SetPixel(fila, col, color);
                }
            }
        }
    }

    // public void Paint()
    // {
    //     // Ejemplo de uso con el sistema corregido:
    //     SetPixel(0, 0, Color.red);       // Esquina SUPERIOR IZQUIERDA
    //     SetPixel(0, 63, Color.blue);     // Esquina SUPERIOR DERECHA
    //     SetPixel(0, 2, Color.blue);     // Esquina SUPERIOR DERECHA
    //     SetPixel(0, 4, Color.blue);     // Esquina SUPERIOR DERECHA

    //     // SetPixel(63, 0, Color.green);    // Esquina INFERIOR IZQUIERDA
    //     // SetPixel(63, 63, Color.yellow);  // Esquina INFERIOR DERECHA
    // }
}