using UnityEngine;


public class SharkController : EnemyController
{
    [Header("상어 공격 판정 위치 설정")]
    public Transform headTransform; // 공격 기준점 (머리 위치)

    public override void Attack(Transform target)
    {
        Vector3 attackOrigin = headTransform ? headTransform.position : transform.position;

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
            Vector3 closestCorner = GetClosestCornerPosition(target.position, attackOrigin);
            Debug.DrawLine(attackOrigin, closestCorner, Color.red, 1f); // 디버깅용
            Debug.Log($"Shark가 뗏목의 가장 가까운 모서리({closestCorner})를 공격했습니다.");

            // Raft 내구도 감소 로직 추가 필요
        }
    }

    public Vector3 GetClosestCornerPosition(Vector3 raftCenter, Vector3 fromPosition)
    {
        float halfSize = 5f; // 타일의 절반
        Vector3[] cornerOffsets =
        {
            new Vector3(-halfSize, 0, -halfSize),
            new Vector3(-halfSize, 0, halfSize),
            new Vector3(halfSize, 0, -halfSize),
            new Vector3(halfSize, 0, halfSize),
        };

        Vector3 closestCorner = Vector3.zero;
        float minDistance = Mathf.Infinity;

        foreach (var offset in cornerOffsets)
        {
            Vector3 cornerPos = raftCenter + offset;
            float dist = Vector3.Distance(fromPosition, cornerPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestCorner = cornerPos;
            }
        }

        return closestCorner;
    }
}
