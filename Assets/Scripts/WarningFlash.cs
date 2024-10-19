using UnityEngine;

public class WarningFlash : MonoBehaviour
{
    private GameObject Text;
    private bool isOn = true;

    void Start()
    {
        Text = gameObject; 
        InvokeRepeating("ToggleObject", 1f, 1f);
    }

    void ToggleObject()
    {
        Text.SetActive(isOn);
        isOn = !isOn;
    }
}
