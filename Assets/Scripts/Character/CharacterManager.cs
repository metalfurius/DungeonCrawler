using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake() {
        DontDestroyOnLoad(this);
        characterController=GetComponent<CharacterController>();
        characterNetworkManager=GetComponent<CharacterNetworkManager>();
    }
    protected virtual void Update(){
        if(IsOwner){
            characterNetworkManager.networkPosition.Value=transform.position;
            characterNetworkManager.netWorkRotation.Value=transform.rotation;
        }
        else
        {
            transform.position=Vector3.SmoothDamp(transform.position,
            characterNetworkManager.networkPosition.Value,
            ref characterNetworkManager.networkPositionVelocity,
            characterNetworkManager.networkPositionSmoothTime);

            transform.rotation=Quaternion.Slerp(transform.rotation,
            characterNetworkManager.netWorkRotation.Value,
            characterNetworkManager.networkRotationSmoothTime);
        }
    }
}
