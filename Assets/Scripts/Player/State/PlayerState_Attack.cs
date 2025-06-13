using UnityEngine;

// 플레이어가 공격하는 동안의 FSM 상태 클래스
public class PlayerState_Attack : IState
{
    private readonly Player player; // FSM 대상이 되는 플레이어 참조

    private float attackDuration = 0.5f; // 공격 애니메이션 유지 시간 (예: 0.5초)
    private float attackTimer = 0f; // 경과 시간 누적용 변수

    public PlayerState_Attack(Player player)
    {
        this.player = player; // 생성자에서 플레이어 참조 저장
    }

    public void Enter()
    {
        attackTimer = 0f; // 진입 시 타이머 초기화

        player.AnimatorWrapper.SetAttacking(true); // 공격 애니메이션 파라미터 활성화

        // 디버그 로그 출력
        Debug.Log("Entered Attack State");
    }

    public void Update()
    {
        attackTimer += Time.deltaTime; // 매 프레임 경과 시간 누적

        if (attackTimer >= attackDuration)
        {
            // 공격 종료 시 다음 상태로 전환
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move); // 이동 중이면 이동 상태 복귀
            else
                player.FSM.ChangeState(PlayerStateType.Idle); // 아니면 대기 상태 복귀
        }
    }

    public void FixedUpdate()
    {
        // 공격 중에도 중력 처리 필요하면 유지
        player.Controller.Move(); // 이동 자체는 허용 (Raft 스타일)
    }

    public void Exit()
    {
        player.AnimatorWrapper.SetAttacking(false); // 공격 애니메이션 파라미터 비활성화
    }
}
