using UnityEngine;
using System.Collections.Generic;

public class RoomPrefab : MonoBehaviour
{
    [Header("Room Properties")]
    [SerializeField] private string roomName;
    [SerializeField] private RoomType roomType;
    [SerializeField] private Vector2 roomSize = new Vector2(10f, 10f);
    
    [Header("Connection Points")]
    [SerializeField] private List<Transform> connectionPoints;
    
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> itemSpawnPoints;
    [SerializeField] private List<Transform> enemySpawnPoints;
    
    [Header("Room Features")]
    [SerializeField] private bool hasWindows;
    [SerializeField] private bool hasLights;
    [SerializeField] private bool isMainRoom;
    
    public string RoomName => roomName;
    public RoomType RoomType => roomType;
    public Vector2 RoomSize => roomSize;
    public List<Transform> ConnectionPoints => connectionPoints;
    public List<Transform> ItemSpawnPoints => itemSpawnPoints;
    public List<Transform> EnemySpawnPoints => enemySpawnPoints;
    public bool HasWindows => hasWindows;
    public bool HasLights => hasLights;
    public bool IsMainRoom => isMainRoom;
}

public enum RoomType
{
    Standard,
    Corridor,
    Large,
    Entrance,
    Exit,
    Special
} 