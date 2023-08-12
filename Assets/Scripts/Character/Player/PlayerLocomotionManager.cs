using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public float verticalMovement,horizontalMovement;
    private Vector3 moveDirection;
    Rigidbody rb;
    [SerializeField] float runningSpeed=50;

    protected override void Awake() {
        base.Awake();
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation=true;
    }

    public void HandleAllMovement(){
        HandleGroundedMovement();
    }
    private void HandleGroundedMovement(){
        verticalMovement=PlayerInputManager.instance.verticalInput;
        horizontalMovement=PlayerInputManager.instance.horizontalInput;
        moveDirection=PlayerCamera.instance.transform.forward*verticalMovement+PlayerCamera.instance.transform.right*horizontalMovement;
        moveDirection.y=0f;
        moveDirection.Normalize();
        rb.AddForce(moveDirection*runningSpeed*10f*Time.deltaTime,ForceMode.Force);
    }
}
