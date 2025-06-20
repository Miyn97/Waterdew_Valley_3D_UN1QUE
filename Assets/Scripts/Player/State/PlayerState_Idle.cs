﻿using UnityEngine;

// 플레이어 대기 상태 클래스
public class PlayerState_Idle : IState
{
    private readonly Player player; // FSM 대상이 되는 플레이어 객체 참조

    private float idleActionTimer = 0f; // 대기 상태 누적 시간
    private float nextIdleActionTime = 0f; // 다음 연출 발동까지 간격 시간

    public PlayerState_Idle(Player player)
    {
        this.player = player; // 생성자 주입으로 플레이어 인스턴스 저장
    }

    public void Enter()
    {
        EventBus.SubscribeVoid("OnMoveStart", OnMoveStart); // 이동 시작 이벤트 구독 등록

        // Idle 상태 진입 시 반드시 방향 초기화
        player.AnimatorWrapper.SetDirection(0f, 0f);
        player.AnimatorWrapper.UpdateFlowDirection(); // 보간된 방향값을 애니메이터에 적용

        // 수영 중인 상태에서 Idle 상태 진입을 방지
        if (!player.Controller.IsGrounded()) // 접지되지 않은 경우 수영 상태로 강제 전환
        {
            player.FSM.ChangeState(PlayerStateType.Swim);
            return;
        }

        // Idle 상태 진입 시 수중 효과 강제 OFF 처리
        if (player.Controller.UnderwaterVolume != null) // null 체크
        {
            player.Controller.UnderwaterVolume.weight = 0f; // 수면 위 상태 확정 시 강제 끔
        }

        player.AnimatorWrapper.SetMove(false); // Idle 상태 진입 시 애니메이션에서 이동 상태 false 설정

        // 랜덤 연출 타이머 초기화
        idleActionTimer = 0f; // 경과 시간 초기화
        nextIdleActionTime = Random.Range(4f, 10f); // 다음 연출까지 간격 설정
    }

    public void Update()
    {
        player.Controller.ReadMoveInput(); // 방향 및 점프 입력 처리

        // 지면에 있을 때만 이동 상태로 전환 가능하도록 조건 추가
        if (player.Controller.IsGrounded() &&
            (player.Controller.HasMovementInput() || Input.GetKeyDown(KeyCode.Space)))
        {
            EventBus.PublishVoid("OnMoveStart"); // 이동 시작 이벤트 발행 → FSM 상태 전환 트리거
        }

        // 이동 입력 발생 시 즉시 Move 상태 전이
        if (player.Controller.HasMovementInput())
        {
            EventBus.PublishVoid("OnMoveStart");
            return; // 이동 상태로 넘어갔으면 아래는 건너뜀
        }

        // 점프 입력 시 강제 점프 상태로 전이
        if (Input.GetKeyDown(KeyCode.Space) && player.Controller.IsGrounded())
        {
            player.FSM.ChangeState(PlayerStateType.Jump);
            return; // 점프 상태로 넘어갔으면 아래는 건너뜀
        }

        // 갈고리 입력 (임시 Q 키)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.FSM.ChangeState(PlayerStateType.ThrowHook);
        }

        // 갈고리 입력 (임시 F 키)
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.FSM.ChangeState(PlayerStateType.Fish);
        }

        // 갈고리 입력 (임시 B 키)
        if (Input.GetKeyDown(KeyCode.B))
        {
            player.FSM.ChangeState(PlayerStateType.Build);
        }

        // 마우스 좌클릭 입력 (왼쪽 버튼)
        if (Input.GetMouseButtonDown(0))
        {
            player.FSM.ChangeState(PlayerStateType.Attack); // 공격 상태로 전환
        }

        // -------- 랜덤 대기 연출 트리거 처리 --------
        idleActionTimer += Time.deltaTime; // 시간 누적

        if (idleActionTimer >= nextIdleActionTime) // 설정된 간격이 지나면
        {
            idleActionTimer = 0f; // 타이머 초기화
            nextIdleActionTime = Random.Range(4f, 10f); // 다음 발동 시간 재설정

            int random = Random.Range(0, 2); // 0 또는 1 랜덤

            if (random == 0)
                player.AnimatorWrapper.TriggerLookAround(); // 고개 돌리기 트리거 발동
            else
                player.AnimatorWrapper.TriggerRelaxed(); // 자세 전환 트리거 발동
        }
    }

    public void FixedUpdate()
    {
        // Idle 상태에서도 중력 및 점프 처리를 위해 Move()는 항상 호출되어야 함
        // 그래야 FixedUpdate 타이밍이 FSM 상태 전이보다 먼저 올 때도 점프 유효
        player.Controller.Move(); // 이동/중력/점프 처리 실행
    }

    public void Exit()
    {
        EventBus.UnsubscribeVoid("OnMoveStart", OnMoveStart); // 상태 종료 시 이동 이벤트 구독 해제
    }

    private void OnMoveStart()
    {
        player.FSM.ChangeState(PlayerStateType.Move); // FSM 상태를 Move로 전환
    }
}
