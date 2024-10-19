using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager _instance;
    public string roomName;
    bool isPressed;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }
    public void StartRoom(string roomName)
    {
        if (isPressed) return;
        this.roomName = roomName;
        if(PhotonNetwork.InLobby)PhotonNetwork.LeaveLobby();
        if(PhotonNetwork.IsConnected)PhotonNetwork.Disconnect();
        isPressed = true;
        Invoke(nameof(SceneChange), .5f); 
    }
    void SceneChange()
    {
        isPressed = false;
        SceneManager.LoadScene("Demo");
    }
}
