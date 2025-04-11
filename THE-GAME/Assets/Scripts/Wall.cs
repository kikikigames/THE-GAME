using UnityEngine;
public class Wall 
{
    [SerializeField] public int wallId;
    [SerializeField] public GameObject wallObject;
    [SerializeField] public Vector3 wallWorldPos;
    [SerializeField] public Vector3 wallRot; 
    [SerializeField] public bool isOpen;
    [SerializeField] public bool isIntersect; //Başka odalarla kesişimi kontrol eder
    [SerializeField] public bool isIntersectCorridor;
    [SerializeField]  public bool isInsideOfRoom;//Kendi içindeki odaların kesişimi kontrol eder
    public Wall()
    {
        wallId=-1;
        wallObject=null;
        wallWorldPos=Vector3.zero;
        wallRot=Vector3.zero;
        isOpen=false;
        isIntersect=false;
        isInsideOfRoom=false;
    }
    public Wall(Wall _wall)
    {
        this.wallId=_wall.wallId;
        this.wallObject= _wall.wallObject;
        this.wallWorldPos= _wall.wallWorldPos;
        this.wallRot=_wall.wallRot;
        this.isOpen= _wall.isOpen;
        this.isIntersect= _wall.isIntersect;
        this.isInsideOfRoom= _wall.isInsideOfRoom;
    }
    public Wall(int _wallId,GameObject _wallMesh)
    {
        this.wallId=_wallId;
        this.wallObject=_wallMesh;
    }
    public Wall(int _wallId,GameObject _wallMesh,Vector3 _wallWorldPos,Vector3 _wallRot)
    {
        this.wallId=_wallId;
        this.wallObject= _wallMesh;
        this.wallWorldPos= _wallWorldPos;
        this.wallRot=_wallRot;
    }
    public Wall(int _wallId,GameObject _wallMesh,Vector3 _wallWorldPos,Vector3 _wallRot,bool _isOpen,bool _isIntersect,bool _isInsideOfRoom)
    {
        this.wallId=_wallId;
        this.wallObject= _wallMesh;
        this.wallWorldPos= _wallWorldPos;
        this.wallRot=_wallRot;
        this.isOpen=_isOpen;
        this.isIntersect= _isIntersect;
        this.isInsideOfRoom= _isInsideOfRoom;
    }               
}  
