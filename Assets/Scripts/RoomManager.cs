using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager _instance;
    public List<NetPlayer> netPlayers = new();
     PhotonView pV;
    public TextMeshProUGUI debugText;
    public GameObject canvasCamera;
    public bool DidTimeout { private set; get; }
    public static readonly RoomOptions s_RoomOptions = new RoomOptions
    {
        MaxPlayers = 10,
        EmptyRoomTtl = 5,
        PublishUserId = true,
    };
    public string roomName;
    void Awake()
    {
        //Assert.AreEqual(1, FindObjectsOfType<RoomManager>().Length);
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        pV = gameObject.AddComponent<PhotonView>();
        //pV.ViewID = 3;
        DontDestroyOnLoad(this);
        Join();
    }
    public void Join()
    {
        debugText.text = "connecting to server";
        //RoomUIManager._instance.pingText.text = " ";
        Leave();
        roomName = LobbyManager._instance.roomName;
        StartCoroutine(DoJoinOrCreateRoom(roomName));
    }
    public void Leave()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.Disconnect();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    public IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #region All Room Settings
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        debugText.text = ("Created Room");
        netPlayers = new();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("username", $"guest{PhotonNetwork.LocalPlayer.UserId}");
        PhotonNetwork.Instantiate("FirstPersonController", transform.position, Quaternion.identity);
        canvasCamera.gameObject.SetActive(false);
        StartCoroutine(SynchroniseGame());
    }
    public void JoinOrCreateRoom(string preferredRoomName)
    {
        StopAllCoroutines();
        const float timeoutSeconds = 15f;
        StartCoroutine(DoCheckTimeout(timeoutSeconds));
        StartCoroutine(DoJoinOrCreateRoom(preferredRoomName));
    }

    IEnumerator DoCheckTimeout(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        if (!PhotonNetwork.InRoom)
        {
            StopAllCoroutines();
            Leave();
        }


    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.LogError("connected to master");
    }

    IEnumerator DoJoinOrCreateRoom(string preferredRoomName)
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.IsConnected)
        {
            debugText.text = "Status : " + "Connecting to server";
            PhotonNetwork.ConnectUsingSettings();
        }

        while (!PhotonNetwork.IsConnectedAndReady)
        {

            yield return null;
        }

        if (!PhotonNetwork.InLobby && PhotonNetwork.NetworkClientState != ClientState.JoiningLobby)
        {
            debugText.text = "Status : " + $"Connecting to server ,state ={PhotonNetwork.NetworkClientState}";
            PhotonNetwork.JoinLobby();

        }

        while (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {

            yield return null;
        }


        if (preferredRoomName != null)
        {
            debugText.text = "Status : " + "Joining or creating Room";
            bool isJoined = PhotonNetwork.JoinOrCreateRoom(preferredRoomName, s_RoomOptions, TypedLobby.Default);
        }
        else
        {
            debugText.text = "Status : " + "Joined Random Room";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Room Created on joining fail");
        PhotonNetwork.CreateRoom(null, s_RoomOptions, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        debugText.text = "Status : " + "Room Created on joining fail";
        PhotonNetwork.CreateRoom(null, s_RoomOptions, TypedLobby.Default);
    }
    #endregion
    #region Synchronization Code
    IEnumerator SynchroniseGame()
    {
        yield return new WaitForSeconds(1);
        if (PhotonNetwork.CountOfPlayers == s_RoomOptions.MaxPlayers)
        {
            Debug.LogError("damm");
        }
        debugText.text = "";
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
            debugText.text = "Ping : " + PhotonNetwork.GetPing().ToString();
        }
    }
    #endregion

}
[System.Serializable]
public class NetPlayer
{
    public string userId;
    public string username;
    public GameObject go;
    public string score;

    public Hashtable hashValues;
    public NetPlayer(string id, string name)
    {
        this.userId = id;
        this.username = name;
        hashValues = new();

    }
}