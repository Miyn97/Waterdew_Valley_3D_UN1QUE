using UnityEngine;

public class PlayerState_ThrowHook : IState
{
    private readonly Player player;
    private GameObject ropePrefab;
    private Rope rope;
    private Hook hook;

    public PlayerState_ThrowHook(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entered ThrowHook State");
        //ropePrefab = Object.Instantiate(player.ropePrefab, player.hand.position, player.transform.rotation, player.hand);
        ropePrefab = Object.Instantiate(player.ropePrefab, player.hand.position, Quaternion.identity, player.hand);
        rope = ropePrefab.GetComponent<Rope>();
        hook = ropePrefab.GetComponentInChildren<Hook>();
        rope.startPoint = player.hand;
        hook.startPosition = player.hand;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            player.FSM.ChangeState(PlayerStateType.Idle); // 원상태로 복귀
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {
        Debug.Log("훅 모드 해제");
        Object.Destroy(ropePrefab);
    }
}
