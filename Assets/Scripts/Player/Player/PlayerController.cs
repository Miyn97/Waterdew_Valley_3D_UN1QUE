using UnityEngine;

// 입력 처리 및 이동 + 달리기 + 점프 제어를 담당하는 컴포넌트
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 5f;              // 기본 걷기 속도 설정
    [SerializeField] private float runSpeed = 8f;               // Shift 키 입력 시 적용될 달리기 속도 설정
    [SerializeField] private float jumpPower = 8f;              // 점프 시 y축으로 상승할 초기 속도 값
    [SerializeField] private float gravity = -20f;              // 중력 가속도 값 (음수 방향으로 작용)

    [Header("점프 설정")]
    [SerializeField] private float jumpBufferTime = 0.2f;       // 점프 유예 시간 (버퍼) 설정
    private float jumpBufferCounter = 0f;                       // 현재 프레임 기준으로 남은 점프 버퍼 시간

    private CharacterController characterController;            // Unity 내장 이동 및 충돌 처리를 위한 컴포넌트 참조
    private Vector3 moveInput;                                  // 현재 프레임의 수평 이동 입력 방향 값
    private float verticalVelocity;                             // 수직 방향 이동 속도 (점프 및 중력 처리에 사용)

    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); // 현재 GameObject에 부착된 CharacterController를 캐싱
    }

    public void ReadMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // 좌우 방향키 입력 감지 (A/D)
        float v = Input.GetAxisRaw("Vertical");   // 전후 방향키 입력 감지 (W/S)
        moveInput = new Vector3(h, 0f, v).normalized; // 입력 벡터 정규화

        if (moveInput.sqrMagnitude > 0.01f) // 미세 입력 제외
            transform.forward = moveInput; // 방향에 따라 회전

        if (Input.GetKeyDown(KeyCode.Space)) // Space 키가 눌렸을 경우
            jumpBufferCounter = jumpBufferTime; // 점프 유예 시간 초기화

        if (jumpBufferCounter > 0f) // 점프 유예 시간이 남아있다면
            jumpBufferCounter -= Time.deltaTime; // 감소시킴
    }

    public void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // Shift 여부로 속도 결정
        Vector3 horizontalMove = moveInput * speed; // 수평 이동 벡터 계산

        if (characterController.isGrounded)
        {
            if (IsJumpBuffered()) // 여기에 jumpBuffer 확인 조건 명시
            {
                verticalVelocity = jumpPower;
                jumpBufferCounter = 0f;
            }
            else if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        else // 공중 상태일 경우
        {
            verticalVelocity += gravity * Time.deltaTime; // 중력 누적 적용
        }

        Vector3 verticalMove = Vector3.up * verticalVelocity; // 수직 이동 벡터 계산
        Vector3 finalMove = horizontalMove + verticalMove;    // 최종 이동 벡터 합산

        if (finalMove.y > -0.001f && finalMove.y < 0.001f) // 수직 이동이 너무 작으면
            finalMove.y = -0.001f; // 최소 이동값 강제 적용 (접지 판단 보정)

        characterController.Move(finalMove * Time.deltaTime); // 이동 실행 (시간 보정 포함)
    }

    public bool HasMovementInput()
    {
        return moveInput.sqrMagnitude > 0.01f; // 이동 입력 유무 반환
    }

    public bool IsJumping()
    {
        return !characterController.isGrounded && verticalVelocity > 0.1f; // 공중에서 상승 중이면 점프 상태
    }

    public bool IsFalling()
    {
        return !characterController.isGrounded && verticalVelocity < -0.1f; // 공중에서 하강 중이면 true
    }

    public bool IsRunning()
    {
        return HasMovementInput() && Input.GetKey(KeyCode.LeftShift); // 입력 + Shift 조합일 때만 달리기
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded; // Unity 엔진의 접지 판정 반환
    }

    public bool IsJumpBuffered()
    {
        return jumpBufferCounter > 0f; // 점프 버퍼 유지 여부 반환
    }

}
