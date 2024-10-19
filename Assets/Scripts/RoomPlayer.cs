using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{
    PhotonView pv;
    public Camera mainCamera;
    public GameObject networkGo;
    public TextMeshPro nameText;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        networkGo.SetActive(!pv.IsMine);
        mainCamera.enabled = pv.IsMine;
        if (!pv.IsMine)
            nameText.text = pv.Owner.NickName;
    }
}
