// Create a new C# script called "RealityAnchor.cs"
using UnityEngine;

public class RealityAnchor : MonoBehaviour
{
    [Header("Vacuum Settings")]
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private float captureSpeed = 1f;
    [SerializeField] private LineRenderer vacuumBeam;
    [SerializeField] private Transform vacuumNozzle;
    [SerializeField] private ParticleSystem captureEffect;
    
    private Camera playerCamera;
    private bool isCapturing = false;
    private GameObject targetAnomaly;
    private float captureProgress = 0f;
    
    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (vacuumBeam == null)
            Debug.LogWarning("Vacuum beam LineRenderer not assigned!");
        if (vacuumNozzle == null)
            Debug.LogWarning("Vacuum nozzle Transform not assigned!");
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginCapture();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndCapture();
        }
        
        if (isCapturing)
        {
            UpdateCapture();
        }
    }
    
    private void BeginCapture()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxRange))
        {
            if (hit.collider.CompareTag("Anomaly"))
            {
                isCapturing = true;
                targetAnomaly = hit.collider.gameObject;
                captureProgress = 0f;
                
                if (vacuumBeam != null)
                {
                    vacuumBeam.enabled = true;
                    vacuumBeam.SetPosition(0, vacuumNozzle.position);
                }
                
                if (captureEffect != null)
                    captureEffect.Play();
            }
        }
    }
    
    private void UpdateCapture()
    {
        if (targetAnomaly == null)
        {
            EndCapture();
            return;
        }
        
        // Update vacuum beam
        if (vacuumBeam != null)
        {
            vacuumBeam.SetPosition(0, vacuumNozzle.position);
            vacuumBeam.SetPosition(1, targetAnomaly.transform.position);
        }
        
        // Progress capture
        captureProgress += captureSpeed * Time.deltaTime;
        if (captureProgress >= 1.0f)
        {
            CaptureAnomaly();
        }
    }
    
    private void EndCapture()
    {
        isCapturing = false;
        targetAnomaly = null;
        
        if (vacuumBeam != null)
            vacuumBeam.enabled = false;
        
        if (captureEffect != null)
            captureEffect.Stop();
    }
    
    private void CaptureAnomaly()
    {
        Debug.Log("Anomaly captured!");
        
        // Example of what you might do when an anomaly is captured
        if (targetAnomaly != null)
        {
            // Play capture effect
            if (captureEffect != null)
            {
                ParticleSystem effect = Instantiate(captureEffect, targetAnomaly.transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }
            
            // Destroy or disable the anomaly
            Destroy(targetAnomaly);
        }
        
        EndCapture();
    }
}