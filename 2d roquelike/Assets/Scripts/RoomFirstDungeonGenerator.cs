using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 2;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    int numbertoSpawn;
    [SerializeField]
    private List<GameObject> spawnPool;
    [SerializeField]
    private GameObject Boss, Player;
    public int roomCount;
    private bool spawnedChar;
    Slider slider;
    Text Count;

    public void EditMinRoomWidth(string minroomWidth)
    {
        minRoomWidth = int.Parse(minroomWidth);
    }

    public void EditMinRoomHeight(string minroomHeight)
    {
        minRoomHeight = int.Parse(minroomHeight);
    }

    public void EditMinDungeonWidth(string mindungeonWidth)
    {
        dungeonWidth = int.Parse(mindungeonWidth);
    }

    public void EditMinDungeonHeight(string mindungeonHeight)
    {
        dungeonHeight = int.Parse(mindungeonHeight);
    }

    public void SliderOffset(float offset_new)
    {
        offset = (int) offset_new;
    }


    public void SetRandomWalk(bool random)
    {
        randomWalkRooms = random;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }


    //private IEnumerator CreateRooms()
    private void CreateRooms() //Алгоритм создания комнаты в концах лабиринта
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        //Debug.Log(roomCount);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        if (randomWalkRooms) // цикл для рандомной генерации комнаты
        {
            floor = CreateRoomsRandomly(roomsList);
            RandomSpawnObjects(roomsList);
            LastRoom(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
            RandomSpawnObjects(roomsList);
            LastRoom(roomsList);

        }
        //yield return new WaitForSecondsRealtime(0.5f); - создает комнаты поэтапно


        List<Vector2Int> roomCenters = new List<Vector2Int>(); // определяем центр комнаты для соединения комнат коридорами
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));            
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList) //рандомная генерация комнаты
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();       
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)//Соединяем комнаты коридорами
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)//метод по созданию коридора между комнатами
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters) //нахождение ближайшего соседнего центра комнаты для соединения коридором
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList) //Создаем комнату, добавляя клетку пола в список потенциальных позиций
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        roomCount = 0;
        foreach (var room in roomsList)
        {
            roomCount++;
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);                   
                }
            }            
        }
        return floor;
    }

    public static List<GameObject> GetRandomItemsFromList<GameObject>(List<GameObject> list, int number) //заносим в список декоротивные объекты в случайном порядке
    {
        // this is the list we're going to remove picked items from
        List<GameObject> tmpList = new List<GameObject>(list);
        // this is the list we're going to move items to
        List<GameObject> newList = new List<GameObject>();

        // make sure tmpList isn't already empty
        while (newList.Count < number && tmpList.Count > 0)
        {
            int index = Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }

        return newList;
    }

    public void RandomSpawnObjects(List<BoundsInt> roomsList) //метод случайной генерации декоративных объектов в комнатах
    {
        DestroyObjects();
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        int randomItem = 0;
        List<GameObject> randomItems = GetRandomItemsFromList<GameObject>(spawnPool, 4);
        GameObject toSpawn;
        float screenX, screenY;
        Vector2 positions;       
        for (int i = 0; i < numbertoSpawn; i++)
        {
            randomItem = Random.Range(0, randomItems.Count);
            toSpawn = randomItems[randomItem];
            foreach (var room in roomsList) {
                for (int col = 0; col < 2; col++)
                {
                    for (int row = 0; row < 2; row++)
                    {
                        screenX = Random.Range(room.min.x+1 + offset, room.max.x-1-offset);
                        screenY = Random.Range(room.min.y+1 + offset, room.max.y-1 - offset);
                        positions = new Vector2(screenX, screenY);
                        Instantiate(toSpawn, positions, toSpawn.transform.rotation);
                    }
                }
            }
        }
    }

    public void LastRoom(List<BoundsInt> roomsList) // нахождение первой и последней комнаты для создания игрока и босса 
    {
        DestroyBoss();
        DestroyPlayer();
        if (spawnedChar == false)
        {
            for (int i = 0; i < roomsList.Count; i++)
            {
                var roomBounds = roomsList[i];
                Vector3 roomCenter = new Vector3Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y),-30);
                //roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
                if (i == roomsList.Count - 1)
                {                   
                    Instantiate(Boss, roomCenter, Quaternion.identity);
                    spawnedChar = true;
                }
                else if (i == 0)
                {
                    Instantiate(Player, roomCenter, Quaternion.identity);
                }
            }
        }
        spawnedChar = false;
    }

    private void DestroyObjects() // чистим старые объекты
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawnable"))
        {
            DestroyImmediate(obj);
        }
    }

    private void DestroyBoss()//унчитожаем объект босса
    {
        foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            DestroyImmediate(boss);
        }
    }

    private void DestroyPlayer()//унчитожаем объект игрока
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            DestroyImmediate(player);
        }
    }
}
