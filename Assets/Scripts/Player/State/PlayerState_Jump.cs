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
        player.AnimatorWrapper.SetJump(true); // 점프 애니메이션 활성화
        Debug.Log("Entered Jump State"); // 상태 진입 로그 출력
    }

    public void Update()
    {
        player.Controller.ReadMoveInput(); // 입력 처리

        // 착지 시 상태 전환
        if (player.Controller.IsGrounded())
        {
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move); // 이동 상태로 전환
            else
                player.FSM.ChangeState(PlayerStateType.Idle); // 대기 상태로 전환
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
