using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public Vector3 size;
    [SerializeField] public GameObject [] walls;
    [SerializeField] public int RoomId;
    [SerializeField] private int wallCount;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Vector3 [] wallPos;
    [SerializeField] private Vector3 [] wallRot;
    [SerializeField] private Transform wallParentTransform;
    Dictionary<Vector3,int> wallDict= new Dictionary< Vector3,int>();
    /* void Awake()
    {

        for (int i = 0; i < walls.Length; i++)
        {
            wallDict.Add(walls[i].transform.position,i);
        }
    } */
    /* private void CreateWall()
    {
        for (int i = 0; i < wallCount; i++)
        {
            Instantiate(wallPrefab,)
        }
    }
 */   /*  public void DeleteWall(Vector3 wallPos)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            if(wallDict.ContainsKey(wallPos))
            {
                walls[wallDict[wallPos]].gameObject.SetActive(false);
            }
        }
    } */
}
