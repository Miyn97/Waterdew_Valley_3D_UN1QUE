using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JmInvenSizeManager : MonoBehaviour
{
    [Header("참조")]
    public RectTransform contentRect;
    public RectTransform mainInventoryBaseRect;
    public RectTransform inventoryAreaRect;

    [Header("슬롯 사이즈")]
    public float slotWidth = 90f;
    public float slotHeight = 90f;

    [Header("스캐핑(갭)")]
    public float spacingX = 20f;
    public float spacingY = 20f;

    public Vector2 baseMainInventorySize = new Vector2(640, 770);
    public int baseColumns = 5;
    public int baseRows = 2;

    public void ResizeContent(int columns, int rows)
    {
        GridLayoutGroup grid = contentRect.GetComponent<GridLayoutGroup>();
        RectOffset padding = grid.padding;

        float totalWidth = columns * (slotWidth + spacingX) - spacingX + padding.left + padding.right;
        float totalHeight = rows * (slotHeight + spacingY) - spacingY + padding.top + padding.bottom;

        contentRect.sizeDelta = new Vector2(totalWidth, totalHeight);

        // 여기가 핵심!
        inventoryAreaRect.sizeDelta = new Vector2(inventoryAreaRect.sizeDelta.x, totalHeight);

        ResizeMainInventoryBase(columns, rows);
    }

    public void ResizeMainInventoryBase(int columns, int rows)
    {
        var vertical = mainInventoryBaseRect.GetComponent<VerticalLayoutGroup>();

        // contentRect 크기 기준으로 증가분 계산
        float widthGrowth = (columns - baseColumns) * (slotWidth + spacingX);
        float heightGrowth = (rows - baseRows) * (slotHeight + spacingY);

        float newWidth = baseMainInventorySize.x + widthGrowth;
        float newHeight = baseMainInventorySize.y + heightGrowth;

        mainInventoryBaseRect.sizeDelta = new Vector2(newWidth, newHeight);

    }
}
