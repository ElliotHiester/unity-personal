using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTileMap;
    [SerializeField] private TileBase floorTile, wall;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)     
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    internal void PaintSingleWall(Vector2Int position)
    {
        PaintSingleTile(wallTileMap, wall, position);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        wallTileMap.ClearAllTiles();
        floorTilemap.ClearAllTiles();
    }
}
