using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {
    NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + ", RandomNumber: " + randomNumber.Value);
        };
    }

    private void Update() {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T)) {
            randomNumber.Value = Random.Range(0, 100);
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
