using UnityEngine;

// 플레이어 점프 상태 클래스
public class PlayerState_Jump : IState
{
    private readonly Player player; // FSM 대상이 되는 플레이어 참조

    public PlayerState_Jump(Player player)
    {
        this.player = player; // 생성자에서 플레이어 인스턴스 저장
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetJump(true); // 1. 애니메이션 먼저 설정
        player.Controller.DoJump();           // 2. Y속도 상승은 그다음
    }

    public void Update()
    {
        player.Controller.ReadMoveInput();
        player.AnimatorWrapper.UpdateFlowDirection(); // 보간된 방향값을 애니메이터에 적용

        if (player.Controller.IsGrounded())
        {
            if (player.AnimatorWrapper != null)
                player.AnimatorWrapper.SetJump(false); // 애니메이션 먼저 false로 전환

            // 한 프레임 정도 딜레이 두기 → Exit()과 충돌 방지
            // 예: 다음 프레임에 전이되도록 보장할 수 있는 flag, 또는 상태 잠깐 유지

            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move);
            else
                player.FSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public void FixedUpdate()
    {
        player.Controller.Move(); // 이동/중력 처리
    }

    public void Exit()
    {
        player.AnimatorWrapper.SetJump(false); // 점프 애니메이션 비활성화
    }
}
