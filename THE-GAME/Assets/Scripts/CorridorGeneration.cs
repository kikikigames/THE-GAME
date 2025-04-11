using Unity.Mathematics;
using UnityEngine;

public class CorridorGeneration : MonoBehaviour
{
    [Header("Corridors")]	
    [SerializeField] GameObject [] corridors;
    [SerializeField] Transform parentCorridors;
    Grid grid;
    void Awake()
    {
        grid=FindFirstObjectByType<Grid>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateCorridor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateCorridor()
    {
        for(int i=0; i<FloorManager.yellowFloors.Count;i ++)
        {
            int index=GetObjectIndex(grid.GetNeighborColors(FloorManager.yellowFloors[i]));
            Vector3 nodeWorldPos=grid.CalculateWorldPoint(FloorManager.yellowFloors[i].gridX,FloorManager.yellowFloors[i].gridY);
            if(index >=0)
            {
                GameObject roomTemp = Instantiate(corridors[index],nodeWorldPos,corridors[index].transform.rotation);
                roomTemp.transform.SetParent(parentCorridors);
            }
            else
                Debug.LogError("Corridor olustururken index 0dan kucuk oldu");
        }
    }
    // Renk kombinasyonuna göre obje seçen metod
    private int GetObjectIndex(FloorType [] floorTypes)
    {
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 0; //4 yol agzi
        if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 1; //3 yol agzi
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 2;//3 yol agzi + 90
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 3;//3 yol agzi + 180
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.White))
            return 4; //3yol agzi +270
        if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 5; //2 yol agzi 
         if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.White))
            return 6; //2 yol agzi +90
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.White))
            return 7; //L 2 yol agzi 
         if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.White))
            return 8; //L 2 yol agzi +90
         if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 9; //L 2 yol agzi +180
         if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 10; //L 2 yol agzi +270
        if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.Yellow|| floorTypes[1]==FloorType.Red)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.White))
            return 11; //Tek yol agzi
       if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.Yellow|| floorTypes[2]==FloorType.Red)&&
            (floorTypes[3]==FloorType.White))
            return 12; //Tek yol agzi +90
        if((floorTypes[0]==FloorType.White)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.Yellow|| floorTypes[3]==FloorType.Red))
            return 13; //Tek yol agzi+180
        if((floorTypes[0]==FloorType.Yellow|| floorTypes[0]==FloorType.Red)&&
            (floorTypes[1]==FloorType.White)&&
            (floorTypes[2]==FloorType.White)&&
            (floorTypes[3]==FloorType.White))
            return 14; //Tek yol agzi+270
        return -1;
    }
}
