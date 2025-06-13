using UnityEngine;

// 플레이어가 수영 상태에 진입하거나 빠져나가는지를 감지하는 테스트용 컴포넌트
public class Test_PlayerWaterCheck : MonoBehaviour
{
    [SerializeField] private Player player; // FSM을 제어할 대상 플레이어 객체

    private bool isInWater = false; // 현재 물속에 있는지를 나타내는 내부 상태 캐싱 변수

    private void Update()
    {
        // 플레이어의 현재 y 좌표가 기준보다 낮은지 판단 (임시 수면 기준: y < 0.5f)
        bool nowInWater = transform.position.y < 0.5f;

        // 이전에는 물 밖이었는데, 지금은 물 안에 들어간 경우
        if (!isInWater && nowInWater)
        {
            player.FSM.ChangeState(PlayerStateType.Swim); // 수영 상태로 전환
        }
        // 이전에는 물 안이었는데, 지금은 물 밖으로 나온 경우
        else if (isInWater && !nowInWater)
        {
            // 이동 입력이 있으면 이동 상태로 전환
            if (player.Controller.HasMovementInput())
                player.FSM.ChangeState(PlayerStateType.Move); // 이동 상태로 전환
            else
                player.FSM.ChangeState(PlayerStateType.Idle); // 대기 상태로 전환
        }

        // 현재 물 상태를 내부 상태로 저장 (다음 프레임 비교용)
        isInWater = nowInWater;
    }
}
