// 플레이어 건축 상태 클래스
using UnityEngine;

public class PlayerState_Build : IState
{
    private readonly Player player;

    public PlayerState_Build(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetBuilding(true);
        Debug.Log("Entered Build State");
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
        player.AnimatorWrapper.SetBuilding(false);
    }
}
