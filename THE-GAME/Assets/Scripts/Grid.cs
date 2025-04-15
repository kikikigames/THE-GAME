using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Floor References")]
    [SerializeField] Transform parentFloorTransform;
    [SerializeField] GameObject whiteFloorPrefab;

    [Header("Grid Parameters")]
    [SerializeField] Vector3 bottomLeft;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] public Vector2 gridWorldSize;
    [SerializeField] public float nodeRadius;
    
    //Arrays
    public Node[,] grid;
    public Node[,] gridFloor;
    public Node[,] gridDoor;
    
    //Variables
    public float nodeDiameter;
    public int gridSizeX, gridSizeZ;
    [SerializeField] private int corridorLeftOffset;
    [SerializeField] private int corridorRightOffset;
    [SerializeField] private int corridorTopOffset;
    [SerializeField] private int corridorBottomOffset;
    [SerializeField] private int extraCorridorLeftOffset;
    [SerializeField] private int extraCorridorRightOffset;
    [SerializeField] private int extraCorridorTopOffset;
    [SerializeField] private int extraCorridorBottomOffset;
    [SerializeField] private int roomLeftOffset;
    [SerializeField] private int roomRightOffset;
    [SerializeField] private int roomTopOffset;
    [SerializeField] private int roomBottomOffset;


    [Header("DoorPrefabs")]
    [SerializeField] private GameObject doorPrefabUp;
    [SerializeField] private GameObject doorPrefabRight;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        CreateWalls();
    }
    void Update()
    {
        UpdateFloor();
    }
    /// <summary>
    /// Grid oluşturulur.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeZ; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Wall[] walls = CreateNodeWalls(worldPoint);
                grid[x, y] = new Node(null, FloorType.whiteFloorRoomId, worldPoint, x, y, walls, -1);
            }
        }
    }
    /// <summary>
    /// Zemin oluşturulur.
    /// </summary>
    private void CreateWalls()
    {
        gridFloor = new Node[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeZ; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                GameObject _floor = Instantiate(whiteFloorPrefab, worldPoint, Quaternion.identity);
                Wall[] walls = CreateNodeWalls(worldPoint);
                gridFloor[x, y] = new Node(_floor, FloorType.whiteFloorRoomId, worldPoint, x, y, walls, -1);
                _floor.transform.SetParent(parentFloorTransform);
            }
        }
    }
    /// <summary>
    /// Duvarlar oluşturulur.
    /// </summary>
    /// <param name="worldPoint"></param>
    /// <returns></returns>
    private Wall[] CreateNodeWalls(Vector3 worldPoint)
    {
        Wall[] walls = new Wall[4];
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = new Wall(); // Her bir kapıyı başlat
        }
        walls[0].wallWorldPos = worldPoint + new Vector3(0, 0, nodeRadius); //Yukari
        walls[1].wallWorldPos = worldPoint + new Vector3(nodeRadius, 0, 0); //Sag
        walls[2].wallWorldPos = worldPoint + new Vector3(0, 0, -nodeRadius); //Asagi
        walls[3].wallWorldPos = worldPoint + new Vector3(-nodeRadius, 0, 0); //Sol

        return walls;
    }
    /// <summary>
    /// Zemin noktası güncellenir.
    /// </summary>
    private void UpdateFloor()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeZ; y++)
            {
                gridFloor[x, y] = new Node(gridFloor[x, y].floor.floorGameObject, grid[x, y].floor.floorType, grid[x, y].worldPosition, x, y, grid[x, y].walls, grid[x, y].roomId);
                if (gridFloor[x, y].floor.floorType == FloorType.Red)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if (gridFloor[x, y].floor.floorType == FloorType.whiteFloorRoomId)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                }
                else if (gridFloor[x, y].floor.floorType == FloorType.yellowFloorRoomId)
                {
                    gridFloor[x, y].floor.floorGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
    }
    /// <summary>
    /// Grid dünyasındaki noktanın dünya noktası hesaplanır.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <returns></returns>
    public Vector3 CalculateWorldPoint(int xRandom, int zRandom)
    {
        return bottomLeft + Vector3.right * (xRandom * nodeDiameter + nodeRadius) + Vector3.forward * (zRandom * nodeDiameter + nodeRadius);
    }
    /// <summary>
    /// Dünya noktasını alarak grid dünyasındaki noktayı bulur.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = (int)(worldPosition.x - bottomLeft.x) / (int)nodeDiameter;
        int y = (int)(worldPosition.z - bottomLeft.z) / (int)nodeDiameter;
        return grid[x, y];
    }
    /// <summary>
    /// Oda uygun mu kontrol edilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    public bool CheckIfRoomFits(int xRandom, int zRandom, GameObject room)
    {
        int xLocal = 0, zLocal = 0;

        if (room.TryGetComponent<Room>(out Room roomComp))
        {
            xLocal = (int)(room.GetComponent<Room>()?.size.x / nodeDiameter);
            zLocal = (int)(room.GetComponent<Room>()?.size.z / nodeDiameter);
        }

        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                if (grid[xRandom + i, zRandom + j].roomId != -1) // Oda zaten var mı?
                {
                    return false;
                }
            }
        }

        return true;
    }
    /// <summary>
    /// Oda çizilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    public void DrawRoom(int xRandom, int zRandom, GameObject room, int roomId)
    {
        int xLocal = 0, zLocal = 0;

        if (room.TryGetComponent<Room>(out Room roomComp))
        {
            xLocal = (int)(room.GetComponent<Room>()?.size.x / nodeDiameter);
            zLocal = (int)(room.GetComponent<Room>()?.size.z / nodeDiameter);
        }
        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                grid[xRandom + i, zRandom + j].floor.floorType = FloorType.Red;
                gridFloor[xRandom + i, zRandom + j].floor.floorType = FloorType.Red;
                grid[xRandom + i, zRandom + j].roomId = roomId;
                gridFloor[xRandom + i, zRandom + j].roomId = roomId;
                FloorManager.AddRedFloor(grid[xRandom + i, zRandom + j]);

                //Wall idlerini room idleriyle esitledim
                for (int x = 0; x < grid[xRandom + i, zRandom + j].walls.Length; x++)
                {
                    grid[xRandom + i, zRandom + j].walls[x].wallId = roomId;
                    gridFloor[xRandom + i, zRandom + j].walls[x].wallId = roomId;
                }
            }
        }
        CreateRoomWalls(xRandom, zRandom, xLocal, zLocal, room);
    }
    /// <summary>
    /// Tüm nodeların komşularına bakarak duvarlar oluşturur.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="xLocal"></param>
    /// <param name="zLocal"></param>
    /// <param name="room"></param>
    public void CreateRoomWalls(int xRandom, int zRandom, int xLocal, int zLocal, GameObject room)
    {
        int currentId = -2;
        Node[] nodeNeighbor = new Node[4];
        List<GameObject> neighborList = new List<GameObject>();//Komşunun nodeları
        Dictionary<int, List<GameObject>> neighborDict = new Dictionary<int, List<GameObject>>(); //Birden fazla komşunun nodelarını tutan dict
        for (int i = 0; i < xLocal; i++)
        {
            for (int j = 0; j < zLocal; j++)
            {
                for (int x = 0; x < nodeNeighbor.Length; x++)
                {
                    if (x == 0)
                    {
                        nodeNeighbor[x] = GetNeighbor(grid[xRandom + i, zRandom + j], Vector2.up);
                        if (nodeNeighbor[x].roomId == -1) //Beyazsa 
                        {
                            //DoorPrefab olusturmaca
                            GameObject tempWall = Instantiate(doorPrefabUp, grid[xRandom + i, zRandom + j].walls[x].wallWorldPos, doorPrefabUp.transform.rotation);
                            tempWall.transform.SetParent(room.transform.GetChild(0).GetChild(1).transform, true);
                            grid[xRandom + i, zRandom + j].walls[x].wallObject = tempWall;
                        }
                        else if (nodeNeighbor[x].roomId > 0)//Beyaz veya sari degilse
                        {
                            if (nodeNeighbor[x].roomId != grid[xRandom + i, zRandom + j].roomId)
                            {
                                currentId = nodeNeighbor[x].roomId;// Komşunun alt kapının kesiştiğini anlamak için
                                neighborList.Add(nodeNeighbor[x].walls[2].wallObject);
                            }
                        }
                    }
                    if (x == 1)
                    {
                        nodeNeighbor[x] = GetNeighbor(grid[xRandom + i, zRandom + j], Vector2.right);
                        if (nodeNeighbor[x].roomId == -1) //Beyazsa 
                        {
                            //DoorPrefab olusturmaca
                            GameObject tempWall = Instantiate(doorPrefabRight, grid[xRandom + i, zRandom + j].walls[x].wallWorldPos, doorPrefabRight.transform.rotation) as GameObject;
                            tempWall.transform.SetParent(room.transform.GetChild(0).GetChild(1).transform, true);
                            grid[xRandom + i, zRandom + j].walls[x].wallObject = tempWall;
                        }
                        else if (nodeNeighbor[x].roomId > 0)
                        {
                            if (nodeNeighbor[x].roomId != grid[xRandom + i, zRandom + j].roomId)
                            {
                                currentId = nodeNeighbor[x].roomId; // Komşunun sağ kapının kesiştiğini anlamak için
                                neighborList.Add(nodeNeighbor[x].walls[3].wallObject);
                            }
                        }
                    }
                    if (x == 2)
                    {
                        nodeNeighbor[x] = GetNeighbor(grid[xRandom + i, zRandom + j], Vector2.down);
                        if (nodeNeighbor[x].roomId == -1) //Beyazsa 
                        {
                            //DoorPrefab olusturmaca
                            GameObject tempWall = Instantiate(doorPrefabUp, grid[xRandom + i, zRandom + j].walls[x].wallWorldPos, doorPrefabUp.transform.rotation) as GameObject;
                            tempWall.transform.SetParent(room.transform.GetChild(0).GetChild(1).transform, true);
                            grid[xRandom + i, zRandom + j].walls[x].wallObject = tempWall;
                        }
                        else if (nodeNeighbor[x].roomId > 0)
                        {
                            if (nodeNeighbor[x].roomId != grid[xRandom + i, zRandom + j].roomId)
                            {
                                currentId = nodeNeighbor[x].roomId; // Komşunun aşağı kapının kesiştiğini anlamak için
                                neighborList.Add(nodeNeighbor[x].walls[0].wallObject);
                            }
                        }
                    }
                    if (x == 3)
                    {
                        nodeNeighbor[x] = GetNeighbor(grid[xRandom + i, zRandom + j], Vector2.left);
                        if (nodeNeighbor[x].roomId == -1) //Beyazsa 
                        {
                            //DoorPrefab olusturmaca
                            GameObject tempWall = Instantiate(doorPrefabRight, grid[xRandom + i, zRandom + j].walls[x].wallWorldPos, doorPrefabRight.transform.rotation) as GameObject;
                            tempWall.transform.SetParent(room.transform.GetChild(0).GetChild(1).transform, true);
                            grid[xRandom + i, zRandom + j].walls[x].wallObject = tempWall;
                        }
                        else if (nodeNeighbor[x].roomId > 0)
                        {
                            if (nodeNeighbor[x].roomId != grid[xRandom + i, zRandom + j].roomId)
                            {
                                currentId = nodeNeighbor[x].roomId; // Komşunun sol kapının kesiştiğini anlamak için
                                neighborList.Add(nodeNeighbor[x].walls[1].wallObject);
                            }
                        }
                    }
                    if (!neighborDict.ContainsKey(currentId))
                    {
                        neighborDict[currentId] = new List<GameObject>();//İlk listeyi oluşturdum.
                    }
                    // Tekrar edenleri önlemek için sadece yeni olanları ekle
                    foreach (var neighbor in neighborList)
                    {
                        if (!neighborDict[currentId].Contains(neighbor))
                        {
                            neighborDict[currentId].Add(neighbor);
                        }
                    }
                }
            }
        }
        OpenRandomDoor(neighborDict);
    }
    /// <summary>
    /// Komşu dictionarydeki her listeden bir random numaradaki duvarı kapatıyor.
    /// </summary>
    /// <param name="neighborDict"></param>
    /// <param name="room"></param>
    public void OpenRandomDoor(Dictionary<int, List<GameObject>> neighborDict)
    {
        foreach (var gameObject in neighborDict)
        {
            int randomNumber = Random.Range(0, gameObject.Value.Count);
            for (int i = 0; i < gameObject.Value.Count; i++)
            {
                if (i == randomNumber)
                {
                    gameObject.Value[i].SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// Nodeun verilen posizyona göre komşusunu bulur.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Node GetNeighbor(Node node, Vector2 pos)
    {
        Vector2 currentPos = new Vector2(node.gridX, node.gridY) + pos;
        return grid[(int)currentPos.x, (int)currentPos.y];
    }
    /// <summary>
    /// Tüm komşular bulunur. Liste olarak döner. Pathfinding için kullanılır.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int[,] directions = new int[,]
        {
            { 0, 1 },  // Yukarı
            { 1, 0 },  // Sağ
            { 0, -1},  // Aşağı
            { -1, 0 }  // Sol
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = node.gridX + directions[i, 0];
            int checkY = node.gridY + directions[i, 1];

            if (IsCorridorInsideOfGrid(checkX,checkY))
            {
                neighbors.Add(grid[checkX, checkY]);
            }
        }
        return neighbors;
    }
    /// <summary>
    /// Tüm komşular bulunur. Array olarak döner.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Node[] GetNeighborNodes(Node node)
    {
        Node[] nodes = new Node[4];
        nodes[0] = GetNeighbor(node, Vector2.up) != null ? GetNeighbor(node, Vector2.up) : null;
        nodes[1] = GetNeighbor(node, Vector2.right) != null ? GetNeighbor(node, Vector2.right) : null;
        nodes[2] = GetNeighbor(node, Vector2.down) != null ? GetNeighbor(node, Vector2.down) : null;
        nodes[3] = GetNeighbor(node, Vector2.left) != null ? GetNeighbor(node, Vector2.left): null;
        return nodes;
    }
    /// <summary>
    /// Pathfinding başlangıç ve bitiş noktası için tasarlandı. O noktaların etrafında koridor varsa o duvar deaktive olur.
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="currentNode"></param>
    public void CheckPathNeighbors(List<Node> neighbors,Node currentNode)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].roomId==0)
            {
                currentNode.walls[i].wallObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Koridorun gridin içinde olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridZ"></param>
    /// <returns></returns>
    public bool IsCorridorInsideOfGrid(int gridX,int gridZ)
    {
       if(gridX <corridorLeftOffset || gridX > gridSizeX - corridorRightOffset || gridZ < corridorBottomOffset || gridZ > gridSizeZ - corridorTopOffset)
            return false;
        return true;
    }
    /// <summary>
    /// Ekstra koridorun gridin içinde olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridZ"></param>
    /// <returns></returns>
     public bool IsExtraCorridorInsideOfGrid(int gridX,int gridZ)
    {
       if(gridX <extraCorridorLeftOffset || gridX > gridSizeX - extraCorridorRightOffset || gridZ < extraCorridorBottomOffset || gridZ > gridSizeZ - extraCorridorTopOffset)
            return false;
        return true;
    }
    /// <summary>
    /// Odanın gridin içinde olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="xLocal"></param>
    /// <param name="xRandom"></param>
    /// <param name="z"></param>
    /// <param name="zLocal"></param>
    /// <param name="zRandom"></param>
    /// <returns></returns>
     public bool IsRoomInsideOfGrid(int x,int xLocal, int xRandom, int z, int zLocal, int zRandom)
    {
        if (x < roomLeftOffset || xRandom > x - xLocal -roomRightOffset || z < roomBottomOffset|| zRandom > z - zLocal -roomTopOffset)
            return false;
        return true;
    }
}