using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour {
    Lobby hostLobby;
    float heartbeatTimer;
    string playerName;

    private async void Start() {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Sign in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "CrystalMind" + Random.Range(10, 99);
        Debug.Log(playerName);
    }

    private void Update() {
        HandleLobbyHearthbeat();

        if (Input.GetKeyDown(KeyCode.L)) {
            CreateLobby();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ListLobbies();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            QuickJoinLobby();
            //JoinLobbyByCode();
        }
    }

    async void HandleLobbyHearthbeat() {
        if (hostLobby != null) {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f) {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    async void CreateLobby() {
        try {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions {
                IsPrivate = false,
                Player = GetPlayer()
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;

            PrintPlayer(hostLobby);
            Debug.Log("Create Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    async void ListLobbies() {
        try {
            QueryLobbiesOptions options = new QueryLobbiesOptions {
                Count = 25,
                Filters = new System.Collections.Generic.List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new System.Collections.Generic.List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results) {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    async void JoinLobbyByCode(string lobbyCode) {
        try {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions {
                Player = GetPlayer()
            };
            //QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            Debug.Log("Joined lobby with code " + lobbyCode);

            PrintPlayer(joinedLobby);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    async void QuickJoinLobby() {
        try {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    Player GetPlayer() {
        return new Player {
            Data = new System.Collections.Generic.Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    void PrintPlayer(Lobby lobby) {
        Debug.Log("Players in Lobby " + lobby.Name);
        foreach (Player player in lobby.Players) {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }
}
