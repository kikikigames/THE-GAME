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
}
