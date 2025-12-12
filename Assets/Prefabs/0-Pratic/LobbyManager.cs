using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using System.Text;
public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{

    public static LobbyManager Instance;

    public NetworkRunner runner;
    public NetworkObject avatar;

    //luu tru nickname cua nguoi choi
    public string nickName;

    //chu phong
    string roomID;
    string passRoom;//luu tru mat khau phong 

    //thanh vien
    public string inputRoomID;
    public string inputPassRoom;

    //Host
    public int countPlayerJoined;
    public int countPlayerReady;
    Dictionary<PlayerRef, NetworkObject> listPlayerJoined = new Dictionary<PlayerRef, NetworkObject>();

    //local player
    public NetworkObject self_networkObject;

    //UI
    public Transform parentAvatar;
    public TextMeshProUGUI text_buttonReady;
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(PressdStartGameButton);
        JoinSessionLobby();
    }


    private void Awake() => Instance = this;

    private void OnEnable()
    {
        runner.RemoveCallbacks(this);
    }

    private void Update()
    {
        //tao phong
        if (Input.GetKeyDown(KeyCode.H)) StartHost();
        if (Input.GetKeyDown(KeyCode.J)) JoinHost();
    }

    async void JoinSessionLobby()
    {
        await runner.JoinSessionLobby(SessionLobby.Shared);
    }

   async void StartHost()
    {
        roomID = UnityEngine.Random.Range(1000, 9999).ToString();
        passRoom = inputPassRoom;

        bool rqPass = string.IsNullOrEmpty(passRoom) ? false : true;

        //thong tin phong
        var sp = new Dictionary<string, SessionProperty>() 
        {
            {"roomMaster", nickName },
            {"requirePassword", rqPass },
            
        };
        var args = new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomID,
            SessionProperties = sp,
            
        };

        await runner.StartGame(args);


    }
    async void JoinHost()
    {
        var args = new StartGameArgs()
        {
           GameMode = GameMode.Client,
           SessionName = inputRoomID,
           ConnectionToken = Encoding.UTF8.GetBytes(inputPassRoom)
        };
    }

    void PressdStartGameButton()
    {
        if (runner.IsServer)
        {
            if(countPlayerReady == countPlayerJoined - 1)
            {
                Debug.Log("start game");
            }
            else
            {
                Debug.Log("none");
            }
        }
        else
        {
            self_networkObject.GetComponent<Avatar>().RpcChangStatus();
        }
    }

    //Callback
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        if(!runner.IsServer) return;

        string incomingPass = "";

        if(token != null && token.Length > 0)
        {
            incomingPass = Encoding.UTF8.GetString(token);
        }

        if(incomingPass == passRoom)
        {
            request.Accept();
        }
        else
        {
            request.Refuse();
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            NetworkObject n = runner.Spawn(avatar, Vector3.zero, Quaternion.identity, player);
            n.GetComponent<Avatar>().RpcSaveNetworkObject();
            listPlayerJoined.Add(player, n);
            countPlayerJoined++;
            SetStartButtonHost();
        }
    }

    void SetStartButtonHost()
    {
        text_buttonReady.text = "StartGame";
        if(countPlayerJoined >= 1)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        if (runner.IsServer)
        {
            runner.Despawn(listPlayerJoined[player]);
            listPlayerJoined.Remove(player);
            countPlayerJoined--;
            SetStartButtonHost(); 
        }

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        foreach(var session in sessionList)
        {
            
        }
    }


    //phuong thuc trieen khai interface INetworkRunnerCallbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    

    

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
}
