    using UnityEngine;

public class CorridorGeneration : MonoBehaviour
{
    [Header("Corridors")]
    [SerializeField] GameObject[] corridors;
    [SerializeField] Transform parentCorridors;
    Grid grid;

    //Variables
    [SerializeField] private int randomIndexNumber;
    [SerializeField] private int yellowFloorRoomId;
    [SerializeField] private int whiteFloorRoomId;

    private int counter;
    void Awake()
    {
        grid = FindFirstObjectByType<Grid>();
    }
    void Start()
    {
        counter=0;
        CreateExtraCorridor();
        CreateCorridor();
    }
    /// <summary>
    /// Ekstra koridor oluşturur. Ekstra koridorlar, sarı odaların etrafındaki beyaz olan odaları sarı yapar (+ şeklinde).
    /// </summary>
    private void CreateExtraCorridor()
    {
        while(counter < randomIndexNumber)
        {
            int randomIndex = Random.Range(0, FloorManager.yellowFloors.Count);
            //Debug.LogError("Random index " + FloorManager.yellowFloors[randomIndex].gridX + " " + FloorManager.yellowFloors[randomIndex].gridY);
            Node[] neighbors = grid.GetNeighborNodes(FloorManager.yellowFloors[randomIndex]);
            if (grid.IsCorridorInsideOfGrid(FloorManager.yellowFloors[randomIndex].gridX, FloorManager.yellowFloors[randomIndex].gridY))
            {
                counter++;
                for (int i = 0; i < neighbors.Length; i++)
                {
                    //Çevresi beyazsa koridor ekle
                    if (neighbors[i]?.roomId == whiteFloorRoomId&& grid.IsExtraCorridorInsideOfGrid(neighbors[i].gridX, neighbors[i].gridY))
                    {
                        neighbors[i].roomId = yellowFloorRoomId;
                        FloorManager.AddYellowFloor(neighbors[i]);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Koridorları oluşturur.
    /// </summary>
    private void CreateCorridor()
    {
        for (int i = 0; i < FloorManager.yellowFloors.Count; i++)
        {
            int index = GetObjectIndex(grid.GetNeighborNodes(FloorManager.yellowFloors[i]));
            Vector3 nodeWorldPos = grid.CalculateWorldPoint(FloorManager.yellowFloors[i].gridX, FloorManager.yellowFloors[i].gridY);
            if (index >= 0)
            {
                GameObject roomTemp = Instantiate(corridors[index], nodeWorldPos, corridors[index].transform.rotation);
                roomTemp.transform.SetParent(parentCorridors);
            }
            else
                Debug.LogError("Corridor olustururken index 0dan kucuk oldu");
        }
    }
     /// <summary>
     /// Node etrafındaki odaların durumunu kontrol eder. Onlara göre uygun odayı seçer.
     /// </summary>
     /// <param name="nodes"></param>
     /// <returns></returns>
    private int GetObjectIndex(Node[] nodes)
    {
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 0; //4 yol agzi
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 1; //3 yol agzi
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
            (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 2;//3 yol agzi + 90
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 3;//3 yol agzi + 180
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
            (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 4; //3yol agzi +270
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 5; //2 yol agzi 
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
           (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
           (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
           (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 6; //2 yol agzi +90
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 7; //L 2 yol agzi 
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
           (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
           (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
           (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 8; //L 2 yol agzi +90
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
           (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
           (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
           (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 9; //L 2 yol agzi +180
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
           (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
           (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
           (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 10; //L 2 yol agzi +270
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
            (nodes[1].roomId == yellowFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == false) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 11; //Tek yol agzi
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
             (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
             (nodes[2].roomId == yellowFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == false) &&
             (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 12; //Tek yol agzi +90
        if ((nodes[0].roomId == whiteFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == true) &&
            (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == yellowFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == false))
            return 13; //Tek yol agzi+180
        if ((nodes[0].roomId == yellowFloorRoomId || nodes[0].walls[2].wallObject?.activeSelf == false) &&
            (nodes[1].roomId == whiteFloorRoomId || nodes[1].walls[3].wallObject?.activeSelf == true) &&
            (nodes[2].roomId == whiteFloorRoomId || nodes[2].walls[0].wallObject?.activeSelf == true) &&
            (nodes[3].roomId == whiteFloorRoomId || nodes[3].walls[1].wallObject?.activeSelf == true))
            return 14; //Tek yol agzi+270
        return -1;
    }
}
