using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var slot in inventory.slots)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            var ui = go.GetComponent<UIItemSlot>();
            ui.Set(slot.itemData, slot.quantity);
        }
    }
}
