using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    public float verticalMovement,horizontalMovement,moveAmount;
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed=2;
    [SerializeField] float runningSpeed=5;
    [SerializeField] float rotationSpeed=15;

    protected override void Awake() {
        base.Awake();
        player=GetComponent<PlayerManager>();
    }

    public void HandleAllMovement(){
        HandleGroundedMovement();
        HandleRotation();
    }
    private void GetVerticalAndHorizontalInputs(){
        verticalMovement=PlayerInputManager.instance.verticalInput;
        horizontalMovement=PlayerInputManager.instance.horizontalInput;
    }
    private void HandleGroundedMovement(){
        GetVerticalAndHorizontalInputs();
        moveDirection=PlayerCamera.instance.transform.forward*verticalMovement;
        moveDirection=moveDirection+PlayerCamera.instance.transform.right*horizontalMovement;
        moveDirection.y=0;
        moveDirection.Normalize();
        if (PlayerInputManager.instance.moveAmount>0.5f)
        {
            player.characterController.Move(runningSpeed*Time.deltaTime*moveDirection);
        }
        else if (PlayerInputManager.instance.moveAmount<=0.5f)
        {
            player.characterController.Move(walkingSpeed*Time.deltaTime*moveDirection);
        }
    }
    private void HandleRotation(){
        Vector3 targetRotationDirection=Vector3.zero;
        targetRotationDirection=PlayerCamera.instance.cameraObject.transform.forward*verticalMovement;
        targetRotationDirection=targetRotationDirection+PlayerCamera.instance.cameraObject.transform.right*horizontalMovement;
        targetRotationDirection.y=0;
        targetRotationDirection.Normalize();
        if(targetRotationDirection==Vector3.zero){
            targetRotationDirection=transform.forward;
        }
        Quaternion newRotation=Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation=Quaternion.Slerp(transform.rotation,newRotation,rotationSpeed*Time.deltaTime);
        transform.rotation=targetRotation;
    }
}
