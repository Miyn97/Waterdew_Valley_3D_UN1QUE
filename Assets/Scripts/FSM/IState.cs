using UnityEngine;

// FSM 상태 클래스가 구현해야 할 인터페이스
public interface IState
{
    void Enter(); // 상태 진입 시 호출
    void Update(); // 매 프레임 처리
    void FixedUpdate(); // 물리 처리 필요 시 호출
    void Exit(); // 상태 종료 시 호출
}
