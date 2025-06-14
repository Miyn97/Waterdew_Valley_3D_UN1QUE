using UnityEngine;

// 카메라 오비트 동작의 공통 로직을 정의하는 베이스 클래스
public class BaseCameraOrbit : MonoBehaviour
{
    [Header("카메라 타겟 설정")]
    [SerializeField] protected Transform target; // 카메라가 따라다닐 대상 (플레이어의 머리 위 빈 오브젝트 등)

    [Header("회전 설정")]
    [SerializeField] protected float sensitivity = 2f; // 마우스 감도
    [SerializeField] protected float minY = -30f;      // 피치 최소 각도
    [SerializeField] protected float maxY = 60f;       // 피치 최대 각도

    [Header("카메라 거리 및 충돌 설정")]
    [SerializeField] protected Vector3 offset = new Vector3(0f, -4.4f, -2.65f); // 기본 오프셋 위치 (3인칭 기준)
    [SerializeField] protected float collisionOffset = 0.2f; // 충돌 시 카메라 거리 보정
    [SerializeField] protected LayerMask collisionMask; // 충돌 체크 대상 레이어

    protected float yaw = 0f;   // 좌우 회전 각도
    protected float pitch = 20f; // 상하 회전 각도

    // 커서 잠금 및 숨기기
    protected virtual void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        Cursor.visible = false;                   // 커서 숨김
    }

    // 프레임 종료 시점에 카메라 회전 및 위치 갱신
    protected virtual void LateUpdate()
    {
        if (Time.timeScale == 0f) return;               // 일시정지 시 처리 중단
        if (!GetComponent<Camera>().enabled) return;    // 비활성 카메라는 처리하지 않음

        ReadMouseInput();           // 마우스 입력으로 yaw, pitch 계산
        UpdateCameraPosition();     // 실제 카메라 위치 갱신
    }

    // 마우스 입력값으로 yaw, pitch 각도 계산
    protected void ReadMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X"); // 마우스 X축 이동
        float mouseY = Input.GetAxis("Mouse Y"); // 마우스 Y축 이동

        yaw += mouseX * sensitivity;             // yaw 값 누적
        pitch -= mouseY * sensitivity;           // pitch는 반대 방향
        pitch = Mathf.Clamp(pitch, minY, maxY);  // pitch 제한
    }

    // yaw, pitch 및 충돌 계산을 기반으로 카메라 위치 갱신
    protected void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f); // 마우스 회전 각도 계산

        if (offset == Vector3.zero)
        {
            // 1인칭 카메라: 위치는 타겟과 동일, 회전만 적용
            transform.position = target.position;     // 위치 고정
            transform.rotation = rotation;            // 회전 적용
        }
        else
        {
            // 3인칭 카메라: 회전된 오프셋 위치로 카메라 배치 + 충돌 보정
            Vector3 desiredPosition = target.position + rotation * offset;
            Vector3 direction = desiredPosition - target.position;
            float distance = offset.magnitude;

            if (Physics.Raycast(target.position, direction.normalized, out RaycastHit hit, distance, collisionMask))
            {
                desiredPosition = hit.point - direction.normalized * collisionOffset; // 충돌 보정
            }

            transform.position = desiredPosition; // 위치 적용
            transform.LookAt(target.position);    // 타겟을 바라보도록 회전
        }
    }

}
