using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int width;
    public int height;
    private GameObject[,] board;
    private int currentBoxCount;
    private int currentBedCount; //Bed tiles, not number of beds
    private int currentWindowCount;
    private int currentWallCount;
    public GameObject BoxPrefab;
    private System.Random rng = new System.Random();
    private int xOffset;
    private int yOffset;

    void Awake()
    {
        board = new GameObject[width, height];
        xOffset = width / 2;
        yOffset = height / 2;
    }

    void Start()
    {
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            int x = (int)wall.transform.position.x + xOffset;
            int y = (int)wall.transform.position.y + yOffset;
            board[x, y] = wall;
            currentWallCount++;
        }

        var boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in boxes)
        {
            int x = (int)box.transform.position.x + xOffset;
            int y = (int)box.transform.position.y + yOffset;
            board[x, y] = box;
            currentBoxCount++;
        }

        var windows = GameObject.FindGameObjectsWithTag("Window");
        foreach (var window in windows)
        {
            int x = (int)window.transform.position.x + xOffset;
            int y = (int)window.transform.position.y + yOffset;
            board[x, y] = window;
            currentWindowCount++;
        }

        GameObject bed = GameObject.FindGameObjectWithTag("Bed");
        board[xOffset - 1, yOffset + 3] = board[xOffset - 1, yOffset + 4] = bed;
        currentBedCount += 2;

        GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
        int xr = (int)respawn.transform.position.x + xOffset;
        int yr = (int)respawn.transform.position.y + yOffset;
        board[xr, yr] = respawn;
    }

    internal Vector2 GetRandomWorldCoordWithinOuterWalls()
    {
        int col = rng.Next(-xOffset + 1, xOffset - 1);
        int row = rng.Next(-yOffset + 1, yOffset - 1);
        return new Vector2(col, row);
    }

    private bool ValidBoardCoord(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    internal bool TryMoveNearbyBox(int x, int y)
    {
        var boardCoord = GameToBoardCoordinate(x, y);
        int boxX = -1, boxY = -1, emptyX = -1, emptyY = -1;

        for (int i = (int)boardCoord.x - 1; i < (int)boardCoord.x + 1; i++)
        {
            for (int j = (int)boardCoord.y - 1; j < (int)boardCoord.y + 1; j++)
            {
                if(!ValidBoardCoord(i, j))
                    continue;

                if (board[i, j] == null)
                {
                    emptyX = i;
                    emptyY = j;

                    if (boxX >= 0) // how is this not equiv to boxX > -1?
                    {
                        SwapWithEmpty(emptyX, emptyY, boxX, boxY);
                        return true;
                    }
                }
                else if (board[i, j].tag == "Box")
                {
                    boxX = i;
                    boxY = j;

                    if(emptyX >= 0)
                    {
                        SwapWithEmpty(emptyX, emptyY, boxX, boxY);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SwapWithEmpty(int emptyX, int emptyY, int boxX, int boxY)
    {
        board[emptyY, emptyY] = board[boxX, boxY];
        board[emptyY, emptyY].transform.position = BoardToGameCoordinate(emptyX, emptyY);
        board[boxX, boxY] = null;
    }

    public Vector2 BoardToGameCoordinate(int col, int row)
    {
        return new Vector2(col - xOffset, row - yOffset);
    }

    Vector2 GameToBoardCoordinate(int col, int row)
    {
        return new Vector2(col + xOffset, row + yOffset);
    }

    private int GetEmptyTileCount()
    {
        return width * height - currentBedCount - currentBoxCount -
            currentWallCount - currentWindowCount - 1; //1=respawn
    }

    private int GetTileCount()
    {
        return width * height;
    }

    internal void AddBoxes(int boxCount)
    {
        var rng = new System.Random();
        int emptyTileCount = GetEmptyTileCount();
        int[] newBoxTileIndeces = new int[4] {
            rng.Next(emptyTileCount),
            rng.Next(emptyTileCount-1),
            rng.Next(emptyTileCount-2),
            rng.Next(emptyTileCount-3)
        };

        Array.Sort(newBoxTileIndeces, 0, 4);
        var iter = newBoxTileIndeces.GetEnumerator();
        iter.MoveNext();

        int counter = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                    continue;

                if (counter == (int)iter.Current)
                {
                    Debug.Assert(board[i, j] == null);
                    var instaPos = BoardToGameCoordinate(i, j);
                    board[i, j] = Instantiate(BoxPrefab,
                        instaPos, Quaternion.identity);

                    if (!iter.MoveNext())
                    {
                        Debug.Assert(counter <= width * height);
                        currentBoxCount += 4;
                        emptyTileCount -= 4;
                        return;
                    }
                }
                counter++;
            }
        }
    }
}
