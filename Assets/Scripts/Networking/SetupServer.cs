using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class SetupServer : MonoBehaviour
{
    private NetworkManager _networkManager;
    private UnityTransport _transport;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = GetComponent<NetworkManager>();
        _transport = GetComponent<UnityTransport>();

        //_transport.SetRelayServerData("127.0.0.1", 7777, )
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
