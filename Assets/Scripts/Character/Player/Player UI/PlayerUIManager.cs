using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [Header("Network Join")]
    [SerializeField] bool startGameAsClient;
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
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //we shut down the network as host to connect as client
            NetworkManager.Singleton.Shutdown();
            //start the network as client
            NetworkManager.Singleton.StartClient();
        }
    }
}
