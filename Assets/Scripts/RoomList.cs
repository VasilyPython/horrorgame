using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    public List<RoomButtons> roomButtons;

    private void Start()
    {
        StartCoroutine(JoinLobby());
        RoomOff();
    }

    void RefreshRoomList(List<RoomInfo> roomList)
    {
        // Set the number of buttons to the number of rooms (or buttons, whichever is smaller)
        int roomCount = Mathf.Min(roomList.Count, roomButtons.Count);
        
        // Activate the buttons based on the number of rooms available
        for (int i = 0; i < roomButtons.Count; i++)
        {
            roomButtons[i].button.onClick.RemoveAllListeners(); // Clear previous listeners
            
            if (i < roomCount) // Only update buttons if there are rooms to display
            {
                int x = i; // Capture the index for use in the listener
                roomButtons[i].button.onClick.AddListener(() =>
                {
                    LobbyManager._instance.StartRoom(roomList[x].Name);
                });
                roomButtons[i].text.gameObject.SetActive(true);
                roomButtons[i].button.gameObject.SetActive(true);
                roomButtons[i].text.text = roomList[x].Name; // Use roomList[x] instead of roomList[i]
            }
            else // Hide buttons that do not have a corresponding room
            {
                roomButtons[i].text.gameObject.SetActive(false);
                roomButtons[i].button.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator JoinLobby()
    {
        Leave();
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Status : " + "Connecting to server");
            PhotonNetwork.ConnectUsingSettings();
        }

        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        if (!PhotonNetwork.InLobby && PhotonNetwork.NetworkClientState != ClientState.JoiningLobby)
        {
            Debug.LogError("Status : " + $"Connecting to server, state = {PhotonNetwork.NetworkClientState}");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.LogError("Connected to master");
        TypedLobbyInfo info = new TypedLobbyInfo();
        info.Type = LobbyType.Default;
        PhotonNetwork.JoinLobby(info);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.LogError("Lobby Joined");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RefreshRoomList(roomList);
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

    void RoomOff()
    {
        foreach (var item in roomButtons)
        {
            item.button.gameObject.SetActive(false);
            item.text.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class RoomButtons
{
    public Button button;
    public TextMeshProUGUI text;
}
