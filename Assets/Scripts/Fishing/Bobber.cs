using UnityEngine;
using System.Collections;

public class Bobber : MonoBehaviour
{
    private Transform startPoint; // 출발 위치 (낚싯대 끝)
    [SerializeField] private float throwSpeed = 10f;
    [SerializeField] private LayerMask waterLayer;

    private Vector3 targetPosition;
    private bool isFlying = false;

    public void Throw(Transform _startPoint, Vector3 direction, float distance)
    {
        startPoint = _startPoint;
        targetPosition = transform.position + direction * distance;
        isFlying = true;

        // 낚시 찌가 날아갈 동안 충돌 감지
        // Rigidbody.velocity를 쓰지 않고 직접 이동
        StopAllCoroutines();
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (isFlying && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, throwSpeed * Time.deltaTime);
            yield return null;
        }

        isFlying = false;

        // 충돌 감지는 OnTriggerEnter로 별도 처리
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFlying) return;

        isFlying = false;

        if (((1 << other.gameObject.layer) & waterLayer) != 0)
        {
            // 바다와 닿음 => 낚시 시작
            Debug.Log("바다에 착수!");
            //FishingSystem.Instance.StartFishing(this);
        }
        else
        {
            // 바다 아님 => 복귀
            Debug.Log("바다 아님, Bobber 복귀");
            ReturnToStart();
        }
    }

    public void ReturnToStart()
    {
        StopAllCoroutines();
        transform.position = startPoint.position;
    }
}
