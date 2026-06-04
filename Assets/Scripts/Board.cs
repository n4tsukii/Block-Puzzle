using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    
    private readonly Cell[,] cells = new Cell[Size, Size];
    private readonly int[,] data = new int[Size, Size]; // 0 empty, 1 hover, 2 normal
    private readonly List<Vector2Int> hoverPoints = new();
    private readonly List<int> fullLineCols = new();
    private readonly List<int> fullLineRows = new();
    private readonly List<int> highlightPolyominoCols = new();
    private readonly List<int> highlightPolyominoRows = new();


    void Start()
    {
        for (var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                cells[r, c] = Instantiate(cellPrefab, cellsTransform);
                cells[r, c].transform.position = new(c + 0.5f, r + 0.5f, 0.0f);
                cells[r, c].Hide();
            }
        }
    }
    public void Hover(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        UnHover();
        Unhighlight();

        highlightPolyominoCols.Clear();
        highlightPolyominoRows.Clear();

        HoverPoints(point, polyominoRows, polyominoCols, polyomino);
        
        if (hoverPoints.Count > 0)
        {
            Hover();
            Highlighter(point,polyominoCols, polyominoRows);
            
            foreach (var fullLineCol in fullLineCols)
            {
                highlightPolyominoCols.Add(fullLineCol - point.x);
            }
            foreach (var fullLineRow in fullLineRows)
            {
                highlightPolyominoRows.Add(fullLineRow - point.y);
            }
        }

    }

    private void Unhighlight()
    {
        UnHighlightFullLineCols();
        UnHighlightFullLineRows();
    }

    private void Highlighter(Vector2Int point, int polyominoCols, int polyominoRows)
    {
        PredictFullLineCols(point.x, point.x + polyominoCols);
        PredictFullLineRows(point.y, point.y + polyominoRows);

        HighlightFullLineCols();
        HighlightFullLineRows();
    }

    private void HighlightFullLineRows()
    {
        foreach(var r in fullLineRows)
        {
            for (var c = 0; c < Size; ++c)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Highlight();
                }
            }
        }
    }

    private void HighlightFullLineCols()
    {
        foreach(var c in fullLineCols)
        {
            for (var r = 0; r < Size; ++r)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Highlight();
                }
            }
        }
    }
    private void UnHighlightFullLineRows()
    {
        foreach(var r in fullLineRows)
        {
            for (var c = 0; c < Size; ++c)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }

    private void UnHighlightFullLineCols()
    {
        foreach(var c in fullLineCols)
        {
            for (var r = 0; r < Size; ++r)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }

    private void PredictFullLineRows(int y, int v)
    {
        fullLineRows.Clear();
        for (var r = y; r < v; ++r)
        {
            var isFullLine = true;
            for (var c = 0; c < Size; ++c)
            {
                if (data[r, c] != 1 && data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine)
            {
                fullLineRows.Add(r);
            }
            
        }        
    }

    private void PredictFullLineCols(int fromCol, int toColExclusive)
    {
        fullLineCols.Clear();
        for (var c = fromCol; c < toColExclusive; ++c)
        {
            var isFullLine = true;
            for (var r = 0; r < Size; ++r)
            {
                if (data[r, c] != 1 && data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine)
            {
                fullLineCols.Add(c);
            }
            
        }
    }

    private void HoverPoints(Vector2Int point, int polyominoRows, int polyominoCols, int[,] polyomino)
    {
        for (var r = 0; r < polyominoRows; ++r)
        {
            for (var c = 0; c < polyominoCols; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    var hoverPoint = point + new Vector2Int(c, r);
                    if (!isValidPoint(hoverPoint))
                    {
                        hoverPoints.Clear();
                        return;
                    }
                    hoverPoints.Add(hoverPoint);
                }
            }
        }
    }

    private bool isValidPoint(Vector2Int point)
    {
        if (point.x < 0 || Size <= point.x) return false;
        if (point.y < 0 || Size <= point.y) return false;
        if (data[point.y, point.x] > 1) return false;
        return true;
    }

    private void Hover()
    {
        foreach (var point in hoverPoints)
        {
            data[point.y, point.x] = 1;
            cells[point.y, point.x].Hover();
        }
    }
    private void UnHover()
    {
        foreach (var point in hoverPoints)
        {
            data[point.y, point.x] = 0;
            cells[point.y, point.x].Hide();
        }
        hoverPoints.Clear();
    }

    public bool Place(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        UnHover();
        HoverPoints(point, polyominoRows, polyominoCols, polyomino);
        if (hoverPoints.Count > 0)
        {
            Place(point, polyominoCols, polyominoRows);
            return true;
        }
        return false;
    }

    private void Place(Vector2Int point, int polyominoCols, int polyominoRows)
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 2;
            cells[hoverPoint.y, hoverPoint.x].Normal();
        }
        ClearFullLines(point, polyominoCols, polyominoRows);
        hoverPoints.Clear();
    }

    private void ClearFullLines(Vector2Int point, int polyominoCols, int polyominoRows)
    {
        FullLineCols(point.x, point.x + polyominoCols);
        FullLineRows(point.y, point.y + polyominoRows);

        ClearFullLineCols();
        ClearFullLineRows();
    }

    private void ClearFullLineRows()
    {
        foreach(var r in fullLineRows)
        {
            for (var c = 0; c < Size; ++c)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }

    private void ClearFullLineCols()
    {
        foreach(var c in fullLineCols)
        {
            for (var r = 0; r < Size; ++r)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }

    private void FullLineCols(int fromCol, int toColExclusive)
    {
        fullLineCols.Clear();
        for (var c = fromCol; c < toColExclusive; ++c)
        {
            var isFullLine = true;
            for (var r = 0; r < Size; ++r)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine)
            {
                fullLineCols.Add(c);
            }
            
        }
    }


    public bool CheckPlace(int PolyominoIndex) {
        var polyomino = Polyominos.Get(PolyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        for (var r = 0; r <= Size - polyominoRows; ++r)
        {
            for (var c = 0; c <= Size - polyominoCols; ++c)
            {
                if (CheckPlace(c, r , polyominoCols, polyominoRows, polyomino) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckPlace(int cols, int rows, int polyominoCols, int polyominoRows, int[,] polyomino)
    {
        for (var r = 0; r < polyominoRows; ++r)
        {
            for (var c = 0; c < polyominoCols; ++c)
            {
                if (polyomino[r, c] > 0 && data[rows + r, cols + c] == 2)
                {
                    var point = new Vector2Int(cols + c, rows + r);
                    return false;
                }
            }
        }
        return true;
    }

    private void FullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        for (var r = fromRow; r < toRowExclusive; ++r)
        {
            var isFullLine = true;
            for (var c = 0; c < Size; ++c)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine)
            {
                fullLineRows.Add(r);
            }
            
        }        
    }
    public List<int> HighlightPolyominoCols => highlightPolyominoCols;
    public List<int> HighlightPolyominoRows => highlightPolyominoRows;
}
