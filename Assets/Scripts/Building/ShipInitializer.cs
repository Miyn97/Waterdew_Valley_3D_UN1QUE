using UnityEngine;

public class ShipInitializer : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform shipTransform;

    private void Awake()
    {
        GenerateInitialTiles();
    }

    void GenerateInitialTiles()
    {
        // 2x2 타일 생성: 위치 (0,0), (0,1), (1,0), (1,1)
        for (int x = 0; x <= 1; x++)
        {
            for (int z = 0; z <= 1; z++)
            {
                Vector3 position = new Vector3(x, 0, z); // y = 0은 바닥 높이
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, shipTransform);

                tile.name = $"Tile_{x}_{z}";

                // 선택사항: 타일 스크립트가 있다면 좌표 저장
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.gridPosition = new Vector2Int(x, z);
                }
            }
        }
    }
}
