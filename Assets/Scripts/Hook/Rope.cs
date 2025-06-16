using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform hookTransform;

    private LineRenderer line;

    [Header("References")]
    public Hook hook;
    public Transform startPoint; // == handTransform

    [Header("Casting Settings")]
    public float maxDistance = 10f;
    public float maxChargeTime = 1f;

    private float chargeTimer = 0f;
    private bool isCharging = false;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.useWorldSpace = true;
    }

    void Update()
    {
        DrawLine();
        HandleInput();
    }

    private void DrawLine()
    {
        if (handTransform == null || hookTransform == null)
            return;

        // 항상 손과 훅 사이를 선으로 그림(로프)
        line.SetPosition(0, handTransform.position);
        line.SetPosition(1, hookTransform.position);
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
