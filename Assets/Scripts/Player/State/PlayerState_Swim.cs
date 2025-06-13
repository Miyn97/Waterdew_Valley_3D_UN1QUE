using UnityEngine;

// 플레이어가 수영 중일 때의 FSM 상태 클래스
public class PlayerState_Swim : IState
{
    private readonly Player player; // FSM 대상이 되는 플레이어 객체 참조

    public PlayerState_Swim(Player player)
    {
        this.player = player; // 생성자에서 플레이어 참조 저장
    }

    public void Enter()
    {
        // 수영 상태 진입 시 애니메이션 파라미터 설정
        player.AnimatorWrapper.SetSwimming(true); // "IsSwimming" 파라미터를 true로 설정

        // 수영 상태 진입 로그 출력
        Debug.Log("Entered Swim State");
    }

    public void Update()
    {
        // 플레이어가 물 밖으로 나왔는지 판단 (y 좌표로 단순 판별)
        if (player.transform.position.y > 0.5f)
        {
            // 이동 입력 여부에 따라 Idle 또는 Move 상태로 전환
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move); // 이동 상태로 전환
            else
                player.FSM.ChangeState(PlayerStateType.Idle); // 대기 상태로 전환
        }
    }

    public void FixedUpdate()
    {
        // 수영 중에도 기본적인 물리 이동은 유지 (수영용 Move()로 대체 가능)
        player.Controller.Move(); // 수직/수평 이동 적용
    }

    public void Exit()
    {
        // 수영 상태에서 나올 때 애니메이션 파라미터 해제
        player.AnimatorWrapper.SetSwimming(false); // "IsSwimming" 파라미터를 false로 설정
    }
}
