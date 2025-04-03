using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DimensionObject : MonoBehaviour
{
    [SerializeField] private bool visibleInNormalDimension = true;
    [SerializeField] private bool visibleInAlternateDimension = false;
    
    private bool isVisible = true;
    private GameObject visibilityContainer;

    private void Awake()
    {
        // Create a container for visibility control
        visibilityContainer = new GameObject($"{gameObject.name}_VisibilityContainer");
        visibilityContainer.transform.SetParent(transform.parent);
        transform.SetParent(visibilityContainer.transform);
        
        // Register immediately when the object is created
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.RegisterDimensionObject(this);
            Debug.Log($"[{gameObject.name}] Registered in Awake");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] DimensionManager.Instance is null in Awake!");
        }
    }

    private void Start()
    {
        Debug.Log($"[{gameObject.name}] Starting. Normal visible: {visibleInNormalDimension}, Alternate visible: {visibleInAlternateDimension}");
        
        // Ensure we're registered
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.RegisterDimensionObject(this);
            Debug.Log($"[{gameObject.name}] Registered in Start");
        }
        
        // Set initial visibility
        UpdateVisibility(DimensionManager.Instance.IsInAlternateDimension());
    }

    private void OnDestroy()
    {
        // Unregister when the object is destroyed
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.UnregisterDimensionObject(this);
            Debug.Log($"[{gameObject.name}] Unregistered in OnDestroy");
        }
        
        // Clean up the container
        if (visibilityContainer != null)
        {
            Destroy(visibilityContainer);
        }
    }

    public void UpdateVisibility(bool isInAlternateDimension)
    {
        bool shouldBeVisible = (isInAlternateDimension && visibleInAlternateDimension) || 
                              (!isInAlternateDimension && visibleInNormalDimension);

        Debug.Log($"[{gameObject.name}] UpdateVisibility called. Should be visible: {shouldBeVisible}, Currently visible: {isVisible}, In alternate dimension: {isInAlternateDimension}, Normal visible: {visibleInNormalDimension}, Alternate visible: {visibleInAlternateDimension}");

        if (shouldBeVisible != isVisible)
        {
            isVisible = shouldBeVisible;
            visibilityContainer.SetActive(shouldBeVisible);
            Debug.Log($"[{gameObject.name}] Set container active to: {shouldBeVisible}");
        }
    }
} 