// Create a new C# script called "TestAnomaly.cs"
using UnityEngine;

public class TestAnomaly : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.2f;
    [SerializeField] private Color anomalyColor = Color.magenta;
    
    private Vector3 originalScale;
    private Material material;
    
    private void Start()
    {
        originalScale = transform.localScale;
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = new Material(renderer.material);
            material.color = anomalyColor;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", anomalyColor * 2f);
            renderer.material = material;
        }
        
        // Add glow effect if using URP
        // Add a Light component for non-URP projects
        GameObject glowLight = new GameObject("AnomalyGlow");
        glowLight.transform.parent = transform;
        glowLight.transform.localPosition = Vector3.zero;
        Light light = glowLight.AddComponent<Light>();
        light.color = anomalyColor;
        light.range = 5f;
        light.intensity = 1.5f;
    }
    
    private void Update()
    {
        // Pulse effect
        float pulse = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * pulse;
        
        // Slow rotation
        transform.Rotate(Vector3.up * Time.deltaTime * 15f);
    }
}