using UnityEngine;

// 물 트리거 감지를 담당하는 컴포넌트 (플레이어가 물속에 진입/이탈 시 이벤트 발행)
public class WaterZone : MonoBehaviour
{
    private const string PlayerTag = "Player"; // 문자열 비교로 인한 GC 방지를 위해 상수로 "Player" 저장

    private void Start()
    {
        WaterSystem.SetWaterSurfaceY(-1.2f); // 게임 시작 시 수면 높이 기준값 설정 (Plane_Visual의 Y좌표와 일치하게 수동 지정)
    }

    // 플레이어가 물 트리거에 진입했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트일 경우에만 수영 진입 이벤트 처리
        if (other.CompareTag(PlayerTag)) // 문자열 비교는 상수 사용으로 GC 최소화
        {
            EventBus.PublishVoid("EnteredWater"); // 수영 상태 진입 이벤트 발행
        }
    }

    // 플레이어가 물 트리거에서 이탈했을 때 호출되는 메서드
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            var controller = other.GetComponent<CharacterController>();
            if (controller == null) return;

            float headY = other.transform.position.y + controller.height / 2f;
            float surfaceY = WaterSystem.GetSurfaceY();

            // 머리가 수면보다 일정 이상 올라가면 수영 해제
            if (headY > surfaceY + 0.2f)
            {
                EventBus.PublishVoid("ForceSwimToJump"); // 점프 상태 전이
            }
        }
    }
}
