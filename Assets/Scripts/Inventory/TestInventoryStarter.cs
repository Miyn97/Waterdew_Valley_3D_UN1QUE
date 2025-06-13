using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryStarter : MonoBehaviour
{
    public Inventory inventory;
    public ItemBaseData testItem1;
    public ItemBaseData testItem2;

    private void Start()
    {
        inventory.AddItem(testItem1, 3);
        inventory.AddItem(testItem2, 1);
        inventory.AddItem(testItem1, 2);

        inventory.UseItem(testItem1);
    }
}
