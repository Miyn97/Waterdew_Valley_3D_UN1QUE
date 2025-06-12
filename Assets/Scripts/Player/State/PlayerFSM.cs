// 플레이어 상태머신을 담당하는 클래스
public class PlayerFSM
{
    private readonly Player player; // FSM이 작동할 대상 플레이어
    private IState currentState; // 현재 상태

    public PlayerFSM(Player player)
    {
        this.player = player; // 대상 플레이어 캐싱
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit(); // 이전 상태 종료 처리

        currentState = newState; // 새로운 상태로 전환
        currentState.Enter(); // 새로운 상태 진입 처리
    }

    public void Update()
    {
        currentState?.Update(); // 현재 상태의 Update 처리 (null 방지)
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate(); // 현재 상태의 FixedUpdate 처리
    }
}
