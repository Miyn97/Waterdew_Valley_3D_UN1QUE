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
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, shipTransform);

                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.gridPosition = new Vector2Int(x, z);
                }
            }
        }
    }
}
