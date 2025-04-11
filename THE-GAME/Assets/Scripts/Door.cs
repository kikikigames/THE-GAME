using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public int doorId;
    [SerializeField] public GameObject doorMesh;
    [SerializeField] public Vector3 doorWorldPos;
    [SerializeField] public Vector3 doorRot; 
    [SerializeField] public bool isOpen;
    [SerializeField] public bool isIntersect; //Başka odalarla kesişimi kontrol eder
    [SerializeField] public bool isIntersectCorridor;
    [SerializeField]  public bool isInsideOfRoom;//Kendi içindeki odaların kesişimi kontrol eder
    public Door()
    {
        doorId=-1;
        doorMesh=null;
        doorWorldPos=Vector3.zero;
        doorRot=Vector3.zero;
        isOpen=false;
        isIntersect=false;
        isInsideOfRoom=false;
    }
    public Door(Door _door)
    {
        this.doorId=_door.doorId;
        this.doorMesh= _door.doorMesh;
        this.doorWorldPos= _door.doorWorldPos;
        this.doorRot=_door.doorRot;
        this.isOpen= _door.isOpen;
        this.isIntersect= _door.isIntersect;
        this.isInsideOfRoom= _door.isInsideOfRoom;
    }
    public Door(int _doorId,GameObject _doorMesh)
    {
        this.doorId=_doorId;
        this.doorMesh=_doorMesh;
    }
    public Door(int _doorId,GameObject _doorMesh,Vector3 _doorWorldPos,Vector3 _doorRot)
    {
        this.doorId=_doorId;
        this.doorMesh= _doorMesh;
        this.doorWorldPos= _doorWorldPos;
        this.doorRot=_doorRot;
    }
    public Door(int _doorId,GameObject _doorMesh,Vector3 _doorWorldPos,Vector3 _doorRot,bool _isOpen,bool _isIntersect,bool _isInsideOfRoom)
    {
        this.doorId=_doorId;
        this.doorMesh= _doorMesh;
        this.doorWorldPos= _doorWorldPos;
        this.doorRot=_doorRot;
        this.isOpen=_isOpen;
        this.isIntersect= _isIntersect;
        this.isInsideOfRoom= _isInsideOfRoom;
    }               
}  
