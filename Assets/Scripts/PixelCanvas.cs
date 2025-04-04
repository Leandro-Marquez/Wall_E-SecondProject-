using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCanvas : MonoBehaviour
{
    public int gridSize = 256; // 256x256 celdas
    public GameObject cellPrefab; // Prefab de cada celda (con Image)
    private GameObject[,] grid; // Matriz para almacenar celdas

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        float cellSize = 1080f / gridSize; // Tamaño de cada celda

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Instanciar celda
                GameObject cell = Instantiate(cellPrefab, transform);
                RectTransform rt = cell.GetComponent<RectTransform>();
                
                // Posición y tamaño
                rt.anchoredPosition = new Vector2(x * cellSize, y * cellSize);
                rt.sizeDelta = new Vector2(cellSize, cellSize);
                
                // Guardar referencia
                grid[x, y] = cell;
            }
        }
    }

    // Obtener celda por coordenadas
    public GameObject GetCell(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            return grid[x, y];
        else
        {
            Debug.LogError($"Coordenadas inválidas: ({x}, {y})");
            return null;
        }
    }
}
