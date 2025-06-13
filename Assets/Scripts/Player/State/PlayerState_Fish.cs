// 플레이어 낚시 상태 클래스
using UnityEngine;

public class PlayerState_Fish : IState
{
    private readonly Player player;

    public PlayerState_Fish(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetFishing(true);
        Debug.Log("Entered Fish State");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.FSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        player.AnimatorWrapper.SetFishing(false);
    }
}
