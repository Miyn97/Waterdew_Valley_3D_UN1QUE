using TMPro;

using UnityEngine;

public class FishUI : MonoBehaviour
{
    [SerializeField] private GameObject biteText;
    [SerializeField] private GameObject successText;
    [SerializeField] private GameObject failText;

    private void OnEnable()
    {
        EventBus.SubscribeVoid("OnBiteText", OnBiteText);
        EventBus.SubscribeVoid("OffBiteText", OffBiteText);
        EventBus.SubscribeVoid("OnSuccessText", OnSuccessText);
        EventBus.SubscribeVoid("OnFailText", OnFailText);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("OnBiteText", OnBiteText);
        EventBus.UnsubscribeVoid("OffBiteText", OffBiteText);
        EventBus.UnsubscribeVoid("OnSuccessText", OnSuccessText);
        EventBus.UnsubscribeVoid("OnFailText", OnFailText);
    }

    private void OnBiteText()
    {
        biteText.SetActive(true);
    }

    private void OffBiteText()
    {
        biteText.SetActive(false);
    }

    private void OnSuccessText()
    {
        successText.SetActive(true);
    }

    private void OffSuccessText()
    {
        successText.SetActive(false);
    }

    private void OnFailText()
    {
        failText.SetActive(true);
    }

    private void OffFailText()
    {
        failText.SetActive(false);
    }
}
