﻿using UnityEngine;
using UnityEngine.Rendering;


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

    [Header("참조")]
    [SerializeField] private Player player; // FSM 상태 확인용 Player 참조

    [Header("수중 효과")]
    [SerializeField] private Volume underwaterVolume; // 수중 상태용 볼륨
    [SerializeField] private float waterSurfaceY = 0f; // 수면 기준 Y (WaterSystem에서 가져올 수도 있음)

    [Header("시점 전환 설정")]
    [SerializeField] private GameObject bodyRoot; // 플레이어 외형 루트 (예: Body_010)
    [SerializeField] private bool isFirstPerson = false; // 현재 시점이 1인칭인지 여부

    private bool isSwimming = false; // 현재 수영 상태 여부

    private CharacterController characterController; // 캐릭터 이동 컨트롤러
    private Vector3 moveInput = Vector3.zero; // 현재 입력 방향
    private float verticalVelocity = 0f; // y축 이동 속도 (점프/낙하/부력 포함)

    private Camera activeCamera; // 현재 참조 중인 카메라

    public Volume UnderwaterVolume => underwaterVolume; // 외부 상태에서 접근 가능하도록 프로퍼티 제공

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (player == null)
            player = GetComponent<Player>();

        if (bodyRoot == null)
        {
            Transform found = transform.Find("Body_010"); // 외형 루트 자동 탐색
            if (found != null) bodyRoot = found.gameObject;
        }
    }

    private void Start()
    {
        activeCamera = Camera.main; // 시작 시 메인 카메라 캐싱

        // WaterSystem에서 수면 높이 가져오기 (초기화 한 번만)
        waterSurfaceY = WaterSystem.GetSurfaceY(); // WaterSystem에 해당 메서드가 존재해야 함
    }

    private void Update()
    {
        Camera mainCam = Camera.main; // 메인 카메라 가져오기
        if (mainCam != null && mainCam.enabled)
        {
            activeCamera = mainCam; // 메인 카메라가 활성 상태면 설정
        }
        else
        {
            Camera subCam = GameObject.FindWithTag("SubCamera")?.GetComponent<Camera>(); // SubCamera 검색
            if (subCam != null && subCam.enabled)
                activeCamera = subCam;
        }

        // 플레이어가 수면 아래에 있을 경우 물 속 효과 적용
        if (underwaterVolume != null)
        {
            float playerY = transform.position.y;
            float surfaceY = waterSurfaceY;

            // 수영 중이면서 수면보다 낮으면 수중 효과 적용
            bool shouldApplyUnderwater =
                player.FSM.CurrentStateType == PlayerStateType.Swim &&
                playerY < surfaceY - 0.2f;

            // 상태에 따라 Volume 전환
            underwaterVolume.weight = shouldApplyUnderwater ? 1f : 0f;
        }
    }

    // 시점 전환 시 외형 숨김 처리
    public void SetFirstPersonView(bool enable)
    {
        isFirstPerson = enable;

        if (bodyRoot != null)
        {
            // 기존 Renderer.enabled 방식 대신 → GameObject 비활성화
            SkinnedMeshRenderer[] skinnedMeshes = bodyRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            foreach (var smr in skinnedMeshes)
            {
                smr.gameObject.SetActive(!enable); // 1인칭이면 비활성화, 3인칭이면 활성화
                Debug.Log($"[SkinnedMeshRenderer] {smr.gameObject.name} 활성화: {!enable}");
            }
        }
    }

    public void ReadMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D 입력
        float v = Input.GetAxisRaw("Vertical");   // W/S 입력

        if (activeCamera == null)
        {
            moveInput = Vector3.zero; // 카메라가 없을 경우 입력 없음 처리
            return;
        }

        Vector3 camForward = activeCamera.transform.forward; // 카메라 기준 전방
        Vector3 camRight = activeCamera.transform.right;     // 카메라 기준 우측

        camForward.y = 0f; // 수직 방향 제거
        camRight.y = 0f;

        camForward.Normalize(); // 정규화
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h; // 방향 계산
        moveInput = moveDir.sqrMagnitude > 0f ? moveDir.normalized : Vector3.zero; // GC 없는 이동 방향

        // 마우스 위치 기준 회전 (Ray + Plane)
        Plane groundPlane = new Plane(Vector3.up, transform.position); // 수평 평면 생성
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 발사

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 부드럽게 회전
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime; // Space 입력 시 점프 버퍼 시작
    }

    public void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // 걷기 or 달리기 속도

        if (isSwimming)
            speed *= swimSpeedMultiplier; // 수영 감속 적용

        Vector3 horizontalMove = moveInput * speed; // 수평 이동

        if (isSwimming && player.FSM.CurrentStateType == PlayerStateType.Swim)
        {
            HandleBuoyancy(); // 수영 상태일 때만 부력 적용
        }
        else
        {
            HandleGravityAndJump(); // 일반 중력 + 점프 적용
        }

        Vector3 verticalMove = Vector3.up * verticalVelocity; // 수직 이동
        Vector3 finalMove = horizontalMove + verticalMove; // 최종 이동 벡터

        if (finalMove.y > -0.001f && finalMove.y < 0.001f)
            finalMove.y = -0.001f; // 지면 접촉 유지를 위한 미세 보정

        characterController.Move(finalMove * Time.deltaTime); // 실제 이동 적용
    }

    private void HandleBuoyancy()
    {
        float surfaceY = WaterSystem.GetSurfaceY(); // 수면 y 기준값

        // 수면보다 많이 올라가면 수영 종료 → 점프 상태로 강제 전환
        if (transform.position.y > surfaceY + 0.8f)
        {
            EventBus.PublishVoid("ForceSwimToJump"); // 수영 상태 종료 + 점프 전환
            return; // 이후 부력 처리 중단
        }

        if (Input.GetKey(KeyCode.Space))
        {
            verticalVelocity = swimVerticalSpeed; // 상승
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            verticalVelocity = -swimVerticalSpeed; // 하강
        }
        else
        {
            float buoyancy = WaterSystem.CalculateBuoyancy(transform.position);
            verticalVelocity = Mathf.Lerp(verticalVelocity, buoyancy, Time.deltaTime * 1.5f);
        }

        verticalVelocity = Mathf.Clamp(verticalVelocity, -3f, 3f); // 속도 제한
    }

    private void HandleGravityAndJump()
    {
        if (characterController.isGrounded)
        {
            if (IsJumpBuffered())
            {
                verticalVelocity = jumpPower; // 점프
                jumpBufferCounter = 0f;       // 버퍼 초기화
            }
            else if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f; // 접지 보정
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // 중력 가속도 적용
        }
    }

    public void ForceVerticalVelocity(float velocity)
    {
        verticalVelocity = velocity; // 외부 수직 속도 강제 설정
    }

    public void SetSwimMode(bool isSwim)
    {
        isSwimming = isSwim; // 외부에서 수영 모드 설정
    }

    public bool HasMovementInput()
    {
        return moveInput.sqrMagnitude > 0.01f; // 이동 입력 여부
    }

    public bool IsJumping()
    {
        return !characterController.isGrounded && verticalVelocity > 0.1f; // 상승 중 점프 간주
    }

    public bool IsFalling()
    {
        return !characterController.isGrounded && verticalVelocity < -0.1f; // 하강 중 낙하 간주
    }

    public bool IsRunning()
    {
        return HasMovementInput() && Input.GetKey(KeyCode.LeftShift); // 달리기 판단
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded; // 접지 여부
    }

    public bool IsJumpBuffered()
    {
        return jumpBufferCounter > 0f; // 점프 유예 체크
    }

    public Vector3 GetMoveInput()
    {
        return moveInput; // 현재 입력 방향 외부 제공
    }

    public void SetActiveCamera(Camera cam)
    {
        if (cam != null)
        {
            activeCamera = cam;
        }
    }

    // 외부에서 점프 발동을 명시적으로 호출할 수 있게
    public void DoJump()
    {
        verticalVelocity = jumpPower; // 수직 속도 적용 (점프)
        jumpBufferCounter = 0f;       // 버퍼 초기화
    }
}
