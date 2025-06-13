using System.Collections.Generic;

// 플레이어 상태머신을 담당하는 클래스
public class PlayerFSM
{
    private readonly Player player; // FSM이 작동할 대상 플레이어
    private IState currentState; // 현재 활성화된 상태

    private Dictionary<PlayerStateType, IState> states; // 상태 캐싱용 딕셔너리

    public PlayerStateType CurrentStateType { get; private set; } // 현재 상태 타입을 외부에 노출

    public PlayerFSM(Player player)
    {
        this.player = player; // 대상 플레이어 저장

        // 상태 딕셔너리 초기화 및 등록
        states = new Dictionary<PlayerStateType, IState>
        {
            { PlayerStateType.Idle, new PlayerState_Idle(player) },
            { PlayerStateType.Move, new PlayerState_Move(player) },
            { PlayerStateType.Jump, new PlayerState_Jump(player) },
            { PlayerStateType.Fish, new PlayerState_Fish(player) },
            { PlayerStateType.Build, new PlayerState_Build(player) },
            { PlayerStateType.Craft, new PlayerState_Craft(player) },
            { PlayerStateType.Attack, new PlayerState_Attack(player) },
            { PlayerStateType.Dead, new PlayerState_Dead(player) },
            { PlayerStateType.ThrowHook, new PlayerState_ThrowHook(player) },
            { PlayerStateType.Swim, new PlayerState_Swim(player) }

        };
    }

    public void ChangeState(PlayerStateType stateType)
    {
        if (states.TryGetValue(stateType, out var newState)) // 상태가 존재하는 경우
        {
            currentState?.Exit(); // 현재 상태 종료 처리
            currentState = newState; // 새로운 상태로 전환
            CurrentStateType = stateType; // 현재 상태 타입 갱신
            currentState.Enter(); // 상태 진입 처리
        }
    }

    public void Update()
    {
        currentState?.Update(); // 현재 상태의 Update 호출
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate(); // 현재 상태의 FixedUpdate 호출
    }
}

// 플레이어 상태 타입 정의용 열거형
public enum PlayerStateType
{
    Idle,       // 대기 상태
    Move,       // 이동 상태
    Jump,       // 점프 상태
    Fish,       // 낚시 상태
    Build,      // 건축 상태
    Craft,      // 제작/요리 상태
    Attack,     // 공격 상태
    Dead,       // 사망 상태
    ThrowHook,  // 갈고리 상태
    Swim        //수영 상태
}
