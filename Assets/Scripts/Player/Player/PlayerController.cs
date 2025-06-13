using UnityEngine;

// 플레이어의 이동 및 점프 + 수영 부력까지 처리하는 컨트롤러 컴포넌트
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 5f; // 기본 걷기 속도
    [SerializeField] private float runSpeed = 8f; // 달리기 속도
    [SerializeField] private float jumpPower = 8f; // 점프 초기 속도
    [SerializeField] private float gravity = -20f; // 중력 가속도

    [Header("점프 버퍼링")]
    [SerializeField] private float jumpBufferTime = 0.2f; // 점프 유예 시간
    private float jumpBufferCounter = 0f; // 점프 유예 시간 카운터

    [Header("수영 설정")]
    [SerializeField] private float swimSpeedMultiplier = 0.5f; // 수영 시 수평 감속 비율
    [SerializeField] private float swimVerticalSpeed = 2f; // 수영 상승/하강 속도

    private bool isSwimming = false; // 현재 수영 상태 여부

    private CharacterController characterController; // 캐릭터 이동 컨트롤러
    private Vector3 moveInput = Vector3.zero; // 현재 입력 방향
    private float verticalVelocity = 0f; // y축 이동 속도 (점프/낙하/부력 포함)

    private Camera activeCamera; // 현재 참조 중인 카메라

    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); // 캐릭터 컨트롤러 캐싱
    }

    private void Start()
    {
        activeCamera = Camera.main; // 시작 시 메인 카메라 캐싱
    }

    private void Update()
    {
        // 현재 활성화된 카메라를 매 프레임 추적
        Camera mainCam = Camera.main;
        if (mainCam != null && mainCam.enabled)
        {
            activeCamera = mainCam; // MainCamera가 활성 상태일 경우 갱신
        }
        else
        {
            Camera subCam = GameObject.FindWithTag("SubCamera")?.GetComponent<Camera>(); // SubCamera 탐색
            if (subCam != null && subCam.enabled)
                activeCamera = subCam;
        }
    }

    public void ReadMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D 입력
        float v = Input.GetAxisRaw("Vertical");   // W/S 입력

        if (activeCamera == null)
        {
            moveInput = Vector3.zero; // 카메라가 없을 경우 이동 입력 없음 처리
            return;
        }

        // 카메라 기준 방향 설정
        Vector3 camForward = activeCamera.transform.forward;
        Vector3 camRight = activeCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize(); // 정규화하여 수평 벡터 보정
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;
        moveInput = moveDir.sqrMagnitude > 0f ? moveDir.normalized : Vector3.zero; // GC 없는 이동 방향 계산

        if (moveInput.sqrMagnitude > 0.01f)
            transform.forward = moveInput; // 이동 방향으로 회전

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime; // Space 입력 시 점프 버퍼 시작
    }

    public void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // 달리기 여부

        if (isSwimming)
            speed *= swimSpeedMultiplier; // 수영 시 감속 적용

        Vector3 horizontalMove = moveInput * speed; // 수평 이동 계산

        if (isSwimming)
        {
            HandleBuoyancy(); // 수영 중일 경우 부력 적용
        }
        else
        {
            HandleGravityAndJump(); // 지상 이동 중일 경우 중력 및 점프 처리
        }

        Vector3 verticalMove = Vector3.up * verticalVelocity; // 수직 이동 계산
        Vector3 finalMove = horizontalMove + verticalMove; // 최종 이동 벡터

        // 접지 유지 위한 수직 속도 보정
        if (finalMove.y > -0.001f && finalMove.y < 0.001f)
            finalMove.y = -0.001f;

        characterController.Move(finalMove * Time.deltaTime); // 캐릭터 이동 적용
    }

    // 수영 상태에서의 부력 처리
    private void HandleBuoyancy()
    {
        verticalVelocity = 0f; // 수직 속도 초기화

        verticalVelocity += WaterSystem.CalculateBuoyancy(transform.position); // 깊이에 따른 부력 적용

        if (Input.GetKey(KeyCode.Space))
            verticalVelocity += swimVerticalSpeed; // 상승 입력 처리

        if (Input.GetKey(KeyCode.LeftControl))
            verticalVelocity -= swimVerticalSpeed; // 하강 입력 처리

        verticalVelocity = Mathf.Clamp(verticalVelocity, -3f, 3f); // 수직 속도 제한
    }

    // 지상 중력 및 점프 처리
    private void HandleGravityAndJump()
    {
        if (characterController.isGrounded)
        {
            if (IsJumpBuffered())
            {
                verticalVelocity = jumpPower; // 점프 적용
                jumpBufferCounter = 0f;       // 점프 버퍼 초기화
            }
            else if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f; // 접지 유지용 보정
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // 중력 가속도 적용
        }
    }

    public void SetSwimMode(bool isSwim)
    {
        isSwimming = isSwim; // 수영 모드 상태 전환
    }

    public bool HasMovementInput()
    {
        return moveInput.sqrMagnitude > 0.01f; // 이동 입력 여부 확인
    }

    public bool IsJumping()
    {
        return !characterController.isGrounded && verticalVelocity > 0.1f; // 상승 중일 때 점프 간주
    }

    public bool IsFalling()
    {
        return !characterController.isGrounded && verticalVelocity < -0.1f; // 하강 중일 때 낙하 간주
    }

    public bool IsRunning()
    {
        return HasMovementInput() && Input.GetKey(KeyCode.LeftShift); // 입력 + Shift 상태 → 달리기 간주
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded; // 접지 여부 반환
    }

    public bool IsJumpBuffered()
    {
        return jumpBufferCounter > 0f; // 점프 유예 시간 잔여 여부
    }
}
