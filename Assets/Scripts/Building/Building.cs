using UnityEngine;

public class Building : MonoBehaviour, IBuildableSurface
{
    public BuildingData data;
    [SerializeField] private bool isOccupied = false;  // 뭔가 이미 설치되어 있는가?

    public bool CanBuildHere(Vector3 worldPosition)
    {
        return !isOccupied;
    }

    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        // 이 건물의 정중앙 위치로 스냅
        return transform.position;
    }


    public void RegisterBuild(Vector3 worldPosition)
    {
        isOccupied = true;
    }

    // 필요 시 설치 해제도 가능하게 만들 수 있음
    public void UnregisterBuild()
    {
        isOccupied = false;
    }
}
