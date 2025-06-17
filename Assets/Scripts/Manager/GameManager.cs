using UnityEngine;

// 전체 게임의 흐름을 제어하는 싱글톤 매니저 클래스
public class GameManager : Singleton<GameManager> // 싱글톤 패턴 적용
{
    [Header("게임 주요 참조")]
    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹 참조
    [SerializeField] private Transform spawnPoint;    // 플레이어 스폰 위치
    private GameObject currentPlayer;                 // 현재 활성 플레이어
    private Player player;                            // Player 스크립트 직접 참조

    public Player Player => player; // 외부 접근용 프로퍼티

    protected override void Awake()
    {
        base.Awake(); // 싱글톤 초기화
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지

        EventBus.SubscribeVoid("OnPlayerDie", OnPlayerDead); // 사망 이벤트 구독
    }

    private void Start()
    {
        InitGame(); // 게임 시작 시 초기화
    }

    // 게임 초기화 처리
    private void InitGame()
    {
        SpawnPlayer(); // 플레이어 생성
    }

    // 플레이어 생성 함수
    private void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity); // 프리팹 생성
            player = currentPlayer.GetComponent<Player>(); // Player 참조 캐싱
        }
        else
        {
            Debug.LogWarning("플레이어 프리팹 또는 스폰 위치가 설정되지 않음");
        }
    }

    // 플레이어 사망 시 호출
    public void OnPlayerDead()
    {
        Debug.Log("GameManager: 플레이어 사망 처리");
        EventBus.PublishVoid("OnPlayerDie"); // FSM 상태 전환 유도
    }

    // 게임 오버 로직 (예: UI 처리 등)
    public void GameOver()
    {
        Debug.Log("GameManager: 게임 오버");
        // UIManager.Instance.ShowGameOver(); 등 가능
    }

    // 게임 리스타트 처리 (사망 이후)
    public void RestartGame()
    {
        Debug.Log("GameManager: 게임 재시작");
        // 씬 리로드 또는 InitGame() 재호출 등 처리 가능
    }

    private void OnDestroy()
    {
        EventBus.UnsubscribeVoid("OnPlayerDie", OnPlayerDead); // 이벤트 해제
    }
}
