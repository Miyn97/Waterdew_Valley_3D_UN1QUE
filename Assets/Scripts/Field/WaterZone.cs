using UnityEngine;

// 물 트리거 감지를 담당하는 컴포넌트 (플레이어가 물속에 진입/이탈 시 이벤트 발행)
public class WaterZone : MonoBehaviour
{
    private const string PlayerTag = "Player"; // 매 프레임 문자열 비교 방지용 상수 선언

    private void Start()
    {
        WaterSystem.SetWaterSurfaceY(-1.2f); // Plane_Visual 위치 기준으로 수면 설정
    }

    // 물에 진입 시 발생하는 트리거
    private void OnTriggerEnter(Collider other)
    {
        // "Player" 태그를 가진 객체만 수영 상태 대상
        if (other.CompareTag(PlayerTag)) // 문자열 비교 GC 줄이기 위해 상수 사용
        {
            EventBus.PublishVoid("EnteredWater"); // 수영 진입 이벤트 발행
        }
    }

    // 물에서 나갈 때 발생하는 트리거
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            EventBus.PublishVoid("ExitedWater"); // 수영 종료 이벤트 발행
        }
    }

}
