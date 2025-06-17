using UnityEngine;

// 플레이어 핵심 제어 스크립트 (FSM, 입력, 애니메이션 연결)
public class Player : MonoBehaviour
{
    [Header("컴포넌트 참조")]
    [SerializeField] private PlayerController controller; // 이동/점프/수영 처리용 컨트롤러
    [SerializeField] private PlayerAnimation animatorWrapper; // 애니메이션 래퍼 클래스 (Animator 파라미터 제어용)
    [SerializeField] private Camera playerCamera; // 현재 플레이어가 사용할 카메라 (3인칭 기준)
    public GameObject buildManager;

    public Transform hand;
    public GameObject fishingRodPrefab;
    public GameObject hammerPrefab;
    public GameObject ropePrefab;

    public PlayerController Controller => controller; // 외부에서 접근 가능한 컨트롤러 프로퍼티 (읽기 전용)
    public PlayerAnimation AnimatorWrapper => animatorWrapper; // 외부에서 접근 가능한 애니메이션 래퍼 프로퍼티
    public Camera PlayerCamera => playerCamera; // 외부에서 사용할 카메라 반환용 프로퍼티

    public PlayerFSM FSM { get; private set; } // 플레이어 FSM 상태머신

    private void Awake()
    {
        FSM = new PlayerFSM(this); // FSM 인스턴스 생성 (플레이어를 컨텍스트로 전달)

        // animatorWrapper가 비어 있는 경우 자동으로 PlayerAnimation 컴포넌트를 가져옴
        if (animatorWrapper == null)
            animatorWrapper = GetComponent<PlayerAnimation>(); // 애니메이션 래퍼 자동 할당

        // controller가 비어 있는 경우 자동으로 PlayerController 컴포넌트를 가져옴
        if (controller == null)
            controller = GetComponent<PlayerController>(); // 컨트롤러 자동 할당

        // playerCamera가 비어 있는 경우 자동으로 Camera.main을 가져옴
        if (playerCamera == null)
            playerCamera = Camera.main; // 카메라 참조 자동 할당
    }

    private void Start()
    {
        FSM.ChangeState(PlayerStateType.Idle); // 시작 시 기본 Idle 상태로 진입
        RegisterWaterEvents(); // 바다 진입/이탈 이벤트 구독 등록
    }

    private void Update()
    {
        FSM.Update(); // 현재 상태의 Update 메서드 호출
    }

    private void FixedUpdate()
    {
        FSM.FixedUpdate(); // 현재 상태의 FixedUpdate 호출 (이동 및 중력/부력 처리용)
    }

    private void OnEnable()
    {
        // 사망 이벤트 수신 등록 (체력 0 → 상태 전이)
        EventBus.SubscribeVoid("OnPlayerDie", OnPlayerDie);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("OnPlayerDie", OnPlayerDie); // 사망 이벤트 구독 해제
        UnregisterWaterEvents(); // 수영 관련 이벤트 구독 해제
    }

    private void OnPlayerDie()
    {
        // 현재 상태가 이미 Dead인 경우 중복 전이 방지
        if (FSM.CurrentStateType != PlayerStateType.Dead)
            FSM.ChangeState(PlayerStateType.Dead); // 사망 상태로 전이
    }

    // 물 진입/이탈 이벤트 등록
    private void RegisterWaterEvents()
    {
        EventBus.SubscribeVoid("EnteredWater", OnEnterWater); // 물 진입 이벤트 구독
        EventBus.SubscribeVoid("ExitedWater", OnExitWater);   // 물 이탈 이벤트 구독
    }

    // 물 진입/이탈 이벤트 해제
    private void UnregisterWaterEvents()
    {
        EventBus.UnsubscribeVoid("EnteredWater", OnEnterWater); // 진입 이벤트 해제
        EventBus.UnsubscribeVoid("ExitedWater", OnExitWater);   // 이탈 이벤트 해제
    }

    // 수영 상태 진입 처리
    private void OnEnterWater()
    {
        FSM.ChangeState(PlayerStateType.Swim); // 수영 상태로 전이 (수면 아래일 경우)
    }

    // 수영 상태 탈출 처리
    private void OnExitWater()
    {
        // 안전 처리: controller 존재 여부 체크 후 상태 전이
        if (controller == null)
            return;

        // 입력이 있는 경우 → 이동 상태, 입력 없을 경우 → 대기 상태
        if (controller.HasMovementInput())
            FSM.ChangeState(PlayerStateType.Move);
        else
            FSM.ChangeState(PlayerStateType.Idle);
    }
}
