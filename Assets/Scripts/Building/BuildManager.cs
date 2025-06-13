using System.Collections.Generic;

using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public BuildPreview buildPreview;
    public Ship ship;

    void Update()
    {
        Vector3 worldPos = GetMouseWorldPosition();
        Vector2Int gridPos = WorldToGrid(worldPos);

        List<Vector2Int> buildable = ship.GetBuildablePositions();

        if (buildable.Contains(gridPos))
        {
            buildPreview.SetValid(true);
            buildPreview.SetPosition(GridToWorld(gridPos));
        }
        else
        {
            buildPreview.SetValid(false);
            buildPreview.SetPosition(worldPos);
        }

        if (Input.GetKeyDown(KeyCode.T) && buildable.Contains(gridPos))
        {
            GameObject newTile = Instantiate(tilePrefab, GridToWorld(gridPos), Quaternion.identity, ship.transform);
            Tile tileScript = newTile.GetComponent<Tile>();
            tileScript.gridPosition = gridPos;
            ship.RegisterTile(gridPos, tileScript);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    Vector2Int WorldToGrid(Vector3 world)
    {
        return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.z));
    }

    Vector3 GridToWorld(Vector2Int grid)
    {
        return new Vector3(grid.x, 0, grid.y);
    }
}
