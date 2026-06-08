using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
    public const int Size = 5;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board board;
    [SerializeField] private Blocks blocks;
    private int polyominoIndex;
    private readonly Cell[,] cells = new Cell[Size, Size];
    private Vector3 previousMousePosition = Vector3.positiveInfinity;
    private Vector2Int previousDragPoint, currentDragPoint;
    private SortingGroup sortingGroup;
    private Vector3 position, scale;
    private Vector2 inputPoint, center;
    private readonly Vector3 inputOffset = new Vector3 (0.0f, 2.0f, 0.0f);
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        sortingGroup = GetComponent<SortingGroup>();
    }
    public void Initialize()
    {
        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c] = Instantiate(cellPrefab, transform);
            }
        }
        position = transform.localPosition;
        scale = transform.localScale;
    }
    public void Show(int polyominoIndex)
    {
        this.polyominoIndex = polyominoIndex;
        Hide();
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominosRows = polyomino.GetLength(0);
        var polyominosCols = polyomino.GetLength(1);
        center = new Vector2(polyominosCols*0.5f, polyominosRows*0.5f);

        for (var r = 0; r < polyominosRows; ++r)
        {
            for (var c = 0; c < polyominosCols; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    cells[r,c].transform.localPosition = new(c - center.x + 0.5f,r - center.y + 0.5f,0.0f);
                    cells[r,c].Normal();
                }
            }
        }
    }
    private void Hide()
    {
        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c].Hide();
            }
        }
    }

    private void OnMouseDown()
    {
        inputPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.localPosition = position + inputOffset;
        transform.localScale = Vector3.one;

        blocks.ResetBlockSortingOrder();
        SetSortingOrder(1);

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);

        board.Hover(currentDragPoint, polyominoIndex);
        Highlight(board.HighlightPolyominoCols, board.HighlightPolyominoRows);

        previousDragPoint = currentDragPoint;
        previousMousePosition = Input.mousePosition;
    }
    private void OnMouseDrag()
    {
        var currentMousePostion = Input.mousePosition;
        if (currentMousePostion != previousMousePosition)
        {
            // Debug.Log("Mouse drag");
            previousMousePosition = currentMousePostion;

            var inputDelta = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - inputPoint;
            transform.localPosition = position + inputOffset + (Vector3)inputDelta;

            currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);
            if (currentDragPoint != previousDragPoint)
            {
                previousDragPoint = currentDragPoint;
                board.Hover(currentDragPoint, polyominoIndex);
                Highlight(board.HighlightPolyominoCols, board.HighlightPolyominoRows);
            }
        }
    }
    private void OnMouseUp()
    { 
        previousMousePosition = Vector3.positiveInfinity;

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position - center);
        if (board.Place(currentDragPoint, polyominoIndex))
        {
            gameObject.SetActive(false);
            blocks.Remove();
        }

        transform.localPosition = position;
        transform.localScale = scale;
    }
    private void Highlight(List<int> cols, List<int> rows)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        Unhighlight(polyominoCols, polyominoRows, polyomino);

        HighlightCols(polyominoRows, polyomino, cols);
        HighlightRows(polyominoCols, polyomino, rows);

    }

    private void HighlightRows(int polyominoCols, int[,] polyomino, List<int> rows)
    {
        foreach (var r in rows)
        {
            for (var c = 0; c < polyominoCols; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    cells[r, c].Highlight();
                }
            }
            
        }
    }


    private void HighlightCols(int polyominoRows, int[,] polyomino, List<int> cols)
    {
        foreach (var c in cols)
        {
            for (var r = 0; r < polyominoRows; ++r)
            {
                if (polyomino[r, c] > 0)
                {
                    cells[r, c].Highlight();
                }
            }
        }
    }


    private void Unhighlight(int polyominoCols, int polyominoRows, int[,] polyomino)
    {
        for (var r = 0; r < polyominoRows; ++r)
        {
            for (var c = 0; c < polyominoCols; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }
    public void SetSortingOrder(int order)
    {
        sortingGroup.sortingOrder = order;
    }
}
