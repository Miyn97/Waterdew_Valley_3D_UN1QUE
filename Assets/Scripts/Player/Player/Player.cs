using UnityEngine;

// 플레이어 핵심 제어 스크립트 (FSM, 입력, 애니메이션 연결)
public class Player : MonoBehaviour
{
    [Header("컴포넌트 참조")]
    [SerializeField] private PlayerController controller; // 입력 처리용 컴포넌트
    [SerializeField] private PlayerAnimation animatorWrapper; // 애니메이션 처리용

    public PlayerController Controller => controller; // 외부에서 읽기 전용
    public PlayerAnimation AnimatorWrapper => animatorWrapper; // 외부에서 읽기 전용

    public PlayerFSM FSM { get; private set; } // FSM 상태머신

    private void Awake()
    {
        FSM = new PlayerFSM(this); // FSM 인스턴스 생성
    }

    private void Start()
    {
        FSM.ChangeState(new PlayerState_Idle(this)); // 상태 초기 진입
    }

    private void Update()
    {
        FSM.Update(); // 상태 Update 실행
    }

    private void FixedUpdate()
    {
        FSM.FixedUpdate(); // 이동 처리 상태용 FixedUpdate 실행
    }
}
