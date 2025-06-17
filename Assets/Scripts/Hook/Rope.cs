using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Transform hookTransform;

    [Header("References")]
    public Hook hook;
    public Transform startPoint;

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
        float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float distance = chargePercent * maxDistance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(10f); // 화면 기준 10f 앞

        Vector3 direction = (targetPoint - startPoint.position).normalized;

        hook.Throw(direction, distance);
    }
}
