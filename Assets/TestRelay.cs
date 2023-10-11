using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI joinCodeText;
    [SerializeField] TextMeshProUGUI inputFieldText;

    async void Start() {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            CreateRelay();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            JoinRelay();
        }
    }

    async void CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCodeText.text = joinCode;
            Debug.Log(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                );

            NetworkManager.Singleton.StartHost();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    async void JoinRelay() {
        string joinCode = inputFieldText.text.Substring(0, 6);
        try {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                );

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
}
