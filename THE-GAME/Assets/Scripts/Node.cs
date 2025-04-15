using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    public Floor floor;

    public Wall[] walls;
    public int roomId;//white floor=-1, Path roomId=0, other rooms=1+ 
    public Node( Vector3 _worldPost, int _gridX, int _gridY)
    {
        this.worldPosition = _worldPost;
        this.gridX = _gridX;
        this.gridY = _gridY;
    }
    public Node(Floor _floor,Vector3 _worldPost, int _gridX, int _gridY)
    {
        this.worldPosition = _worldPost;
        this.gridX = _gridX;
        this.gridY = _gridY;
        this.floor = _floor;
    }
    public Node(GameObject _floor,FloorType _floorType,Vector3 _worldPost, int _gridX, int _gridY, Wall[] _walls,int _roomId)
    {
        this.worldPosition = _worldPost;
        this.gridX = _gridX;
        this.gridY = _gridY;
        this.floor = new Floor(_floor,_floorType);
        this.roomId= _roomId;
        // **Doors dizisini oluştur ve kopyala**
        if (_walls != null)
        {
            this.walls = new Wall[_walls.Length]; // Bellek tahsis ediliyor
            for (int i = 0; i < _walls.Length; i++)
            {
                this.walls[i] = _walls[i]; // Kapıları atıyoruz
            }
        }
        else
        {
            this.walls = new Wall[0]; // Eğer _doors null ise boş bir dizi oluştur
        }
    }
    public Node(Floor _floor,Vector3 _worldPost, int _gridX, int _gridY, Wall[] _walls,int _roomId)
    {
        this.worldPosition = _worldPost;
        this.gridX = _gridX;
        this.gridY = _gridY;
        this.floor = _floor;
        this.roomId= _roomId;
        // **Doors dizisini oluştur ve kopyala**
        if (_walls != null)
        {
            this.walls = new Wall[_walls.Length]; // Bellek tahsis ediliyor
            for (int i = 0; i < _walls.Length; i++)
            {
                this.walls[i] = _walls[i]; // Kapıları atıyoruz
            }
        }
        else
        {
            this.walls = new Wall[0]; // Eğer _doors null ise boş bir dizi oluştur
        }
    }
    
    public int fCost
    {
        get { return gCost + hCost; }
    }
}
public enum FloorType
{
    whiteFloorRoomId,// Walkable
    Red,// Unwalkable
    yellowFloorRoomId,//Path
    None,
}
