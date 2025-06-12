using System.Collections.Generic;

using UnityEngine;

public interface IBuildableObject
{
    /// <summary>
    /// 설치에 필요한 타일 상대 좌표 리스트 (자기 중심 기준)
    /// 예: Vector2Int(0,0), (1,0) 등
    /// 회전 정보에 따라 바뀔 수 있음
    /// </summary>
    List<Vector2Int> GetOccupiedObjOffsets();

    /// <summary>
    /// 회전 (예: 90도씩)
    /// 회전 후엔 OccupiedObjOffsets도 갱신되어야 함
    /// </summary>
    void Rotate();

    /// <summary>
    /// 회전 상태 반환 (0, 90, 180, 270 등)
    /// 필요에 따라 Vector3 또는 int로
    /// </summary>
    int GetRotation();
}
