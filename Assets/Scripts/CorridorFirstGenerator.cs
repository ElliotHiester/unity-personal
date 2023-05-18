using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstGenerator : RanWalkGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField] [Range(0.1f, 1)] private float roomPercent = 0.8f;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject endObject;
    [SerializeField] private bool spawnPlayerAndEnd = false;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);
        Vector2Int playerPosition;
        Vector2Int endPosition;
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions, out playerPosition, out endPosition);

        if(spawnPlayerAndEnd)
        {
            Instantiate(playerObject, (Vector3Int)playerPosition, Quaternion.identity);
            Instantiate(endObject, (Vector3Int)endPosition, Quaternion.identity);
        }
        
        List<Vector2Int> deadEnds = FindDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(!roomFloors.Contains(position))
            {
                var room = RunRandomWalk(ranWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach (var position in floorPositions)
        {
            int neighborCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPositions.Contains(position + direction))
                    neighborCount++;
            }

            if (neighborCount == 1)
                deadEnds.Add(position);
        }

        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions, out Vector2Int playerPosition, out Vector2Int endPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        List<Tuple<float, Vector2Int, Vector2Int>> distances = new List<Tuple<float, Vector2Int, Vector2Int>>();
        foreach(var position in roomsToCreate)
        {
            foreach(var roomPosition in roomsToCreate)
            {
                var difference = position - roomPosition;
                var distance = difference.magnitude;

                var tuple = new Tuple<float, Vector2Int, Vector2Int>(distance, position, roomPosition);

                distances.Add(tuple);
            }
        }

        playerPosition = Vector2Int.zero;
        endPosition = Vector2Int.zero;

        float farthestDistance = 0.0f;
        foreach(var distance in distances)
        {
            if(distance.Item1 > farthestDistance)
            {
                farthestDistance = distance.Item1;
                playerPosition = distance.Item2;
                endPosition = distance.Item3;
            }
        }

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(ranWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);      

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProcGenAlgorithms.RanWalkCorridor(currentPosition, corridorLength);           
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }        
    }
}
