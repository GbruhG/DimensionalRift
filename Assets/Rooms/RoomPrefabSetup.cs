using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RoomPrefabSetup : EditorWindow
{
    [MenuItem("Tools/Room Generator/Setup Room Prefab")]
    public static void SetupRoomPrefab()
    {
        // Create the base room object
        GameObject roomObject = new GameObject("New Room");
        
        // Add required components
        RoomPrefab roomPrefab = roomObject.AddComponent<RoomPrefab>();
        
        // Create connection points container
        GameObject connectionPoints = new GameObject("ConnectionPoints");
        connectionPoints.transform.SetParent(roomObject.transform);
        
        // Create spawn points container
        GameObject spawnPoints = new GameObject("SpawnPoints");
        spawnPoints.transform.SetParent(roomObject.transform);
        
        // Create item spawn points
        GameObject itemSpawnPoints = new GameObject("ItemSpawnPoints");
        itemSpawnPoints.transform.SetParent(spawnPoints.transform);
        
        // Create enemy spawn points
        GameObject enemySpawnPoints = new GameObject("EnemySpawnPoints");
        enemySpawnPoints.transform.SetParent(spawnPoints.transform);
        
        // Create room features container
        GameObject features = new GameObject("Features");
        features.transform.SetParent(roomObject.transform);
        
        // Create a basic floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.SetParent(roomObject.transform);
        floor.transform.localPosition = Vector3.zero;
        floor.transform.localScale = new Vector3(1.2f, 1, 1.2f); // Slightly larger than walls
        
        // Create walls container
        GameObject walls = new GameObject("Walls");
        walls.transform.SetParent(roomObject.transform);
        
        // Add walls (using 10x10 room size)
        CreateWall(walls.transform, "North Wall", new Vector3(0, 1.5f, 5f), new Vector3(10f, 3f, 0.1f));
        CreateWall(walls.transform, "South Wall", new Vector3(0, 1.5f, -5f), new Vector3(10f, 3f, 0.1f));
        CreateWall(walls.transform, "East Wall", new Vector3(5f, 1.5f, 0), new Vector3(0.1f, 3f, 10f));
        CreateWall(walls.transform, "West Wall", new Vector3(-5f, 1.5f, 0), new Vector3(0.1f, 3f, 10f));
        
        // Add connection points with visual indicators (slightly inset from walls)
        CreateConnectionPoint(connectionPoints.transform, "North", new Vector3(0, 0, 4.5f), Vector3.forward);
        CreateConnectionPoint(connectionPoints.transform, "South", new Vector3(0, 0, -4.5f), Vector3.back);
        CreateConnectionPoint(connectionPoints.transform, "East", new Vector3(4.5f, 0, 0), Vector3.right);
        CreateConnectionPoint(connectionPoints.transform, "West", new Vector3(-4.5f, 0, 0), Vector3.left);
        
        // Add example spawn points
        CreateSpawnPoint(itemSpawnPoints.transform, "ItemSpawn1", new Vector3(0, 0.5f, 0));
        CreateSpawnPoint(enemySpawnPoints.transform, "EnemySpawn1", new Vector3(2f, 0.5f, 2f));
        
        // Assign connection points to RoomPrefab component
        var connectionPointTransforms = new List<Transform>();
        foreach (Transform child in connectionPoints.transform)
        {
            connectionPointTransforms.Add(child);
        }
        
        // Use SerializedObject to properly set the serialized field
        SerializedObject serializedRoomPrefab = new SerializedObject(roomPrefab);
        SerializedProperty connectionPointsProperty = serializedRoomPrefab.FindProperty("connectionPoints");
        connectionPointsProperty.ClearArray();
        connectionPointsProperty.arraySize = connectionPointTransforms.Count;
        
        for (int i = 0; i < connectionPointTransforms.Count; i++)
        {
            connectionPointsProperty.GetArrayElementAtIndex(i).objectReferenceValue = connectionPointTransforms[i];
        }
        
        serializedRoomPrefab.ApplyModifiedProperties();
        
        // Select the room object in the hierarchy
        Selection.activeGameObject = roomObject;
        
        Debug.Log("Room prefab setup complete! Configure the RoomPrefab component in the inspector.");
    }
    
    private static void CreateWall(Transform parent, string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.SetParent(parent);
        wall.transform.localPosition = position;
        wall.transform.localScale = scale;
    }
    
    private static void CreateConnectionPoint(Transform parent, string name, Vector3 position, Vector3 forward)
    {
        GameObject point = new GameObject(name);
        point.transform.SetParent(parent);
        point.transform.localPosition = position;
        point.transform.forward = forward;
        
        // Add visual indicator (sphere)
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicator.name = "Indicator";
        indicator.transform.SetParent(point.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        // Create custom cone for arrow
        GameObject arrow = CreateCone();
        arrow.name = "Arrow";
        arrow.transform.SetParent(point.transform);
        arrow.transform.localPosition = new Vector3(0, 0, 0.2f);
        arrow.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
        arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        
        // Set materials for visibility
        Renderer indicatorRenderer = indicator.GetComponent<Renderer>();
        Renderer arrowRenderer = arrow.GetComponent<Renderer>();
        
        // Create materials with standard shader
        Material yellowMaterial = new Material(Shader.Find("Standard"));
        yellowMaterial.color = Color.yellow;
        yellowMaterial.EnableKeyword("_EMISSION");
        yellowMaterial.SetColor("_EmissionColor", Color.yellow);
        
        indicatorRenderer.material = yellowMaterial;
        arrowRenderer.material = yellowMaterial;
        
        // Ensure the objects are active
        indicator.SetActive(true);
        arrow.SetActive(true);
        
        // Add a light component to make it more visible in the editor
        Light light = point.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 2f;
        light.intensity = 1f;
        light.color = Color.yellow;
    }
    
    private static GameObject CreateCone()
    {
        GameObject cone = new GameObject("Cone");
        MeshFilter meshFilter = cone.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = cone.AddComponent<MeshRenderer>();
        
        // Create cone mesh
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        
        // Create vertices for a cone
        Vector3[] vertices = new Vector3[6];
        vertices[0] = new Vector3(0, 0, 0); // Base center
        vertices[1] = new Vector3(0.5f, 0, 0); // Base right
        vertices[2] = new Vector3(-0.5f, 0, 0); // Base left
        vertices[3] = new Vector3(0, 1, 0); // Tip
        vertices[4] = new Vector3(0.5f, 0, 0); // Base right (for UV)
        vertices[5] = new Vector3(-0.5f, 0, 0); // Base left (for UV)
        
        // Create triangles
        int[] triangles = new int[]
        {
            0, 1, 3, // Right side
            0, 3, 2, // Left side
            0, 2, 1  // Base
        };
        
        // Create UVs
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0.5f, 0.5f),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0.5f, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };
        
        // Create normals
        Vector3[] normals = new Vector3[]
        {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.right,
            Vector3.left
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        
        return cone;
    }
    
    private static void CreateSpawnPoint(Transform parent, string name, Vector3 position)
    {
        GameObject point = new GameObject(name);
        point.transform.SetParent(parent);
        point.transform.localPosition = position;
        
        // Add visual indicator
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicator.name = "Indicator";
        indicator.transform.SetParent(point.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
        // Set material for visibility
        Renderer renderer = indicator.GetComponent<Renderer>();
        Material blueMaterial = new Material(Shader.Find("Standard"));
        blueMaterial.color = Color.blue;
        renderer.material = blueMaterial;
    }
} 