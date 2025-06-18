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
        GameObject tile = Instantiate(tilePrefab, shipTransform);
        tile.transform.localPosition = new Vector3(0, -1, 0);

        Tile tileScript = tile.GetComponent<Tile>();
        if (tileScript != null)
        {
            Vector3 localPos = tile.transform.localPosition;
            Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(localPos.x), Mathf.RoundToInt(localPos.z));
            tileScript.gridPosition = gridPos;
        }
    }
}
