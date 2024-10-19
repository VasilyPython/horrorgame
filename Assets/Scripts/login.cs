using System;

using Photon.Pun;

using PlayFab;

using PlayFab.ClientModels;

using UnityEngine;

using UnityEngine.SceneManagement;


// Token: 0x020000D1 RID: 209

public class login : MonoBehaviourPunCallbacks

{

    // Token: 0x060003C2 RID: 962 RVA: 0x00018F32 File Offset: 0x00017132

    private void Start()

    {

        this.Login();

    }


    // Token: 0x060003C3 RID: 963 RVA: 0x00018F3A File Offset: 0x0001713A

    private void Login()

    {

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest

        {

            CustomId = PlayFabSettings.DeviceUniqueIdentifier,

            CreateAccount = new bool?(true)

        }, new Action<LoginResult>(this.OnSuccess), new Action<PlayFabError>(this.OnError), null, null);

    }


    // Token: 0x060003C4 RID: 964 RVA: 0x00018F78 File Offset: 0x00017178

    private void OnSuccess(LoginResult result)

    {

        Debug.Log("Successful login/account create!");

        this.PlayFabPlayerLoggedIn();

        this.PlayFabID = result.PlayFabId;

        string @string = PlayerPrefs.GetString("username");

        UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new UpdateUserTitleDisplayNameRequest();

        updateUserTitleDisplayNameRequest.DisplayName = @string;

        PlayFabClientAPI.UpdateUserTitleDisplayName(updateUserTitleDisplayNameRequest, delegate (UpdateUserTitleDisplayNameResult result2)

        {

            Debug.Log("Display Name Changed!");

        }, delegate (PlayFabError error)

        {

            Debug.Log(error.ErrorDetails);

        }, null, null);

    }


    // Token: 0x060003C5 RID: 965 RVA: 0x00018FFD File Offset: 0x000171FD

    private void OnError(PlayFabError error)

    {

        Debug.Log("Error while logging in/creating account!");

        Debug.Log(error.GenerateErrorReport());

    }


    // Token: 0x060003C6 RID: 966 RVA: 0x00019027 File Offset: 0x00017227

    public override void OnConnectedToMaster()

    {

        base.OnConnectedToMaster();

    }


    // Token: 0x060003C7 RID: 967 RVA: 0x0001902F File Offset: 0x0001722F

    public virtual void PlayFabPlayerLoggedIn()

    {

    }


    // Token: 0x04000494 RID: 1172

    public string PlayFabID;

}