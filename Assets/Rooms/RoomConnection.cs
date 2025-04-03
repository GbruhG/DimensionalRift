using UnityEngine;

public class RoomConnection : MonoBehaviour
{
    [Header("Connection Settings")]
    [SerializeField] private Transform doorPrefab;
    [SerializeField] private float doorWidth = 2f;
    [SerializeField] private float doorHeight = 3f;
    
    [Header("Connection Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    
    private GameObject doorInstance;
    
    public void CreateConnection(Transform pointA, Transform pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        
        // Calculate door position and rotation
        Vector3 doorPosition = (pointA.position + pointB.position) / 2f;
        Vector3 doorDirection = (pointB.position - pointA.position).normalized;
        Quaternion doorRotation = Quaternion.LookRotation(doorDirection);
        
        // Create door
        if (doorPrefab != null)
        {
            doorInstance = Instantiate(doorPrefab.gameObject, doorPosition, doorRotation, transform);
            doorInstance.transform.localScale = new Vector3(doorWidth, doorHeight, 1f);
        }
        
        // Create doorway (empty space)
        GameObject doorway = new GameObject("Doorway");
        doorway.transform.SetParent(transform);
        doorway.transform.position = doorPosition;
        doorway.transform.rotation = doorRotation;
        
        // Add collider to prevent player from walking through walls
        BoxCollider collider = doorway.AddComponent<BoxCollider>();
        collider.size = new Vector3(doorWidth, doorHeight, 0.1f);
        collider.isTrigger = true;
    }
    
    public void RemoveConnection()
    {
        if (doorInstance != null)
        {
            Destroy(doorInstance);
            doorInstance = null;
        }
        
        // Remove doorway
        foreach (Transform child in transform)
        {
            if (child.name == "Doorway")
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }
} 