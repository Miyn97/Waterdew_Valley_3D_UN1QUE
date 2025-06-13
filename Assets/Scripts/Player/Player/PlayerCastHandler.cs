using UnityEngine;

// 플레이어가 캐스팅(낚싯대, 갈고리 등)을 처리하는 기능 전용 클래스
public class PlayerCastHandler : MonoBehaviour
{
    [SerializeField] private Animator animator; // 캐스팅 애니메이션을 위한 Animator 참조
    [SerializeField] private GameObject castOrigin; // 캐스팅 출발 위치 (플레이어 손 등)
    [SerializeField] private GameObject castPrefab; // 던질 프리팹 (낚싯대 캐스팅 or 갈고리)

    private bool isCasting = false; // 현재 캐스팅 중인지 여부 확인용

    private void Update()
    {
        // 마우스 좌클릭 눌렀을 때 캐스팅 시작
        if (Input.GetMouseButtonDown(0))
        {
            StartCast(); // 캐스팅 시작 처리
        }

        // 마우스 좌클릭 뗐을 때 캐스팅 완료
        if (isCasting && Input.GetMouseButtonUp(0))
        {
            CompleteCast(); // 캐스팅 완료 처리
        }
    }

    private void StartCast()
    {
        if (isCasting) return; // 중복 캐스팅 방지

        isCasting = true; // 캐스팅 상태 진입

        // 애니메이션 트리거 또는 파라미터 처리 (원하는 방식으로)
        animator.SetTrigger("Cast"); // 캐스팅 애니메이션 실행

        Debug.Log("캐스팅 시작됨");
    }

    private void CompleteCast()
    {
        isCasting = false; // 캐스팅 종료 상태로 변경

        // 실제 오브젝트(갈고리, 찌 등)를 던지는 처리
        if (castPrefab != null && castOrigin != null)
        {
            Instantiate(castPrefab, castOrigin.transform.position, castOrigin.transform.rotation); // 오브젝트 생성
        }

        Debug.Log("캐스팅 완료됨");
    }
}
