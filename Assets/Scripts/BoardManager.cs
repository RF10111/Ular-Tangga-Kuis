using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public RectTransform boardParent; // RectTransform dari parent board
    public RectTransform[] boardTiles; // Array untuk menampung RectTransform dari tile
    public Dictionary<int, int> snakes;
    public Dictionary<int, int> ladders;

    private void Start()
    {
        InitializeSnakesAndLadders();
        InitializeBoardTiles();
    }

    private void InitializeSnakesAndLadders()
    {
        snakes = new Dictionary<int, int>
        {
            { 97, 39 },
            { 83, 57 },
            { 86, 48 },
            { 72, 14 },
            { 55, 7 },
            { 49, 4 },
            { 42, 16 }
        };

        ladders = new Dictionary<int, int>
        {
            { 1, 22 },
            { 5, 44 },
            { 19, 58 },
            { 56, 95 },
            { 51, 71 },
            { 70, 91 }
        };
    }

    public void InitializeBoardTiles()
    {
        // Atur posisi tile sesuai dengan urutan papan permainan menggunakan RectTransform
        float xStart = 0f;
        float yStart = 0f;
        float xOffset = 4f;
        float yOffset = 4f;

        for (int i = 0; i < boardTiles.Length; i++)
        {
            int row = i / 10;
            int col = i % 10;

            // Zig-zag pattern
            if (row % 2 == 1)
            {
                col = 9 - col; // This should correctly handle zig-zag
            }

            float xPos = xStart + (col * xOffset);
            float yPos = yStart + (row * yOffset); // Negative to move downwards

            boardTiles[i].anchoredPosition = new Vector2(xPos, yPos);

            Debug.Log($"Tile {i} position: {boardTiles[i].anchoredPosition}");
        }
    }

    public int CheckSnakeOrLadder(int tile)
    {
        if (snakes.ContainsKey(tile))
        {
            return snakes[tile];
        }
        if (ladders.ContainsKey(tile))
        {
            return ladders[tile];
        }
        return tile;
    }
}