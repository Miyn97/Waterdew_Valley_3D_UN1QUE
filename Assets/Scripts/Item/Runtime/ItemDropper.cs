using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public void DropItem(ItemBaseData item, int amount)
    {
        GameObject drop = Instantiate(item.ItemPrefab, transform.position + transform.forward, Quaternion.identity);
        var pickup = drop.GetComponent<ItemPickup>();
        pickup.itemData = item;
        pickup.quantity = amount;
    }
}
