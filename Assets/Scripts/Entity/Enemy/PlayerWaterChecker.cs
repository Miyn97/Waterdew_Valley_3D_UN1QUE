using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterChecker : MonoBehaviour
{
    public EnemyController enemy;

    void Update()
    {
        // 임시 : Y 좌표가 0.5 이하이면 물 속으로 간주
        enemy.isPlayerInWater = transform.position.y < 0.5f;
    }
}
