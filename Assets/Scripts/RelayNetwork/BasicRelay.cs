using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class BasicRelay : MonoBehaviour {
    public static BasicRelay Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public async /*void*/ Task<string> CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            //joinCodeText.text = joinCode;
            Debug.Log(joinCode);

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            //        allocation.RelayServer.IpV4,
            //        (ushort)allocation.RelayServer.Port,
            //        allocation.AllocationIdBytes,
            //        allocation.Key,
            //        allocation.ConnectionData
            //    );

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            return joinCode;
        } catch (RelayServiceException e) {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode) {
        //string joinCode = inputFieldText.text.Substring(0, 6);
        try {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            //        joinAllocation.RelayServer.IpV4,
            //        (ushort)joinAllocation.RelayServer.Port,
            //        joinAllocation.AllocationIdBytes,
            //        joinAllocation.Key,
            //        joinAllocation.ConnectionData,
            //        joinAllocation.HostConnectionData
            //    );
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
}
