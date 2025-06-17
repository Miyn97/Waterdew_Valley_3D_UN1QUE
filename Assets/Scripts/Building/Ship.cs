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
        Vector2Int size = tile.size;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int offset = new(position.x + x, position.y + y);
                if (!IsOccupied(offset))
                {
                    placedTiles.Add(offset, tile);
                }
            }
        }
    }

    public List<Vector2Int> GetBuildablePositions()
    {
        HashSet<Vector2Int> result = new();

        Vector2Int[] directions = {
        Vector2Int.up, Vector2Int.down,
        Vector2Int.left, Vector2Int.right
    };

        foreach (var tile in placedTiles.Values)
        {
            Vector2Int pos = tile.gridPosition;
            Vector2Int size = tile.size;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int current = pos + new Vector2Int(x, y);
                    foreach (var dir in directions)
                    {
                        Vector2Int neighbor = current + dir;
                        if (!IsOccupied(neighbor))
                        {
                            result.Add(neighbor);
                        }
                    }
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
                Vector3 localPos = child.localPosition;
                Vector2Int gridPos = new Vector2Int(
                    Mathf.RoundToInt(localPos.x),
                    Mathf.RoundToInt(localPos.z)
                );

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
