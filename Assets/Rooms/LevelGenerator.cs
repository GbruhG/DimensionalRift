using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    [SerializeField] private List<RoomPrefab> standardRooms;
    [SerializeField] private RoomPrefab entranceRoom;
    [SerializeField] private RoomPrefab exitRoom;
    
    [Header("Generation Settings")]
    [SerializeField] private int minRooms = 5;
    [SerializeField] private int maxRooms = 10;
    [SerializeField] private int maxAttempts = 100;
    
    [Header("Connection Settings")]
    [SerializeField] private GameObject doorPrefab;
    
    private List<RoomInstance> placedRooms = new List<RoomInstance>();
    private RoomInstance entranceInstance;
    private RoomInstance exitInstance;
    
    private class ConnectionPointInfo
    {
        public Transform Point { get; set; }
        public RoomInstance Room { get; set; }
        
        public ConnectionPointInfo(Transform point, RoomInstance room)
        {
            Point = point;
            Room = room;
        }
    }
    
    private List<ConnectionPointInfo> availableConnectionPoints = new List<ConnectionPointInfo>();
    
    private void Start()
    {
        GenerateLevel();
    }
    
    public void GenerateLevel()
    {
        Debug.Log("Starting level generation...");
        
        // Clear existing rooms and ALL temporary objects
        foreach (Transform child in transform)
        {
            if (child != null) // Check if child still exists
            {
                Destroy(child.gameObject);
            }
        }
        placedRooms.Clear();
        availableConnectionPoints.Clear();
        
        // Validate prefabs
        if (entranceRoom == null || standardRooms.Count == 0)
        {
            Debug.LogError("Required prefabs are not assigned!");
            return;
        }
        
        // Step 1: Place entrance room
        Debug.Log("Placing entrance room...");
        entranceInstance = PlaceRoom(entranceRoom, Vector3.zero, Quaternion.identity);
        if (entranceInstance == null || entranceInstance.GameObject == null)
        {
            Debug.LogError("Failed to create entrance room!");
            return;
        }
        placedRooms.Add(entranceInstance);
        
        // Step 2: Add entrance room's connection points to the pool
        foreach (Transform connection in entranceRoom.ConnectionPoints)
        {
            availableConnectionPoints.Add(new ConnectionPointInfo(connection, entranceInstance));
        }
        
        // Step 3: Generate random rooms
        int targetRoomCount = Random.Range(minRooms, maxRooms + 1);
        Debug.Log($"Target room count: {targetRoomCount}");
        
        int attempts = 0;
        while (placedRooms.Count < targetRoomCount && attempts < maxAttempts)
        {
            if (availableConnectionPoints.Count == 0)
            {
                Debug.Log("No more available connection points!");
                break;
            }
            
            RoomPrefab roomPrefab = standardRooms[Random.Range(0, standardRooms.Count)];
            
            Debug.Log($"Attempting to place room {placedRooms.Count + 1} of {targetRoomCount}");
            
            if (TryPlaceRoom(roomPrefab))
            {
                attempts = 0;
                Debug.Log($"Successfully placed room {placedRooms.Count}");
            }
            else
            {
                attempts++;
                Debug.Log($"Failed to place room. Attempt {attempts} of {maxAttempts}");
            }
        }
        
        // Step 4: Place exit room
        Debug.Log("Placing exit room...");
        PlaceExitRoom();
        
        Debug.Log($"Level generation complete. Total rooms: {placedRooms.Count}");
    }
    
    private RoomInstance PlaceRoom(RoomPrefab prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Attempted to place null room prefab!");
            return null;
        }

        GameObject roomObj = Instantiate(prefab.gameObject, position, rotation, transform);
        if (roomObj == null)
        {
            Debug.LogError("Failed to instantiate room object!");
            return null;
        }

        roomObj.name = $"{prefab.name}_Instance_{placedRooms.Count}";
        RoomInstance instance = new RoomInstance(prefab, roomObj);
        return instance;
    }
    
    private bool TryPlaceRoom(RoomPrefab prefab)
    {
        // Create a list of indices and shuffle it
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < availableConnectionPoints.Count; i++)
        {
            availableIndices.Add(i);
        }
        
        GameObject tempObj = null; // Declare outside to ensure cleanup
        
        try
        {
            // Try connection points in random order
            while (availableIndices.Count > 0)
            {
                // Pick a random index
                int randomIndex = Random.Range(0, availableIndices.Count);
                int i = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);
                
                ConnectionPointInfo connectionInfo = availableConnectionPoints[i];
                if (connectionInfo == null || connectionInfo.Point == null)
                {
                    continue; // Skip invalid connection points
                }
                
                // Skip if this connection point is already used
                if (connectionInfo.Room.UsedConnectionPoints.Contains(connectionInfo.Point))
                {
                    continue;
                }
                
                // Shuffle the connection points of the new room
                List<Transform> shuffledConnections = new List<Transform>(prefab.ConnectionPoints);
                for (int j = shuffledConnections.Count - 1; j > 0; j--)
                {
                    int k = Random.Range(0, j + 1);
                    Transform temp = shuffledConnections[j];
                    shuffledConnections[j] = shuffledConnections[k];
                    shuffledConnections[k] = temp;
                }
                
                // Try each connection point on the new room in random order
                foreach (Transform newConnection in shuffledConnections)
                {
                    if (newConnection == null) continue;
                    
                    // Skip if this connection point is already used
                    if (connectionInfo.Room.UsedConnectionPoints.Contains(newConnection))
                    {
                        continue;
                    }
                    
                    // Calculate rotation first - we need this for correct position calculation
                    Quaternion targetRotation = Quaternion.FromToRotation(newConnection.forward, -connectionInfo.Point.forward);
                    
                    // Create a temporary object to help us calculate the correct position
                    if (tempObj != null) Destroy(tempObj);
                    tempObj = new GameObject("TempCalculation");
                    tempObj.transform.position = Vector3.zero;
                    tempObj.transform.rotation = targetRotation;
                    
                    // Get the connection point's position relative to the room's center after rotation
                    Vector3 connectionOffset = tempObj.transform.TransformPoint(newConnection.localPosition);
                    
                    // Calculate the final position by subtracting the rotated connection offset from the target point
                    Vector3 roomPosition = connectionInfo.Point.position - connectionOffset;
                    
                    // Place the room
                    RoomInstance newRoom = PlaceRoom(prefab, roomPosition, targetRotation);
                    if (newRoom == null || newRoom.GameObject == null)
                    {
                        Debug.LogError("Failed to place new room!");
                        continue;
                    }
                    
                    // Mark both connection points as used
                    connectionInfo.Room.UsedConnectionPoints.Add(connectionInfo.Point);
                    newRoom.UsedConnectionPoints.Add(newConnection);
                    
                    // Remove the used connection point from the pool
                    availableConnectionPoints.RemoveAt(i);
                    
                    // Add ALL of the new room's connection points to the pool
                    foreach (Transform connection in prefab.ConnectionPoints)
                    {
                        if (connection != null && connection != newConnection) // Don't add the one we just used
                        {
                            // Transform the connection point to world space
                            Vector3 worldPos = newRoom.GameObject.transform.TransformPoint(connection.localPosition);
                            Transform worldSpaceConnection = new GameObject($"WorldSpace_{connection.name}").transform;
                            worldSpaceConnection.position = worldPos;
                            worldSpaceConnection.rotation = newRoom.GameObject.transform.rotation * connection.rotation;
                            worldSpaceConnection.SetParent(newRoom.GameObject.transform); // Parent to room for proper cleanup
                            
                            availableConnectionPoints.Add(new ConnectionPointInfo(worldSpaceConnection, newRoom));
                            Debug.Log($"Added new connection point {connection.name} from room {newRoom.GameObject.name} to pool at position {worldPos}");
                        }
                    }
                    
                    // Create door between connection points
                    CreateDoor(connectionInfo.Point, newConnection);
                    
                    // Add to placed rooms
                    placedRooms.Add(newRoom);
                    
                    Debug.Log($"Successfully placed room {newRoom.GameObject.name} using connection points: {connectionInfo.Point.name} -> {newConnection.name} at position {roomPosition}");
                    Debug.Log($"Available connection points in pool: {availableConnectionPoints.Count}");
                    return true;
                }
            }
        }
        finally
        {
            // Clean up temporary object
            if (tempObj != null)
            {
                Destroy(tempObj);
            }
        }
        
        Debug.Log("Could not find valid connection point to place room");
        return false;
    }
    
    private void PlaceExitRoom()
    {
        if (availableConnectionPoints.Count == 0)
        {
            Debug.LogError("No available connection points for exit room!");
            return;
        }
        
        // Find furthest connection point from entrance
        ConnectionPointInfo furthestConnection = availableConnectionPoints
            .OrderByDescending(c => Vector3.Distance(c.Point.position, entranceInstance.Position))
            .First();
            
        Debug.Log($"Placing exit room near connection point: {furthestConnection.Point.name}");
        
        Transform exitConnection = exitRoom.ConnectionPoints[0];
        if (exitConnection == null)
        {
            Debug.LogError("No connection points found in exit room!");
            return;
        }
        
        // Skip if this connection point is already used
        if (furthestConnection.Room.UsedConnectionPoints.Contains(furthestConnection.Point))
        {
            Debug.LogError($"Cannot use connection point {furthestConnection.Point.name} - already used!");
            return;
        }
        
        // Calculate rotation first - we need this for correct position calculation
        Quaternion rotation = Quaternion.FromToRotation(exitConnection.forward, -furthestConnection.Point.forward);
        
        // Create a temporary object to help us calculate the correct position
        GameObject tempObj = new GameObject("TempCalculation");
        tempObj.transform.position = Vector3.zero;
        tempObj.transform.rotation = rotation;
        
        // Get the connection point's position relative to the room's center after rotation
        Vector3 connectionOffset = tempObj.transform.TransformPoint(exitConnection.localPosition);
        Destroy(tempObj);
        
        // Calculate the final position by subtracting the rotated connection offset from the target point
        Vector3 roomPosition = furthestConnection.Point.position - connectionOffset;
        
        exitInstance = PlaceRoom(exitRoom, roomPosition, rotation);
        placedRooms.Add(exitInstance);
        
        // Mark both connection points as used
        furthestConnection.Room.UsedConnectionPoints.Add(furthestConnection.Point);
        exitInstance.UsedConnectionPoints.Add(exitConnection);
        
        // Remove the used connection point from the pool
        availableConnectionPoints.Remove(furthestConnection);
        
        // Create door between rooms
        CreateDoor(furthestConnection.Point, exitConnection);
        
        Debug.Log($"Successfully placed exit room at position {roomPosition}");
    }
    
    private void CreateDoor(Transform pointA, Transform pointB)
    {
        if (doorPrefab == null)
        {
            Debug.LogError("Door prefab is not assigned!");
            return;
        }

        // Calculate door position and rotation
        Vector3 doorPosition = (pointA.position + pointB.position) / 2f;
        Quaternion doorRotation = Quaternion.LookRotation(pointB.position - pointA.position);
        
        // Create door
        GameObject door = Instantiate(doorPrefab, doorPosition, doorRotation, transform);
        door.name = $"Door_{pointA.name}_{pointB.name}";
        
        Debug.Log($"Created door between {pointA.name} and {pointB.name}");
    }
}

public class RoomInstance
{
    public RoomPrefab Prefab { get; private set; }
    public GameObject GameObject { get; private set; }
    public Vector3 Position => GameObject.transform.position;
    public HashSet<Transform> UsedConnectionPoints { get; private set; }
    
    public RoomInstance(RoomPrefab prefab, GameObject gameObject)
    {
        Prefab = prefab;
        GameObject = gameObject;
        UsedConnectionPoints = new HashSet<Transform>();
    }
} 