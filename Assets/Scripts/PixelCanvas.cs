using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicPixelGrid : MonoBehaviour
{
    public int gridSize = 256;
    public float cellSize = 2f; // Tamaño inicial de cada celda
    public Color defaultColor = Color.white;

    private Image[,] cells;
    private GridLayoutGroup gridLayout;

    void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = Vector2.one * cellSize;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gridSize;

        cells = new Image[gridSize, gridSize];

        // Crear celdas
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject cellObj = new GameObject($"Cell_{i}_{j}");
                cellObj.transform.SetParent(transform, false);
                
                Image img = cellObj.AddComponent<Image>();
                img.color = defaultColor;
                cells[i, j] = img;
            }
        }
    }

    public void SetCellColor(int x, int y, Color color)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            cells[x, y].color = color;
    }

    public void UpdateCellSize(float newSize)
    {
        cellSize = newSize;
        gridLayout.cellSize = Vector2.one * cellSize;
    }
}