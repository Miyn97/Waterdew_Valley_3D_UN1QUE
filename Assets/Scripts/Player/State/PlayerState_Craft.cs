// 플레이어 조합 상태 클래스
using UnityEngine;

public class PlayerState_Craft : IState
{
    private readonly Player player;

    public PlayerState_Craft(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetCrafting(true);
        Debug.Log("Entered Craft State");
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
        player.AnimatorWrapper.SetCrafting(false);
    }
}
