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

    public void ResizeContent(int columns, int rows)
    {
        GridLayoutGroup grid = contentRect.GetComponent<GridLayoutGroup>();
        RectOffset padding = grid.padding;

        float totalWidth = columns * (slotWidth + spacingX) - spacingX + padding.left + padding.right;
        float totalHeight = rows * (slotHeight + spacingY) - spacingY + padding.top + padding.bottom;

        contentRect.sizeDelta = new Vector2(totalWidth, totalHeight);
        ResizeMainInventoryBase();
    }

    public void ResizeMainInventoryBase()
    {
        var vertical = mainInventoryBaseRect.GetComponent<VerticalLayoutGroup>();
        var padding = vertical.padding;

        float currentInventoryHeight = inventoryAreaRect.sizeDelta.y;
        float newHeight = mainInventoryBaseRect.sizeDelta.y - inventoryAreaRect.sizeDelta.y + currentInventoryHeight;
        float newWidth = inventoryAreaRect.sizeDelta.x + padding.left + padding.right;

        mainInventoryBaseRect.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
