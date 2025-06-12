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
            inventory.AddItem(itemData, quantity);
            Destroy(gameObject);
        }
    }
}
