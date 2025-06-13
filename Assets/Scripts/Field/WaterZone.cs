using UnityEngine;

// 물 트리거 감지를 담당하는 컴포넌트 (플레이어가 물속에 진입/이탈 시 이벤트 발행)
public class WaterZone : MonoBehaviour
{
    // 물에 진입 시 발생하는 트리거
    private void OnTriggerEnter(Collider other)
    {
        // "Player" 태그를 가진 객체만 수영 상태 대상
        if (other.CompareTag("Player"))
        {
            EventBus.PublishVoid("EnteredWater"); // 수영 진입 이벤트 발행
        }
    }

    // 물에서 나갈 때 발생하는 트리거
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventBus.PublishVoid("ExitedWater"); // 수영 종료 이벤트 발행
        }
    }
}
