using System.Collections.Generic;

using UnityEngine;

public class BuildingPreview : MonoBehaviour, IBuildableObject
{
    private int rotation = 0;

    // 예시: 2x1 건물
    private List<Vector2Int> baseOffsets = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0)
    };

    private List<Vector2Int> currentOffsets = new List<Vector2Int>();

    private void Awake()
    {
        UpdateOffsets();
    }

    public void Rotate()
    {
        rotation = (rotation + 90) % 360;
        transform.Rotate(0, 90, 0);
        UpdateOffsets();
    }

    public int GetRotation()
    {
        return rotation;
    }

    public List<Vector2Int> GetOccupiedObjOffsets()
    {
        return currentOffsets;
    }

    private void UpdateOffsets()
    {
        currentOffsets.Clear();

        foreach (var offset in baseOffsets)
        {
            Vector2Int rotated = RotateOffset(offset, rotation);
            currentOffsets.Add(rotated);
        }
    }

    private Vector2Int RotateOffset(Vector2Int offset, int angle)
    {
        switch (angle)
        {
            case 0: return offset;
            case 90: return new Vector2Int(-offset.y, offset.x);
            case 180: return new Vector2Int(-offset.x, -offset.y);
            case 270: return new Vector2Int(offset.y, -offset.x);
            default: return offset;
        }
    }
}
