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

    public bool HasItem(ItemBaseData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item && slot.quantity >= amount)
                return true;
        }
        return false;
    }

    public bool AddItem(ItemBaseData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item && item.IsStackable)
            {
                slot.quantity += amount;
                return true;
            }
        }

        slots.Add(new InventorySlot(item, amount));
        return true;
    }

    public bool UseItem(ItemBaseData item)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item && slot.quantity > 0)
            {
                if (item is IUsable usable)
                {
                    usable.Use(gameObject);
                }

                slot.quantity--;
                if (slot.quantity <= 0)
                    slots.Remove(slot);

                return true;
            }
        }

        return false;
    }

    public void RemoveItem(ItemBaseData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                if (slot.quantity >= amount)
                {
                    slot.quantity -= amount;
                    if (slot.quantity <= 0)
                    {
                        slots.Remove(slot);
                    }
                    return;
                }
            }
        }
    }
}
