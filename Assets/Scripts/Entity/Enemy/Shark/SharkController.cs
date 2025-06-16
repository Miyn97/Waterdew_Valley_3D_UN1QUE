using UnityEngine;

public class SharkController : EnemyController
{
    [Header("Shark Specific")]
    public Transform headTransform; // 상어 머리 위치 (공격 기준점)

    public override void Attack(Transform target)
    {
        Vector3 attackOrigin = headTransform != null ? headTransform.position : transform.position;

        if (target.CompareTag("Player"))
        {
            PlayerStatus status = target.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.TakeDamage(20f);
                Debug.Log("Shark가 플레이어에게 피해를 입혔습니다.");
            }
        }
        else if (target.CompareTag("Raft"))
        {
            Transform closestCorner = GetClosestCorner(target, attackOrigin);
            if (closestCorner != null)
            {
                Debug.Log($"Shark가 뗏목의 가장 가까운 모서리({closestCorner.position})를 공격했습니다.");
                // 예시: closestCorner.GetComponent<RaftPart>()?.TakeDamage(10f);
            }
        }
    }

    // 뗏목의 가장 가까운 모서리를 계산
    private Transform GetClosestCorner(Transform raftTransform, Vector3 fromPosition)
    {
        float halfSize = 0.5f; // Raft 한 타일 크기 기준
        Vector3[] cornerOffsets =
        {
            new Vector3(-halfSize, 0, -halfSize),
            new Vector3(-halfSize, 0, halfSize),
            new Vector3(halfSize, 0, -halfSize),
            new Vector3(halfSize, 0, halfSize),
        };

        float minDistance = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;

        foreach (var offset in cornerOffsets)
        {
            Vector3 corner = raftTransform.position + offset;
            float distance = Vector3.Distance(fromPosition, corner);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = corner;
            }
        }

        // 위치만 필요한 경우 Transform 대신 Vector3로도 충분하지만, 필요한 경우 실제 Transform으로 반환
        Transform temp = new GameObject("TempCorner").transform;
        temp.position = closestPoint;
        return temp;
    }
}
