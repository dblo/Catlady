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

    void Start()
    {
        board = new GameObject[width, height];
        var xOffset = width / 2;
        var yOffset = height / 2;

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

    Vector2 BoardToGameCoordinates(int col, int row)
    {
        var colOffset = width / 2;
        var rowOffset = height / 2;
        return new Vector2(col - colOffset, row - rowOffset);
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
                    var instaPos = BoardToGameCoordinates(i, j);
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
