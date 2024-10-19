using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using Photon.Realtime;

public class PlayFabManager : MonoBehaviourPunCallbacks
{
    public string PlayFabID;
    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = PlayFabSettings.DeviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");

        PlayFabPlayerLoggedIn();

        PlayFabID = result.PlayFabId;

        string pUsername = PlayerPrefs.GetString("username");

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = pUsername
        }, delegate (UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Display Name Changed!");
        }, delegate (PlayFabError error)
        {
            Debug.Log(error.ErrorDetails);
        });

    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);

        if (error.Error == PlayFabErrorCode.AccountBanned)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(3);

        Debug.Log(error.ErrorDetails);

        Debug.Log(error.GenerateErrorReport());
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public virtual void PlayFabPlayerLoggedIn()
    {

    }

}