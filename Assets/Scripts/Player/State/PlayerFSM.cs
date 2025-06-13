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
        this.player = player; // 플레이어 저장

        // 상태 딕셔너리 초기화 및 상태 인스턴스 등록
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

        // 수영 진입/이탈 이벤트 구독 처리
        EventBus.SubscribeVoid("EnteredWater", OnEnterWater); // 물에 들어갔을 때 수영 상태로 전환
        EventBus.SubscribeVoid("ExitedWater", OnExitWater);   // 물에서 나왔을 때 Idle 상태로 전환
    }

    public void ChangeState(PlayerStateType stateType)
    {
        if (states.TryGetValue(stateType, out var newState)) // 상태 존재 여부 확인
        {
            currentState?.Exit(); // 기존 상태 종료
            currentState = newState; // 새 상태 전환
            CurrentStateType = stateType; // 현재 상태 타입 갱신
            currentState.Enter(); // 새 상태 진입
        }
    }

    public void Update()
    {
        currentState?.Update(); // 현재 상태 업데이트 호출
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate(); // 현재 상태 FixedUpdate 호출
    }

    // 수영 상태 진입 트리거 (WaterZone 통해 이벤트 수신)
    private void OnEnterWater()
    {
        player.Controller.SetSwimMode(true); // 컨트롤러 수영 모드 활성화
        ChangeState(PlayerStateType.Swim);   // 상태 전환
    }

    // 수영 상태 종료 트리거 (WaterZone 통해 이벤트 수신)
    private void OnExitWater()
    {
        player.Controller.SetSwimMode(false); // 컨트롤러 수영 모드 비활성화
        ChangeState(PlayerStateType.Idle);    // Idle로 복귀
    }
}
