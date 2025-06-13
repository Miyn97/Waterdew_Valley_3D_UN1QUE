using UnityEngine;

// 전체 게임의 흐름을 제어하는 싱글톤 매니저 클래스
public class GameManager : Singleton<GameManager>
{
    [Header("게임 주요 참조")]
    [SerializeField] private Player player; // 현재 게임의 플레이어 참조

    public Player Player => player; // 외부에서 플레이어 접근 가능

    protected override void Awake()
    {
        base.Awake(); // Singleton 기반 초기화 (중복 방지)

        // 씬 유지용
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitGame(); // 게임 시작 시 초기화
    }

    // 게임 시작 초기화 함수
    private void InitGame()
    {
        Debug.Log("게임 초기화 완료");
        // 초기 상태나 월드 설정 등 추가 가능
    }

    // 사망 시 호출되는 처리
    public void OnPlayerDead()
    {
        Debug.Log("플레이어 사망 처리");
        EventBus.PublishVoid("OnPlayerDie"); // FSM 상태 전환 트리거
    }

    // 게임 오버 로직
    public void GameOver()
    {
        Debug.Log("게임 오버 처리");
        // 추후 UIManager 호출 등 처리
    }

    // 게임 리스타트 (사망 이후)
    public void RestartGame()
    {
        Debug.Log("게임 재시작 처리");
        // 씬 Reload 또는 플레이어 상태 복구 처리
    }
}
