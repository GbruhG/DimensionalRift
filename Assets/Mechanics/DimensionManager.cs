using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [Header("Dimension Settings")]
    [SerializeField] private bool isInAlternateDimension = false;
    [SerializeField] private float dimensionShiftDuration = 1f;
    [SerializeField] private float dimensionEnergyDrainRate = 10f;
    [SerializeField] private float dimensionEnergyRegenRate = 5f;
    [SerializeField] private float maxDimensionEnergy = 100f;
    [SerializeField] private float minDimensionEnergy = 20f;

    [Header("Visual Effects")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float chromaticAberrationIntensity = 0.5f;
    [SerializeField] private float vignetteIntensity = 0.5f;
    [SerializeField] private float lensDistortionIntensity = 0.5f;
    [SerializeField] private Color alternateDimensionTint = new Color(0.5f, 0.2f, 0.8f, 1f);

    private float currentDimensionEnergy;
    private bool isShifting = false;
    private float shiftTimer = 0f;
    private float currentEffectIntensity = 0f;

    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;
    private Camera mainCamera;

    private HashSet<DimensionObject> dimensionObjects = new HashSet<DimensionObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentDimensionEnergy = maxDimensionEnergy;
        mainCamera = Camera.main;
        InitializePostProcessing();
        
        // Find and register all existing dimension objects
        DimensionObject[] existingObjects = FindObjectsByType<DimensionObject>(FindObjectsSortMode.None);
        foreach (DimensionObject obj in existingObjects)
        {
            RegisterDimensionObject(obj);
        }
    }

    public void RegisterDimensionObject(DimensionObject obj)
    {
        if (obj != null && dimensionObjects.Add(obj))
        {
            Debug.Log($"Registered dimension object: {obj.gameObject.name}. Total objects: {dimensionObjects.Count}");
        }
    }

    public void UnregisterDimensionObject(DimensionObject obj)
    {
        if (obj != null && dimensionObjects.Remove(obj))
        {
            Debug.Log($"Unregistered dimension object: {obj.gameObject.name}. Total objects: {dimensionObjects.Count}");
        }
    }

    private void InitializePostProcessing()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out chromaticAberration);
            postProcessVolume.profile.TryGet(out vignette);
            postProcessVolume.profile.TryGet(out lensDistortion);
            postProcessVolume.profile.TryGet(out colorAdjustments);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isShifting && HasEnoughEnergy())
        {
            StartDimensionShift();
        }

        if (isShifting)
        {
            UpdateDimensionShift();
        }
        else if (isInAlternateDimension)
        {
            DrainDimensionEnergy();
        }
        else
        {
            RegenerateDimensionEnergy();
        }
    }

    private void StartDimensionShift()
    {
        isShifting = true;
        shiftTimer = 0f;
        currentEffectIntensity = 0f;
        Debug.Log("Starting dimension shift...");
        NotifyAllObjects();
    }

    private void UpdateDimensionShift()
    {
        shiftTimer += Time.deltaTime;
        float progress = shiftTimer / dimensionShiftDuration;

        if (progress >= 1f)
        {
            CompleteDimensionShift();
            return;
        }

        // Smooth easing function
        currentEffectIntensity = Mathf.SmoothStep(0f, 1f, progress);
        UpdateVisualEffects(currentEffectIntensity);
    }

    private void CompleteDimensionShift()
    {
        isShifting = false;
        isInAlternateDimension = !isInAlternateDimension;
        currentEffectIntensity = isInAlternateDimension ? 1f : 0f;
        UpdateVisualEffects(currentEffectIntensity);
        Debug.Log($"Dimension shift complete. Now in alternate dimension: {isInAlternateDimension}");
        NotifyAllObjects();
    }

    private void UpdateVisualEffects(float intensity)
    {
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = intensity * chromaticAberrationIntensity;

        if (vignette != null)
            vignette.intensity.value = intensity * vignetteIntensity;

        if (lensDistortion != null)
            lensDistortion.intensity.value = intensity * lensDistortionIntensity;

        if (colorAdjustments != null)
            colorAdjustments.colorFilter.value = Color.Lerp(Color.white, alternateDimensionTint, intensity);
    }

    private void NotifyAllObjects()
    {
        Debug.Log($"Notifying all objects of dimension change. Current dimension: {(isInAlternateDimension ? "Alternate" : "Normal")}");
        Debug.Log($"Found {dimensionObjects.Count} dimension objects to notify");
        
        foreach (DimensionObject obj in dimensionObjects)
        {
            if (obj != null)
            {
                Debug.Log($"Notifying object: {obj.gameObject.name}");
                obj.UpdateVisibility(isInAlternateDimension);
            }
            else
            {
                Debug.LogError("Found null DimensionObject in the list!");
            }
        }
    }

    private void DrainDimensionEnergy()
    {
        currentDimensionEnergy = Mathf.Max(0f, currentDimensionEnergy - dimensionEnergyDrainRate * Time.deltaTime);
        if (currentDimensionEnergy <= 0f)
        {
            StartDimensionShift();
        }
    }

    private void RegenerateDimensionEnergy()
    {
        currentDimensionEnergy = Mathf.Min(maxDimensionEnergy, currentDimensionEnergy + dimensionEnergyRegenRate * Time.deltaTime);
    }

    private bool HasEnoughEnergy()
    {
        return currentDimensionEnergy >= minDimensionEnergy;
    }

    public bool IsInAlternateDimension()
    {
        return isInAlternateDimension;
    }

    public float GetDimensionEnergyPercentage()
    {
        return currentDimensionEnergy / maxDimensionEnergy;
    }

    // Methods needed by PlayerController
    public void ToggleDimension()
    {
        if (!isShifting && HasEnoughEnergy())
        {
            StartDimensionShift();
        }
    }

    public float GetDimensionEnergy()
    {
        return currentDimensionEnergy;
    }

    public float GetMaxDimensionEnergy()
    {
        return maxDimensionEnergy;
    }
} 