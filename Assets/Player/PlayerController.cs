// Create a new C# script called "PlayerController.cs"
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Components")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Image dimensionEnergyBar;
    private CharacterController characterController;
    
    private float verticalLookRotation;
    private Vector3 playerVelocity;
    private bool isGrounded;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        
        // Lock cursor for first-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        isGrounded = characterController.isGrounded;
        HandleMovement();
        HandleLook();
        HandleJump();
        HandleDimensionShift();
        UpdateDimensionEnergyUI();
    }
    
    private void HandleMovement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * horizontalMove + transform.forward * verticalMove;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        
        // Apply gravity
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
    
    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        
        // Rotate player for horizontal look
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate camera for vertical look
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }
    
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }

    private void HandleDimensionShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Shift key pressed!");
            if (DimensionManager.Instance != null)
            {
                Debug.Log("DimensionManager found, toggling dimension...");
                DimensionManager.Instance.ToggleDimension();
            }
            else
            {
                Debug.LogError("DimensionManager.Instance is null!");
            }
        }
    }

    private void UpdateDimensionEnergyUI()
    {
        if (dimensionEnergyBar != null)
        {
            float energyPercentage = DimensionManager.Instance.GetDimensionEnergy() / DimensionManager.Instance.GetMaxDimensionEnergy();
            dimensionEnergyBar.fillAmount = energyPercentage;
        }
    }
}