using System.Collections.Generic;

public static class FloorManager 
{
    public static List<Node> yellowFloors = new List<Node>();
    public static List<Node> redFloors = new List<Node>();
    
    public static void AddYellowFloor(Node yellowFloor)
    {
        if(yellowFloor.floor.inTheList)
            return;
        yellowFloors.Add(yellowFloor);
        yellowFloor.floor.inTheList = true;
    }
    public static void AddRedFloor(Node redFloor)
    {
        if(redFloor.floor.inTheList)
            return;
        redFloors.Add(redFloor);
        redFloor.floor.inTheList = true;
    }
    public static void RemoveYellowFloor(Node yellowFloor)
    {
        yellowFloors.Remove(yellowFloor);
        yellowFloor.floor.inTheList = false;
    }
    public static void RemoveRedFloor(Node redFloor)
    {
        redFloors.Remove(redFloor);
        redFloor.floor.inTheList = false;
    }
}
