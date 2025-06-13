using UnityEngine;

// 플레이어의 이동 및 점프 처리를 담당하는 컴포넌트
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 5f; // 기본 걷기 속도 설정
    [SerializeField] private float runSpeed = 8f; // Shift 키 입력 시 적용될 달리기 속도 설정
    [SerializeField] private float jumpPower = 8f; // 점프 시 상승 초기 속도
    [SerializeField] private float gravity = -20f; // 중력 가속도 설정 (음수)

    [Header("점프 설정")]
    [SerializeField] private float jumpBufferTime = 0.2f; // 점프 유예 시간 설정 (0.2초)
    private float jumpBufferCounter = 0f; // 현재 프레임 기준 남은 점프 버퍼 시간

    [Header("수영 설정")]
    [SerializeField] private float swimSpeedMultiplier = 0.5f; // 수영 시 이동 속도 감속 비율
    [SerializeField] private float swimVerticalSpeed = 2f; // 수영 시 수직 이동 속도

    private bool isSwimming = false; // 현재 수영 상태인지 여부 저장

    private CharacterController characterController; // 충돌 및 이동 제어용 CharacterController
    private Vector3 moveInput = Vector3.zero; // 매 프레임 수평 입력 방향을 저장
    private float verticalVelocity = 0f; // 수직 이동 속도 저장용 (점프/낙하/수영)

    private Camera activeCamera; // 현재 이동 방향 기준이 되는 카메라 캐싱용 참조

    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); // CharacterController 컴포넌트 캐싱
    }

    private void Start()
    {
        activeCamera = Camera.main; // 초기 활성 카메라는 MainCamera로 설정
    }

    private void Update()
    {
        // 현재 활성화된 카메라를 매 프레임 기준으로 감지하여 교체
        Camera mainCam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>(); // MainCamera 탐색
        Camera subCam = GameObject.FindWithTag("SubCamera")?.GetComponent<Camera>(); // SubCamera 탐색

        // 둘 중 활성화된 카메라로 activeCamera 갱신
        if (mainCam != null && mainCam.enabled)
            activeCamera = mainCam;
        else if (subCam != null && subCam.enabled)
            activeCamera = subCam;
    }

    public void ReadMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D 입력 감지
        float v = Input.GetAxisRaw("Vertical");   // W/S 입력 감지

        if (activeCamera == null)
        {
            moveInput = Vector3.zero; // 카메라가 없으면 이동 없음 처리
            return;
        }

        Vector3 camForward = activeCamera.transform.forward; // 카메라 기준 전방 벡터
        Vector3 camRight = activeCamera.transform.right;     // 카메라 기준 우측 벡터

        camForward.y = 0f; // 수직 방향 제거
        camRight.y = 0f;
        camForward.Normalize(); // 정규화 (길이 1)
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h; // 방향 계산: 카메라 기준 forward/back + right/left
        moveInput = moveDir.normalized; // 방향 벡터를 정규화하여 캐싱

        if (moveInput.sqrMagnitude > 0.01f)
            transform.forward = moveInput; // 캐릭터가 이동 방향을 바라보도록 회전

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime; // 점프 유예 시간 시작
    }

    public void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // 기본 이동 속도

        if (isSwimming) // 수영 중이면 이동 속도 감속 적용
            speed *= swimSpeedMultiplier;

        Vector3 horizontalMove = moveInput * speed; // 수평 이동 벡터 계산

        if (isSwimming)
        {
            // 수영 중에는 중력 무시하고 직접 수직 이동 처리
            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = swimVerticalSpeed; // 위로 상승
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                verticalVelocity = -swimVerticalSpeed; // 아래로 하강
            }
            else
            {
                verticalVelocity = 0f; // 정지 상태 (둥둥 떠있는 느낌)
            }
        }
        else
        {
            // 일반 중력 처리 로직
            if (characterController.isGrounded)
            {
                if (IsJumpBuffered()) // 점프 유예 시간 존재 시
                {
                    verticalVelocity = jumpPower; // 점프 속도 적용
                    jumpBufferCounter = 0f;       // 점프 요청 초기화
                }
                else if (verticalVelocity < 0f)
                {
                    verticalVelocity = -2f; // 접지 유지용 미세 하향 속도 적용
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime; // 공중 낙하 시 중력 적용
            }
        }

        Vector3 verticalMove = Vector3.up * verticalVelocity; // 수직 이동 벡터 계산
        Vector3 finalMove = horizontalMove + verticalMove; // 전체 이동 벡터 합산

        if (finalMove.y > -0.001f && finalMove.y < 0.001f)
            finalMove.y = -0.001f; // 접지 판정이 끊기지 않도록 최소 y값 보정

        characterController.Move(finalMove * Time.deltaTime); // 실제 이동 실행 (시간 보정 포함)
    }

    public void SetSwimMode(bool isSwim)
    {
        isSwimming = isSwim; // FSM에서 수영 상태 전환 시 호출됨
    }

    public bool HasMovementInput()
    {
        return moveInput.sqrMagnitude > 0.01f; // 방향 입력이 존재하는지 반환
    }

    public bool IsJumping()
    {
        return !characterController.isGrounded && verticalVelocity > 0.1f; // 상승 중이면 점프 상태로 간주
    }

    public bool IsFalling()
    {
        return !characterController.isGrounded && verticalVelocity < -0.1f; // 하강 중이면 낙하 상태
    }

    public bool IsRunning()
    {
        return HasMovementInput() && Input.GetKey(KeyCode.LeftShift); // 이동 + Shift 입력 시 달리기 상태
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded; // CharacterController의 접지 상태 반환
    }

    public bool IsJumpBuffered()
    {
        return jumpBufferCounter > 0f; // 점프 유예 시간 잔여 여부
    }
}
