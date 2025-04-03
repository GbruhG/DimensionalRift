using UnityEngine;
using UnityEditor;

public class DoorPrefabSetup : EditorWindow
{
    [MenuItem("Tools/Room Generator/Setup Door Prefab")]
    public static void SetupDoorPrefab()
    {
        // Create the base door object
        GameObject doorObject = new GameObject("New Door");
        
        // Create door frame
        GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frame.name = "Frame";
        frame.transform.SetParent(doorObject.transform);
        frame.transform.localPosition = Vector3.zero;
        frame.transform.localScale = new Vector3(2f, 3f, 0.1f);
        
        // Create door panel
        GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        panel.name = "Panel";
        panel.transform.SetParent(doorObject.transform);
        panel.transform.localPosition = new Vector3(0, 0, 0.05f);
        panel.transform.localScale = new Vector3(1.8f, 2.8f, 0.05f);
        
        // Add collider
        BoxCollider collider = doorObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(2f, 3f, 0.1f);
        collider.isTrigger = true;
        
        // Add door script
        Door door = doorObject.AddComponent<Door>();
        
        // Select the door object in the hierarchy
        Selection.activeGameObject = doorObject;
        
        Debug.Log("Door prefab setup complete! Configure the Door component in the inspector.");
    }
}

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isAnimating = false;
    
    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        
        if (isOpen)
        {
            transform.rotation = openRotation;
        }
    }
    
    public void ToggleDoor()
    {
        if (isLocked) return;
        
        isOpen = !isOpen;
        isAnimating = true;
    }
    
    private void Update()
    {
        if (isAnimating)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isAnimating = false;
            }
        }
    }
    
    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }
} 