using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmInvenManager : MonoBehaviour
{
    public JmInvenSlotManager slotManager;
    public JmInvenSizeManager sizeManager;
    public JmInvenInfoManager infoManager;

    public int defaultColumns = 5;
    public int defaultRows = 2;

    [Header("퀵슬롯")]
    public JmQuickSlotManager quickSlotManager;

    [Header("선택 관련")]
    public JmSlot nowSelectedSlot;
    //    현재 선택된 슬롯(퀵슬롯 설정용)

    public bool isSelected = false;

    public JmSlot selectedSlot1;
    //    처음 선택된 슬롯
    public JmSlot selectedSlot2;
    //    두번째로 선택된 슬롯


    [Header("디버그용")]
    public ItemBaseData item1;
    public ItemBaseData item2;

    private void Awake()
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
        slotManager.AddItem(item1);
    }

    public void AddItem2()
    {
        slotManager.AddItem(item2);
    }
}
