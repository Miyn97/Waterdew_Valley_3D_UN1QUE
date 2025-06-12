using UnityEngine;

public interface IBuildableSurface
{
    /// <summary>
    /// 해당 위치 위에 건물 설치가 가능한지 반환
    /// 예: 이미 무언가가 설치되어 있으면 false
    /// </summary>
    bool CanBuildHere(Vector3 worldPosition);

    /// <summary>
    /// 실제 설치될 정확한 위치 반환
    /// 예: 스냅 좌표 반환 (중앙 정렬 등)
    /// </summary>
    Vector3 GetSnappedPosition(Vector3 worldPosition);

    /// <summary>
    /// 실제 설치가 완료되었을 때 호출
    /// 예: 이 위에 무언가가 설치되었다고 내부 상태 업데이트
    /// </summary>
    void RegisterBuild(Vector3 worldPosition);
}
