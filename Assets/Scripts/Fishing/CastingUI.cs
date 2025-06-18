using UnityEngine;
using UnityEngine.UI;

public class CastingUI : MonoBehaviour
{
    public Image castingFillImage;
    public float maxCastingTime = 1f;

    private float currentTime = 0f;
    private bool isCasting = false;

    private void OnEnable()
    {
        EventBus.SubscribeVoid("StartCasting", StartCasting);
        EventBus.SubscribeVoid("StopCasting", StopCasting);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("StartCasting", StartCasting);
        EventBus.UnsubscribeVoid("StopCasting", StopCasting);
    }

    public void StartCasting()
    {
        isCasting = true;
        currentTime = 0f;
        castingFillImage.fillAmount = 0f;
        castingFillImage.gameObject.SetActive(true);
    }

    public void StopCasting()
    {
        isCasting = false;
        castingFillImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isCasting) return;

        currentTime += Time.deltaTime;
        float fill = Mathf.Clamp01(currentTime / maxCastingTime);
        castingFillImage.fillAmount = fill;
    }
}
