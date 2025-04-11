using UnityEngine;

public class Floor 
{
    public GameObject floorGameObject;
    public FloorType floorType;
    public bool inTheList;
    public Floor(GameObject _floor, FloorType _floorType)
    {
        this.floorGameObject = _floor;
        this.floorType = _floorType;
    }
    public Floor(GameObject _floor, FloorType _floorType,bool _inTheList)
    {
        this.floorGameObject = _floor;
        this.floorType = _floorType;
        this.inTheList = _inTheList;
    }
}
