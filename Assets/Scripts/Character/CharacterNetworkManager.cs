using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition=
    new NetworkVariable<Vector3>(Vector3.zero,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public NetworkVariable<Quaternion> netWorkRotation=
    new NetworkVariable<Quaternion>(Quaternion.identity,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public NetworkVariable<Vector3> networkScale=
    new NetworkVariable<Vector3>(new Vector3(0,1.65f,0),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
}
