using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmInvenManager : MonoBehaviour
{
    public JmInvenSlotManager slotManager;
    public JmInvenSizeManager sizeManager;

    public int defaultColumns = 5;
    public int defaultRows = 2;


    public TestItem item1;
    public TestItem item2;

    void Start()
    {
        slotManager.Init(defaultColumns, defaultRows);
        sizeManager.ResizeContent(defaultColumns, defaultRows);
    }

    public void ResizeInventory(int newCols, int newRows)
    {
        slotManager.Init(newCols, newRows);
        sizeManager.ResizeContent(newCols, newRows);
    }

    public void AddItem1()
    {
        slotManager.AddItemToFirstEmptySlot(item1);
    }

    public void AddItem2()
    {
        slotManager.AddItemToFirstEmptySlot(item2);
    }
}
