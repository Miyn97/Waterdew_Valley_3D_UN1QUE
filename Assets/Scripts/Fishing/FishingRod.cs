using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [Header("References")]
    public Bobber bobber;                // Bobber 오브젝트 참조
    public Transform startPoint;         // 낚시대 끝

    [Header("Casting Settings")]
    public float maxDistance = 10f;
    public float maxChargeTime = 1f;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool isFishing = false;

    private void OnEnable()
    {
        EventBus.SubscribeVoid("FishingExit", FishingExit);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("FishingExit", FishingExit);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // 좌클릭 시작
        if (!isFishing && Input.GetMouseButtonDown(0))
        {
            chargeTimer = 0f;
            isCharging = true;
            EventBus.PublishVoid("StartCasting");
        }

        // 좌클릭 유지중 => 타이머 증가
        if (isCharging && Input.GetMouseButton(0))
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);
        }

        // 좌클릭 떼면 Cast()
        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            EventBus.PublishVoid("StopCasting");
            Cast();
        }
    }

    void Cast()
    {
        isFishing = true;
        float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float distance = chargePercent * maxDistance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(10f); // 화면 기준 10f 앞

        Vector3 direction = (targetPoint - startPoint.position).normalized;

        bobber.Throw(direction, distance);
    }

    private void FishingExit()
    {
        isFishing = false;
    }
}
