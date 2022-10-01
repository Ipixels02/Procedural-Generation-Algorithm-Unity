using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator // Класс предок абстрактног класса для запуска метода RunRadomWalk
{

    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;
    //protected DropdownHandler randomWalkParametersnew;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        DestroyObjects();
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.RandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }

    private void DestroyObjects() // чистим старые объекты
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawnable"))
        {
            DestroyImmediate(obj);
        }
    }
}
