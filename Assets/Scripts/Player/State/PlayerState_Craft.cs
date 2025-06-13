using UnityEngine;

// 제작/조합 모드 상태 클래스
public class PlayerState_Craft : IState
{
    private readonly Player player;
    private GameObject dummyUI;

    public PlayerState_Craft(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        dummyUI = GameObject.Find("DummyCraftUI"); // 더미 제작 UI 오브젝트
        if (dummyUI != null)
            dummyUI.SetActive(true);
        Debug.Log("제작 모드 진입");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC 키로 닫기
            player.FSM.ChangeState(PlayerStateType.Idle);
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        if (dummyUI != null)
            dummyUI.SetActive(false);
        Debug.Log("제작 모드 종료");
    }
}
