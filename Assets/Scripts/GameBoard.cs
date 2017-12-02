using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {
    public int width;
    public int height;
    private GameObject[,] board;

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
        }

        var boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in boxes)
        {
            int x = (int)box.transform.position.x + xOffset;
            int y = (int)box.transform.position.y + yOffset;
            board[x, y] = box;
        }

        var windows = GameObject.FindGameObjectsWithTag("Window");
        foreach (var window in windows)
        {
            int x = (int)window.transform.position.x + xOffset;
            int y = (int)window.transform.position.y + yOffset;
            board[x, y] = window;
        }

        GameObject bed = GameObject.FindGameObjectWithTag("Bed");
        board[1, 1] = board[1, 2] = bed;
    }
}
