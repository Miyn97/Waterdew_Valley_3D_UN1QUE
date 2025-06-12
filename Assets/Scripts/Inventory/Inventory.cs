using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public class InventorySlot
    {
        public ItemBaseData itemData;
        public int quantity;

        public InventorySlot(ItemBaseData itemData, int quantity)
        {
            this.itemData = itemData;
            this.quantity = quantity;
        }
    }

    public List<InventorySlot> slots = new List<InventorySlot>();

    public void AddItem(ItemBaseData item, int amount = 1)
    {
        InventorySlot foundSlot = null;

        // 슬롯 순회해서 같은 아이템이 있는지 찾기
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData == item)
            {
                foundSlot = slot;
                break;
            }
        }

        if (foundSlot != null)
        {
            foundSlot.quantity += amount;
            Debug.Log($"[인벤토리] {item.ItemName} 수량 증가 → {foundSlot.quantity}");
        }
        else
        {
            slots.Add(new InventorySlot(item, amount));
            Debug.Log($"[인벤토리] {item.ItemName} 추가됨 → 수량: {amount}");
        }
    }

    public void UseItem(ItemBaseData item)
    {
        InventorySlot foundSlot = null;

        // 슬롯에서 해당 아이템 찾기
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData == item)
            {
                foundSlot = slot;
                break;
            }
        }

        if (foundSlot == null || foundSlot.quantity <= 0)
        {
            Debug.Log($"[인벤토리] {item.ItemName} 없음");
            return;
        }

        if (item is IUsable usable)
        {
            usable.Use(gameObject); // 플레이어에게 효과 적용
            foundSlot.quantity--;

            if (foundSlot.quantity <= 0)
            {
                slots.Remove(foundSlot);
                Debug.Log($"[인벤토리] {item.ItemName} 모두 사용됨 → 슬롯 제거");
            }
            else
            {
                Debug.Log($"[인벤토리] {item.ItemName} 사용됨 → 남은 수량: {foundSlot.quantity}");
            }
        }
        else
        {
            Debug.Log($"[인벤토리] {item.ItemName}은(는) 사용할 수 없음");
        }
    }
}
