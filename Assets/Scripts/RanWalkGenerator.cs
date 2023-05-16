using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RanWalkGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField] private int iterations = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;

    [SerializeField] private TileMapVisualizer tileMapVisualizer;

    public void RunProcGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for(int i = 0; i < iterations; i++)
        {
            var path = ProcGenAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);

            if (startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
 }
