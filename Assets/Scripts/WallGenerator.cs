using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, tileMapVisualizer, Direction2D.cardinalDirectionsList);
        foreach(var position in basicWallPositions)
        {
            tileMapVisualizer.PaintSingleWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighborPosition = position + direction;

                if (!floorPositions.Contains(neighborPosition))
                    wallPositions.Add(neighborPosition);
            }
        }


        List<Vector2Int> singleWallPositions = new List<Vector2Int>();

        foreach(var position in wallPositions)
        {
            int neighbors = 0;

            foreach(var direction in directionList)
            {
                var neighborPosition = position + direction;
                if (wallPositions.Contains(neighborPosition) || !floorPositions.Contains(neighborPosition))
                    neighbors++;
            }

            if(neighbors == 0)
            {
                singleWallPositions.Add(position);
            }
        }

        foreach(var position in singleWallPositions)
        {
            wallPositions.Remove(position);
        }

        tileMapVisualizer.PaintFloorTiles(singleWallPositions);

        return wallPositions;
    }
}
