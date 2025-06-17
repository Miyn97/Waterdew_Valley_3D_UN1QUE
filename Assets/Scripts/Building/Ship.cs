using System.Collections.Generic;

using UnityEngine;

public class Ship : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> placedTiles = new();

    public bool IsOccupied(Vector2Int pos)
    {
        return placedTiles.ContainsKey(pos);
    }

    public void RegisterTile(Vector2Int position, Tile tile)
    {
        if (!IsOccupied(position))
            placedTiles.Add(position, tile);
    }

    public void UnregisterTile(Vector2Int position)
    {
        if (IsOccupied(position))
            placedTiles.Remove(position);
    }

    public List<Vector2Int> GetBuildablePositions()
    {
        HashSet<Vector2Int> result = new();

        Vector2Int[] directions = {
            new Vector2Int(0, 2), new Vector2Int(0, -2),
            new Vector2Int(-2, 0), new Vector2Int(2, 0)
        };

        foreach (Tile tile in placedTiles.Values)
        {
            foreach (var dir in directions)
            {
                Vector2Int neighbor = tile.gridPosition + dir;
                if (!IsOccupied(neighbor))
                {
                    result.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(result);
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile != null)
            {
                Vector3 pos = child.position;
                Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
                tile.gridPosition = gridPos;

                RegisterTile(gridPos, tile);
            }
            else
            {
                Debug.LogWarning($"{child.name} 에 Tile 컴포넌트가 없습니다.");
            }
        }
    }
}
