using UnityEngine;

public class Tile : MonoBehaviour, IBuildableSurface
{
    [SerializeField] private BuildingData data;
    [SerializeField] private bool isOccupied = false;  // 타일 위에 이미 설치되어 있는가?
    private Ship ship;

    private float maxHealth = 200f;
    private float tileHealth = 200f;
    public float MaxHealth { get; private set; }
    public float TileHealth { get; private set; }

    public Vector2Int gridPosition; // 이 타일의 위치

    public bool CanBuildHere(Vector3 worldPosition)
    {
        return !isOccupied;
    }

    public Vector3 GetSnappedPosition(Vector3 worldPosition)
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

    public void Init(Ship shipRef)
    {
        ship = shipRef;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("아파요");
        tileHealth = Mathf.Clamp(tileHealth - damage, 0f, maxHealth);

        if (tileHealth <= 0f)
        {
            DestroyTile();
        }
    }

    public void DestroyTile()
    {
        Debug.Log("부서져요");
        ship.UnregisterTile(gridPosition);
        Destroy(gameObject);
    }
}
