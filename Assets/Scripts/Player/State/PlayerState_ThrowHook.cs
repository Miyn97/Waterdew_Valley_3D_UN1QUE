using UnityEngine;

public class PlayerState_ThrowHook : IState
{
    private readonly Player player;

    public PlayerState_ThrowHook(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entered ThrowHook State");
        // 갈고리 캐스팅 애니메이션 시작 또는 UI
        // player.AnimatorWrapper.SetThrowHook(true); // 필요 시
    }

    public void Update()
    {
        // 입력 해제 또는 던지기 완료 시 상태 복귀
        if (Input.GetMouseButtonUp(0)) // 예: 버튼 떼면 완료
        {
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move);
            else
                player.FSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        // player.AnimatorWrapper.SetThrowHook(false); // 필요 시
    }
}
