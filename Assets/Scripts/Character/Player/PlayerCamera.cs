using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;

    [Header("Camera Settings")]
    [SerializeField] float sensX=220,sensY=220,playersHeight=1.65f;
    [Header("Camera Values")]
    [SerializeField] float xRotation,yRotation;

    private void Awake() {
        if(instance==null){
            instance=this;
        }else
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    public void HandleAllCameraActions(){
        if(player!=null){
            HandleFollowTarget();
            HandleRotations();
        }
    }
    private void HandleFollowTarget(){
        if(PlayerInputManager.instance.crouchInput){
            playersHeight=0.825f;
        }else{
            playersHeight=1.65f;
        }
        transform.position=player.transform.position+new Vector3(0,playersHeight,0);
    }
    private void HandleRotations(){
        float mouseX=PlayerInputManager.instance.cameraHorizontalInput*Time.deltaTime*sensX;
        float mouseY=PlayerInputManager.instance.cameraVerticalInput*Time.deltaTime*sensY;
        yRotation+=mouseX;
        xRotation-=mouseY;
        xRotation=Mathf.Clamp(xRotation,-70f,70f);

        GetComponentInChildren<Transform>().transform.rotation=Quaternion.Euler(xRotation,yRotation,0);
        player.transform.rotation=Quaternion.Euler(0,yRotation,0);
    }
}
