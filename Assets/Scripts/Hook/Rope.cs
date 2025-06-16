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
        // 0~1 범위로 캐스팅 퍼센트
        float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float distance = chargePercent * maxDistance;

        // 마우스 위치 기준으로 방향 계산 (월드에서 마우스 클릭 지점)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, startPoint.position); // 수평면

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = (hitPoint - startPoint.position).normalized;

            // Hook에 던지라고 요청 (방향 + 거리)
            hook.Throw(direction, distance);
        }
    }
}
