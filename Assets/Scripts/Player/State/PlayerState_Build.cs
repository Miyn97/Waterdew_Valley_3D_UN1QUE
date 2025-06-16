using UnityEngine;

// 건축 모드 상태 클래스
public class PlayerState_Build : IState
{
    private readonly Player player; // FSM 대상 플레이어
    private GameObject hammerPrefab;

    public PlayerState_Build(Player player)
    {
        this.player = player; // 생성자에서 참조 저장
    }

    public void Enter()
    {
        Debug.Log("건축 모드 진입"); // 디버그 출력
        hammerPrefab = Object.Instantiate(player.hammerPrefab, player.hand.position, Quaternion.identity, player.hand);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            player.FSM.ChangeState(PlayerStateType.Idle); // Idle 상태 복귀
    }

    public void FixedUpdate()
    {

    } // 건축은 고정 프레임에서 별도 처리 없음

    public void Exit()
    {
        Debug.Log("건축 모드 종료");
        Object.Destroy(hammerPrefab);
    }
}
