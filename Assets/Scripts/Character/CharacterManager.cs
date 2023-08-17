using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake() {
        DontDestroyOnLoad(this);
        characterNetworkManager=GetComponent<CharacterNetworkManager>();
    }
    protected virtual void Update(){
        if(IsOwner){
            characterNetworkManager.networkPosition.Value=transform.position;
            characterNetworkManager.netWorkRotation.Value=transform.rotation;
            characterNetworkManager.networkScale.Value=transform.localScale;
        }
        else
        {
            transform.position=characterNetworkManager.networkPosition.Value;
            transform.rotation=characterNetworkManager.netWorkRotation.Value;
            transform.localScale=characterNetworkManager.networkScale.Value;

            /*transform.position=Vector3.SmoothDamp(transform.position,
            characterNetworkManager.networkPosition.Value,
            ref characterNetworkManager.networkPositionVelocity,
            characterNetworkManager.networkPositionSmoothTime);

            transform.rotation=Quaternion.Slerp(transform.rotation,
            characterNetworkManager.netWorkRotation.Value,
            characterNetworkManager.networkRotationSmoothTime);*/
        }
    }
    protected virtual void LateUpdate() {
        
    }
}
