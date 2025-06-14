using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [Header("References")]
    public Bobber bobber;                // Bobber 오브젝트 참조
    public Transform castOrigin;         // 낚시대에서 던질 기준 위치

    [Header("Casting Settings")]
    public float maxDistance = 10f;
    public float maxChargeTime = 1f;

    private float chargeTimer = 0f;
    private bool isCharging = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // 좌클릭 시작
        if (Input.GetMouseButtonDown(0))
        {
            chargeTimer = 0f;
            isCharging = true;
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
            Cast();
        }
    }

    void Cast()
    {
        // 0~1 범위로 캐스팅 퍼센트
        float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float distance = chargePercent * maxDistance;

        // 마우스 위치 기준으로 방향 계산 (월드에서 마우스 클릭 지점)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, castOrigin.position); // 수평면

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = (hitPoint - castOrigin.position).normalized;

            // Bobber에 던지라고 요청 (방향 + 거리)
            bobber.Throw(direction, distance);

            Debug.DrawLine(castOrigin.position, castOrigin.position + direction * distance, Color.green, 2f);
        }
    }
}
