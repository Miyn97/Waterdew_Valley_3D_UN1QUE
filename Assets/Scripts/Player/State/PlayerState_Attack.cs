// 플레이어 공격 상태 클래스
using UnityEngine;

public class PlayerState_Attack : IState
{
    private readonly Player player;
    private float attackDuration = 0.5f;
    private float attackTimer = 0f;

    public PlayerState_Attack(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        attackTimer = 0f;
        player.AnimatorWrapper.SetAttacking(true);
        Debug.Log("Entered Attack State");
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
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
        player.AnimatorWrapper.SetAttacking(false);
    }
}
