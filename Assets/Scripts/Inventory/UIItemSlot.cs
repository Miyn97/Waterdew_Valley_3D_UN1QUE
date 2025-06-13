using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    private ItemBaseData item;
    private int quantity;

    public void Set(ItemBaseData data, int count)
    {
        item = data;
        quantity = count;
        icon.sprite = data.Icon;
        countText.text = data.IsStackable ? count.ToString() : "";
    }

    public void OnClick()
    {
        FindObjectOfType<Inventory>().UseItem(item);
        FindObjectOfType<UIInventory>().Refresh();
    }
}
