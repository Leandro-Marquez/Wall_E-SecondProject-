using UnityEngine;

public class PixelCanvasController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int gridHeight = 64; 
    [SerializeField] private int gridWidth = 64;  
    [SerializeField] private int targetResolution = 1080;

    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private float pixelsPerUnit;

    void Start()
    {
        InitializeCanvas();
        // DrawPerfectSmiley();
        // Paint();
    }

    void InitializeCanvas()
    {
        pixelsPerUnit = targetResolution / (float)gridHeight;
        spriteRenderer = GetComponent<SpriteRenderer>();

        texture = new Texture2D(gridWidth, gridHeight, TextureFormat.RGBA32, false)
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
        Camera.main.orthographicSize = gridHeight / (2f * pixelsPerUnit);
    }

    public void ClearCanvas()
    {
        Color[] pixels = new Color[gridWidth * gridHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        spriteRenderer.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, gridWidth, gridHeight),
            Vector2.one * 0.5f,
            pixelsPerUnit
        );
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < gridHeight && y >= 0 && y < gridWidth)
        {
            // Invertimos la fila (X) para que (0,0) esté arriba a la izquierda
            texture.SetPixel(y, gridHeight - 1 - x, color);
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