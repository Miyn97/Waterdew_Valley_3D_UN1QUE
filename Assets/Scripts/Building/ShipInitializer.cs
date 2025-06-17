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
        Vector3 position = new Vector3(0, -1, 0);
        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, shipTransform);

        Tile tileScript = tile.GetComponent<Tile>();
        if (tileScript != null)
        {
            tileScript.gridPosition = Vector2Int.zero;
        }
    }
}
