// Create a new C# script called "TestRoomGenerator.cs"
using UnityEngine;

public class TestRoomGenerator : MonoBehaviour
{
    [Header("Room Parameters")]
    [SerializeField] private float roomWidth = 20f;
    [SerializeField] private float roomLength = 20f;
    [SerializeField] private float roomHeight = 5f;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material floorMaterial;
    
    private void Start()
    {
        GenerateRoom();
    }
    
    private void GenerateRoom()
    {
        // Create floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(roomWidth/10f, 1f, roomLength/10f);
        floor.transform.position = Vector3.zero;
        if (floorMaterial) floor.GetComponent<Renderer>().material = floorMaterial;
        
        // Create walls
        CreateWall(new Vector3(0, roomHeight/2, roomLength/2), new Vector3(roomWidth, roomHeight, 0.1f), "North Wall");
        CreateWall(new Vector3(0, roomHeight/2, -roomLength/2), new Vector3(roomWidth, roomHeight, 0.1f), "South Wall");
        CreateWall(new Vector3(roomWidth/2, roomHeight/2, 0), new Vector3(0.1f, roomHeight, roomLength), "East Wall");
        CreateWall(new Vector3(-roomWidth/2, roomHeight/2, 0), new Vector3(0.1f, roomHeight, roomLength), "West Wall");
        
        // Create ceiling
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.position = new Vector3(0, roomHeight, 0);
        ceiling.transform.localScale = new Vector3(roomWidth, 0.1f, roomLength);
        if (wallMaterial) ceiling.GetComponent<Renderer>().material = wallMaterial;
    }
    
    private void CreateWall(Vector3 position, Vector3 scale, string name)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        if (wallMaterial) wall.GetComponent<Renderer>().material = wallMaterial;
    }
}