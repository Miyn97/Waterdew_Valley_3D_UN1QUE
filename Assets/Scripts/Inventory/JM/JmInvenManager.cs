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

    [Header("선택 관련")]
    public ItemBaseData nowSelectedItem;
    //    나중에 아이템 데이터 넣는다면 이곳 바꾸기!
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
        slotManager.AddItemToFirstEmptySlot(item1);
    }

    public void AddItem2()
    {
        slotManager.AddItemToFirstEmptySlot(item2);
    }
}
