using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemBaseData itemData;
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemData, quantity);

                var ui = FindObjectOfType<UIInventory>();
                if (ui != null) ui.Refresh();

                Destroy(gameObject);
            }
        }
    }
}
