using System;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class loginMAIN : MonoBehaviourPunCallbacks
{
    [Header("CURRENCY")]
    public string CurrencyName = "GT";
    public TextMeshProUGUI currencyText;

    public int coins;
    public string PlayFabID;

    private void Start()
    {
        Login();
    }

    private void Login()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = PlayFabSettings.DeviceUniqueIdentifier,
            CreateAccount = true
        }, OnSuccess, OnError);
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account creation!");
        PlayFabID = result.PlayFabId;

        // Set display name if available
        string username = PlayerPrefs.GetString("username");
        UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(updateUserTitleDisplayNameRequest, result2 =>
        {
            Debug.Log("Display Name Changed!");
        }, error =>
        {
            Debug.Log(error.ErrorDetails);
        });

        // Get virtual currency after successful login
        GetVirtualCurrencies();

        // Check player inventory after login
        FindObjectOfType<Purchase>().CheckPlayerInventory();
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        if (result.VirtualCurrency != null && result.VirtualCurrency.ContainsKey(CurrencyName))
        {
            coins = result.VirtualCurrency[CurrencyName];
            Debug.Log("Currency found: " + coins.ToString() + " " + CurrencyName);
            currencyText.text = "CURRENCY: " + coins.ToString() + " " + CurrencyName;
        }
        else
        {
            Debug.Log("Currency not found or result.VirtualCurrency is null.");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }
}
