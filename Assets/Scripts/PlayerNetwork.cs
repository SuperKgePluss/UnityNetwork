using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {
    [SerializeField] Transform spawnedObjectPrefab;
    Transform spawnObjectTransform;

    NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 56,
            _bool = true,
        },
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    public struct MyCustomData : INetworkSerializable {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }

    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + ", " + newValue._int + ", " + newValue._bool + "; " + newValue.message);
        };
    }

    private void Update() {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T)) {
            spawnObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            
            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } }) ;

            /*randomNumber.Value = new MyCustomData {
                _int = 10,
                _bool = false,
                message = "APPPPPP",
            };*/
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            Destroy(spawnObjectTransform.gameObject);
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    void TestServerRpc(ServerRpcParams serverRpcParams) {
        Debug.Log("TestServerRpc " + OwnerClientId + ", " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    void TestClientRpc(ClientRpcParams clientRpcParams) {
        Debug.Log("TestClientRpc");
    }
}
