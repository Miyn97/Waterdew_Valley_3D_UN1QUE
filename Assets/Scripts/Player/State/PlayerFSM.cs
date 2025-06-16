using UnityEngine;
using System.Collections.Generic;

// 플레이어 상태머신을 담당하는 클래스
public class PlayerFSM
{
    private readonly Player player; // FSM이 작동할 대상 플레이어
    private IState currentState; // 현재 활성화된 상태
    private Dictionary<PlayerStateType, IState> states; // 상태 캐싱용 딕셔너리

    public PlayerStateType CurrentStateType { get; private set; } // 현재 상태 타입 외부 노출

    public PlayerFSM(Player player)
    {
        this.player = player; // 플레이어 참조 저장

        // 상태 딕셔너리 초기화 및 인스턴스 등록
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

        // 물 진입/이탈 이벤트 구독
        EventBus.SubscribeVoid("EnteredWater", OnEnterWater); // 물 진입 시 수영 전이
        EventBus.SubscribeVoid("ExitedWater", OnExitWater);   // 물 이탈 시 Idle 전이
        EventBus.SubscribeVoid("ForceSwimToJump", OnForceSwimToJump); // 수영 상태 중 점프 전이용

    }

    // 상태 전환 메서드
    public void ChangeState(PlayerStateType stateType)
    {
        if (states.TryGetValue(stateType, out var newState))
        {
            currentState?.Exit();            // 기존 상태 종료
            currentState = newState;        // 새 상태 등록
            CurrentStateType = stateType;   // 상태 타입 갱신
            currentState.Enter();           // 새 상태 진입
        }
    }

    // 프레임 업데이트
    public void Update()
    {
        currentState?.Update(); // 현재 상태의 Update 호출
    }

    // FixedUpdate에서 호출됨
    public void FixedUpdate()
    {
        currentState?.FixedUpdate(); // 현재 상태의 FixedUpdate 호출
    }

    // 수영 상태 진입 (WaterZone.cs에서 EventBus 통해 호출됨)
    private void OnEnterWater()
    {
        // 반드시 수면 아래에 있는 경우만 수영 상태로 전이
        if (WaterSystem.IsUnderwater(player.transform.position)) // ✔ 수면 아래 조건 추가
        {
            player.Controller.SetSwimMode(true); // 컨트롤러 수영 모드 활성화
            ChangeState(PlayerStateType.Swim);   // 상태 전이
        }
    }

    // 수영 상태 종료 (WaterZone.cs에서 EventBus 통해 호출됨)
    private void OnExitWater()
    {
        player.Controller.SetSwimMode(false); // 수영 모드 종료
        if (player.Controller.HasMovementInput())
            ChangeState(PlayerStateType.Move);
        else
            ChangeState(PlayerStateType.Idle);
    }

    private void OnForceSwimToJump()
    {
        player.Controller.SetSwimMode(false); // 부력 해제
        ChangeState(PlayerStateType.Jump); // 바로 점프 상태 진입
    }
}
