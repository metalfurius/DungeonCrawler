using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerControls playerControls;
    KeyCode jumpKey=KeyCode.Space;
    KeyCode crouchKey=KeyCode.LeftControl;
    [Header("Player Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput, horizontalInput, moveAmount;
    public bool jumpInput;
    public bool crouchInput;
    [Header("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput, cameraHorizontalInput;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
            Cursor.lockState=CursorLockMode.Locked;
            Cursor.visible=false;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void OnApplicationFocus(bool focusStatus) {
        if(enabled){
            if(focusStatus){
                playerControls.Enable();
            }
            else{
                playerControls.Disable();
            }
        }
    }
    private void Update() {
        HandleMovementInput();
        HandleCameraMovementInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        jumpInput=Input.GetKey(jumpKey);
        crouchInput=Input.GetKey(crouchKey);
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }
    private void HandleCameraMovementInput(){
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
        
    }
}
