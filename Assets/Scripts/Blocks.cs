using System;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    [SerializeField] private Board board;
    public int blockCount = 0;
    private int[] polyominoIndexes;
    void Start()
    {
        var blockWidth = (float)Board.Size/blocks.Length;
        var cellSize = (float)Board.Size/(Block.Size*blocks.Length + blocks.Length + 1);
        for (var i = 0; i < blocks.Length; ++i)
        {
            blocks[i].transform.localPosition = new(blockWidth*(i + 0.5f), -0.25f - cellSize*4.0f, 0.0f);
            blocks[i].transform.localScale = new(cellSize,cellSize,cellSize);
            blocks[i].Initialize();
        }

        polyominoIndexes = new int[blocks.Length];

        Generate();
    }
    private void Generate()
    {
        for (var i = 0; i < blocks.Length; ++i)
        {
            polyominoIndexes[i] = UnityEngine.Random.Range(0, Polyominos.Length);
            blocks[i].gameObject.SetActive(true);
            blocks[i].Show(polyominoIndexes[i]);
            ++blockCount;

        }
    }

    public void Remove()
    {
        --blockCount;
        if (blockCount <= 0)
        {
            blockCount = 0;
            Generate();
        }

        var lose = true;
        for (var i = 0; i < blocks.Length; ++i)
        {
            if (blocks[i].gameObject.activeSelf && board.CheckPlace(polyominoIndexes[i]))
            {
                lose = false;
                break;
            }
        }
        if (lose)
            Debug.Log("You lose!");
    }
    public void ResetBlockSortingOrder()
    {
        for (var i = 0; i < blocks.Length; ++i)
        {
            blocks[i].SetSortingOrder(0);
        }
    }
}
