using UnityEngine;

// 플레이어 이동 상태 클래스
public class PlayerState_Move : IState
{
    private readonly Player player; // FSM이 조작할 대상 플레이어 참조

    public PlayerState_Move(Player player)
    {
        this.player = player; // 생성자 주입 방식으로 플레이어 인스턴스를 저장
    }

    public void Enter()
    {
        EventBus.SubscribeVoid("OnMoveStop", OnMoveStop); // 이동 멈춤 이벤트에 대한 리스너 등록

        // 수영 중에는 이동 상태 진입을 막고 다시 수영 상태로 전환
        if (!player.Controller.IsGrounded()) // 접지 상태가 아닌 경우 (물 위에 뜬 경우 포함)
        {
            player.FSM.ChangeState(PlayerStateType.Swim); // 잘못된 진입 방지
            return;
        }

        player.AnimatorWrapper.SetMove(true); // 이동 시작 시 애니메이션 파라미터 활성화
    }

    public void Update()
    {
        player.Controller.ReadMoveInput(); // 입력을 받아 수평 이동 방향 및 점프 요청 처리

        player.AnimatorWrapper.SetRun(player.Controller.IsRunning());   // 달리기 여부에 따라 애니메이션 상태 갱신
        bool isRunning = player.Controller.IsRunning();
        player.AnimatorWrapper.SetJump(player.Controller.IsJumping()); // 공중 상승 중 여부에 따라 점프 애니메이션 갱신

        // 지면에 있고 이동 입력이 없을 때만 Idle 상태로 전환
        if (player.Controller.IsGrounded() && !player.Controller.HasMovementInput())
        {
            EventBus.PublishVoid("OnMoveStop"); // 이동 중단 이벤트 발생 → Idle 상태로 전환됨
        }

        // 갈고리 입력 (임시 Q 키)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.FSM.ChangeState(PlayerStateType.ThrowHook);
        }

        // 갈고리 입력 (임시 F 키)
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.FSM.ChangeState(PlayerStateType.Fish);
        }

        // 갈고리 입력 (임시 B 키)
        if (Input.GetKeyDown(KeyCode.B))
        {
            player.FSM.ChangeState(PlayerStateType.Build);
        }

        // 마우스 좌클릭 입력 (왼쪽 버튼)
        if (Input.GetMouseButtonDown(0))
        {
            player.FSM.ChangeState(PlayerStateType.Attack); // 공격 상태로 전환
        }

        // 방향값은 항상 넘기기 (카메라가 있을 때만)
        if (player.PlayerCamera != null)
        {
            Vector3 input = player.Controller.GetMoveInput(); // 입력 방향

            Vector3 camForward = player.PlayerCamera.transform.forward;
            Vector3 camRight = player.PlayerCamera.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.z + camRight * input.x;

            float horizontal = Vector3.Dot(moveDir, player.transform.right);
            float vertical = Vector3.Dot(moveDir, player.transform.forward);

            // 최소 보정값 처리
            if (Mathf.Abs(horizontal) < 0.01f) horizontal = 0f;
            if (Mathf.Abs(vertical) < 0.01f) vertical = 0f;

            player.AnimatorWrapper.SetDirection(horizontal, vertical);
            player.AnimatorWrapper.UpdateFlowDirection(); // 보간된 방향값을 애니메이터에 적용 // 방향 설정
        }

        // 이동 입력이 완전히 끊긴 경우 → 블렌드트리와 이동 파라미터 초기화
        if (!player.Controller.HasMovementInput() && player.Controller.IsGrounded())
        {
            player.AnimatorWrapper.SetDirection(0f, 0f);
            player.AnimatorWrapper.UpdateFlowDirection(); // 보간된 방향값을 애니메이터에 적용 // 블렌드 트리 중앙 복귀
            player.AnimatorWrapper.SetMove(false);       // 이동 파라미터 OFF
        }

    }

    public void FixedUpdate()
    {
        player.Controller.Move(); // 물리 프레임에서 수직/수평 이동을 실제로 반영
    }

    public void Exit()
    {
        EventBus.UnsubscribeVoid("OnMoveStop", OnMoveStop); // 상태 종료 시 이벤트 리스너 해제하여 메모리 누수 방지
    }

    private void OnMoveStop()
    {
        player.FSM.ChangeState(PlayerStateType.Idle); // OnMoveStop 이벤트 수신 시 Idle 상태로 전환
    }
}
