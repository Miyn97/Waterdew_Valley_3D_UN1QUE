using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    public GameObject promptPanel;
    public TextMeshProUGUI promptText;

    private void Awake()
    {
        Instance = this;
        promptPanel.SetActive(false);
    }

    public void ShowPrompt(string text)
    {
        promptText.text = text;
        promptPanel.SetActive(true);
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }
}
