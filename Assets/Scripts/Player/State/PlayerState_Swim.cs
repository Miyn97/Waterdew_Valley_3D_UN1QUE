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
        player.AnimatorWrapper.SetSwimming(true); // 수영 애니메이션 파라미터 활성화
        player.Controller.SetSwimMode(true);      // 컨트롤러 수영 모드 설정

        player.Controller.ForceVerticalVelocity(-1.5f); // 수영 진입 시 가라앉기 위한 초기 하강 속도 설정
    }


    public void Update()
    {
        // 수영 중 상태에서는 입력과 부력 처리는 PlayerController에 위임
        // 상태 전이는 WaterZone.cs → EventBus → FSM.OnExitWater() 경유로 처리됨

        player.Controller.ReadMoveInput();
        player.AnimatorWrapper.UpdateFlowDirection(); // 보간된 방향값을 애니메이터에 적용 // 입력 반영 추가

        // 예외 처리: 물에서 벗어났는데 상태 전환이 안 되었을 경우 대비 (보조 안전장치)
        if (!WaterSystem.IsUnderwater(player.transform.position))
        {
            // 이동 입력 여부에 따라 적절한 상태 전환 (수동 백업 루트)
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move);
            else
                player.FSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public void FixedUpdate()
    {
        player.Controller.Move(); // 수영 중 이동/부력 적용 처리
    }

    public void Exit()
    {
        player.AnimatorWrapper.SetSwimming(false); // 애니메이션 파라미터 해제
    }
}
