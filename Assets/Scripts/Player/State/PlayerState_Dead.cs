using UnityEngine;

// 사망 상태 클래스
public class PlayerState_Dead : IState
{
    private readonly Player player;
    private GameObject dummyUI;

    public PlayerState_Dead(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetDead(true); // 사망 애니메이션 실행
        dummyUI = GameObject.Find("DummyDeadUI"); // 임시 사망 UI 탐색
        if (dummyUI != null)
            dummyUI.SetActive(true);
        Debug.Log("사망 상태 진입");
    }

    public void Update() { } // 사망 상태에서는 입력 차단

    public void FixedUpdate() { }

    public void Exit()
    {
        if (dummyUI != null)
            dummyUI.SetActive(false);
        player.AnimatorWrapper.SetDead(false); // 애니메이션 리셋
        Debug.Log("사망 상태 종료");
    }
}
