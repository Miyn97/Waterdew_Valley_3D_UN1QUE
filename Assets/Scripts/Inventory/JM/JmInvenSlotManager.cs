using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmInvenSlotManager : MonoBehaviour
{

    public GameObject slotPrefab;
    public Transform contentParent;

    public List<JmSlot> slotList = new();

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

    public void AddItem(ItemBaseData item)
    //    디버그용
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

    public void AddItem(ItemBaseData item, int? num)
    //    나중에 쓰게 될 것
    {
        int amountToAdd = num ?? 1;

        if (item.IsStackable)
        {
            // 스택 가능한 아이템이면 한번에 추가
            //TryAddToStack(item, amountToAdd);
        }
        else
        {
            // 스택 불가능하면 하나씩 반복해서 추가
            for (int i = 0; i < amountToAdd; i++)
            {
                //TryAddToInventory(item);
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
