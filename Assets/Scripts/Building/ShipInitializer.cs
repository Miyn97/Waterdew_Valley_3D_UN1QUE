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
        Vector3 position = Vector3.zero; // 시작 위치
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, shipTransform);

        Tile tileScript = tile.GetComponent<Tile>();
        if (tileScript != null)
        {
            tileScript.gridPosition = new Vector2Int(0, 0); // 기준이 되는 위치
        }
    }
}
