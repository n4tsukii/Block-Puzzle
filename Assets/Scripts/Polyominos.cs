using UnityEngine;

public class Polyominos : MonoBehaviour
{
    private static readonly int[][,] polyominos = new int[][,]
    {
        new int[,]
        {
            {0, 0, 1},
            {0, 0, 1},
            {1, 1, 1}
        },
        new int[,]
        {
            {1, 0, 0},
            {1, 0, 0},
            {1, 1, 1}
        },
        new int[,]
        {
            {1, 1, 1},
            {1, 0, 0},
            {1, 0, 0}
        },
        new int[,]
        {
            {1, 1, 1},
            {0, 0, 1},
            {0, 0, 1}
        },
        new int[,]
        {
            {1, 1},
            {1, 1}
        },
        new int[,]
        {
            {1, 1, 1},
            {0, 1, 0},
            {0, 1, 0}
        },
        new int[,]
        {
            {1},
            {1},
            {1}
        },
        new int[,]
        {
            {1, 1, 1}
        },
        new int[,]
        {
            {0, 1, 1},
            {1, 1, 0}
        },
        new int[,]
        {
            {1, 0},
            {1, 0},
            {1, 1}
        },
        new int[,]
        {
            {1}
        },
        new int[,]
        {
            {1, 1}
        },
        new int[,]
        {
            {1},
            {1}
        },
        new int[,]
        {
            {1, 1},
            {1, 0}
        },
        new int[,]
        {
            {1, 1},
            {0, 1}
        },
        new int[,]
        {
            {1, 0},
            {1, 1}
        },
        new int[,]
        {
            {0, 1},
            {1, 1}
        },
        new int[,]
        {
            {1, 1, 1, 1}
        },
        new int[,]
        {
            {1},
            {1},
            {1},
            {1}
        },
        new int[,]
        {
            {1, 1, 1, 1, 1}
        },
        new int[,]
        {
            {1},
            {1},
            {1},
            {1},
            {1}
        },
        new int[,]
        {
            {1, 1, 1},
            {0, 1, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {1, 1, 1}
        },
        new int[,]
        {
            {1, 0},
            {1, 1},
            {1, 0}
        },
        new int[,]
        {
            {0, 1},
            {1, 1},
            {0, 1}
        },
        new int[,]
        {
            {1, 1, 0},
            {0, 1, 1}
        },
        new int[,]
        {
            {1, 0},
            {1, 1},
            {0, 1}
        },
        new int[,]
        {
            {0, 1},
            {1, 1},
            {1, 0}
        },
        new int[,]
        {
            {1, 1, 1},
            {1, 1, 1}
        },
        new int[,]
        {
            {1, 1},
            {1, 1},
            {1, 1}
        },
        new int[,]
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1}
        },
        new int[,]
        {
            {0, 1, 0},
            {1, 1, 1},
            {0, 1, 0}
        },
        new int[,]
        {
            {1, 1, 1},
            {1, 0, 1}
        },
        new int[,]
        {
            {1, 0, 1},
            {1, 1, 1}
        },
        new int[,]
        {
            {1, 1},
            {1, 0},
            {1, 1}
        },
        new int[,]
        {
            {1, 1},
            {0, 1},
            {1, 1}
        },
    };

    static Polyominos()
    {
        foreach (var polyomino in polyominos)
        {
            ReverseRows(polyomino);
        }
    }
    public static int[,] Get(int index) => polyominos[index];
    public static int Length => polyominos.Length;
    private static void ReverseRows (int[,] polyomino)
    {
        var PolyominoRows = polyomino.GetLength(0);
        var PolyominoCols = polyomino.GetLength(1);
        for (var r = 0; r < PolyominoRows/2; ++r)
        {
            var topRow = r;
            var bottomRow = PolyominoRows - 1 - r;
            for (var c = 0; c < PolyominoCols; ++c)
            {
                (polyomino[bottomRow, c], polyomino[topRow, c]) = (polyomino[topRow, c], polyomino[bottomRow, c]);
            }
        }
    }
}
