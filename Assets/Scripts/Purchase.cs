using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class Purchase : MonoBehaviour
{
    [Header("BUY")]
    public int coinsPrice;
    public loginMAIN playfablogin;
    public Button purchaseButton;
    private bool isProcessingPurchase = false;

    private bool hasCosmetic;

    [Header("Cosmetic Details")]
    public string cosmeticItemID = "Straw Hat";

    [Header("Post-Purchase Effects")]
    public GameObject itemPurchasedNotification;
    public GameObject gameObjectToDisable;
    public AudioSource purchaseSound;
    public ParticleSystem purchaseParticles;
    public TextMeshProUGUI itemPurchasedText;

    [Header("Error Handling")]
    public GameObject errorNotification;
    public AudioSource errorSound;

    private void Start()
    {
        CheckPlayerInventory();
        purchaseButton.onClick.AddListener(OnPurchaseButtonPressed);
    }

    public void OnPurchaseButtonPressed()
    {
        if (isProcessingPurchase)
        {
            return;
        }

        if (hasCosmetic)
        {
            PlayErrorSoundAndShowErrorNotification();
            return;
        }

        if (playfablogin.coins >= coinsPrice)
        {
            isProcessingPurchase = true;
            purchaseButton.interactable = false;

            BuyCosmetic();
        }
    }

    public void BuyCosmetic()
    {
        var request = new PurchaseItemRequest
        {
            ItemId = cosmeticItemID,
            VirtualCurrency = playfablogin.CurrencyName,
            Price = coinsPrice
        };

        PlayFabClientAPI.PurchaseItem(request, OnPurchaseItemSuccess, OnError);
    }

    void OnPurchaseItemSuccess(PurchaseItemResult result)
    {
        playfablogin.coins -= coinsPrice;
        playfablogin.GetVirtualCurrencies();
        hasCosmetic = true;

        if (itemPurchasedText != null)
        {
            itemPurchasedText.text = $"{cosmeticItemID}";
        }

        if (itemPurchasedNotification != null)
        {
            itemPurchasedNotification.SetActive(true);
            if (purchaseSound != null)
            {
                purchaseSound.Play();
            }
            if (purchaseParticles != null)
            {
                purchaseParticles.Play();
            }
        }

        if (gameObjectToDisable != null)
        {
            gameObjectToDisable.SetActive(false);
        }

        StartCoroutine(DisablePostPurchaseEffectsAfterDelay(4f));

        purchaseButton.interactable = true;
        isProcessingPurchase = false;
    }

    void OnError(PlayFabError error)
    {
        purchaseButton.interactable = true;
        isProcessingPurchase = false;
    }

    public void CheckPlayerInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        hasCosmetic = false;

        foreach (var item in result.Inventory)
        {
            if (item.ItemId == cosmeticItemID)
            {
                hasCosmetic = true;
                break;
            }
        }
    }

    private IEnumerator DisablePostPurchaseEffectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (itemPurchasedNotification != null)
        {
            itemPurchasedNotification.SetActive(false);
        }
        if (purchaseParticles != null)
        {
            purchaseParticles.Stop();
        }

        if (itemPurchasedText != null)
        {
            itemPurchasedText.gameObject.SetActive(false);
        }

        if (gameObjectToDisable != null)
        {
            gameObjectToDisable.SetActive(true);
        }
    }

    private void PlayErrorSoundAndShowErrorNotification()
    {
        if (errorSound != null)
        {
            errorSound.Play();
        }

        if (errorNotification != null)
        {
            errorNotification.SetActive(true);
            StartCoroutine(HideErrorNotificationAfterDelay(3f));
        }
    }

    private IEnumerator HideErrorNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (errorNotification != null)
        {
            errorNotification.SetActive(false);
        }
    }
}
