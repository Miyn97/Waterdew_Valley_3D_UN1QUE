using UnityEngine;

// 건축 모드 상태 클래스
public class PlayerState_Build : IState
{
    private readonly Player player; // FSM 대상 플레이어
    private GameObject dummyUI;     // 임시 UI 오브젝트

    public PlayerState_Build(Player player)
    {
        this.player = player; // 생성자에서 참조 저장
    }

    public void Enter()
    {
        dummyUI = GameObject.Find("DummyBuildUI"); // 씬에 존재하는 더미 UI 탐색
        if (dummyUI != null)
            dummyUI.SetActive(true); // 더미 UI 활성화
        Debug.Log("건축 모드 진입"); // 디버그 출력
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC 입력 시
            player.FSM.ChangeState(PlayerStateType.Idle); // Idle 상태 복귀
    }

    public void FixedUpdate() { } // 건축은 고정 프레임에서 별도 처리 없음

    public void Exit()
    {
        if (dummyUI != null)
            dummyUI.SetActive(false); // UI 비활성화
        Debug.Log("건축 모드 종료"); // 디버그 출력
    }
}
