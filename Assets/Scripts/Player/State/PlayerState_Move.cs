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
        player.AnimatorWrapper.SetMove(true); // 이동 시작 시 애니메이션 파라미터 활성화
    }

    public void Update()
    {
        player.Controller.ReadMoveInput(); // 입력을 받아 수평 이동 방향 및 점프 요청 처리

        player.AnimatorWrapper.SetRun(player.Controller.IsRunning());   // 달리기 여부에 따라 애니메이션 상태 갱신
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

        // 마우스 좌클릭 입력 (왼쪽 버튼)
        if (Input.GetMouseButtonDown(0))
        {
            player.FSM.ChangeState(PlayerStateType.Attack); // 예: 공격 상태
                                                            // 또는 낚시 FSM 전환: player.FSM.ChangeState(PlayerStateType.Fish);
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
