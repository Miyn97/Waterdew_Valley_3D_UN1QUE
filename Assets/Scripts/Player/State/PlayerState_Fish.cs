// 플레이어 낚시 상태 클래스
using UnityEngine;

public class PlayerState_Fish : IState
{
    private readonly Player player;
    private GameObject fishingRodPrefab;

    public PlayerState_Fish(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entered Fish State");
        fishingRodPrefab = Object.Instantiate(player.fishingRodPrefab, player.hand.position, Quaternion.identity, player.hand);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            player.FSM.ChangeState(PlayerStateType.Idle); // 원상태로 복귀
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {
        Debug.Log("낚시 모드 해제");
        Object.Destroy(fishingRodPrefab);
    }
}
