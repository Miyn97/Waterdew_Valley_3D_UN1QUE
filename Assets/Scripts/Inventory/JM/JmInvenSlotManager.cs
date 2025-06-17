using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmInvenSlotManager : MonoBehaviour
{

    public GameObject slotPrefab;
    public Transform contentParent;

    private List<JmSlot> slotList = new();

    public void Init(int columns, int rows)
    {
        ClearSlots();
        GenerateSlots(columns, rows);
    }

    void GenerateSlots(int columns, int rows)
    {
        int total = columns * rows;
        for (int i = 0; i < total; i++)
        {
            GameObject obj = Instantiate(slotPrefab, contentParent);
            JmSlot slot = obj.GetComponent<JmSlot>();
            slot.SetIndex(i);
            slotList.Add(slot);
        }
    }

    public void UpdateAllSlots()
    {
        foreach (var slot in slotList)
        {
            slot.UpdateData();
        }
    }

    public void AddItemToFirstEmptySlot(ItemBaseData item)
    {
        foreach (var slot in slotList)
        {
            if (slot.currentItem == null)
            {
                slot.currentItem = item;
                slot.UpdateData();
                break;
            }
        }
    }


    void ClearSlots()
    //    초기 동작용
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        slotList.Clear();
    }

   
}
