using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JMInven : MonoBehaviour
{
    [Header("인벤토리 사이즈 관련")]
    public int columns = 5;
    public int rows = 2;

    public GameObject slotPrefab; //    슬롯 프리팹
    public Transform contentParent; //    ScrollView → Content
    public RectTransform contentRect;

    public float slotWidth = 90f;  //    슬롯 하나의 가로 크기(px)
    public float slotHeight = 90f; //    슬롯 하나의 세로 크기(px)
    public float spacingX = 20f;     //    슬롯 간 가로 간격
    public float spacingY = 20f;     //    슬롯 간 세로 간격

    public RectTransform mainInventoryBaseRect;  //    메인 인벤토리 베이스  
    public RectTransform inventoryAreaRect;      //    크기 조절할 인벤토리 구역(스크롤뷰 같은)
    private float originalBaseHeight;            //    초기 전체 높이
    private float originalInventoryHeight;       //    초기 인벤토리 영역 높이


    private List<JmSlot> slotList = new();

    [Header("테스트용")]
    public ItemBaseData item1;
    public ItemBaseData item2;


    void Start()
    {
        // 초기 값 저장
        originalBaseHeight = mainInventoryBaseRect.sizeDelta.y;
        originalInventoryHeight = inventoryAreaRect.sizeDelta.y;

        GenerateSlots();
        ResizeContent();
    }

    public void RefreshInventoryUI(int newColumns, int newRows)
    //    전체적인 인벤토리 사이즈의 정보를 불러오는 것

    {
        columns = newColumns; //    열(가로)
        rows = newRows; //    행(세로)

        GenerateSlots();
        ResizeContent();
        ResizeMainInventoryBase(); //    부모 UI까지 리사이징
    }

    void ResizeContent()
    {
        //    패딩 값 구하기
        var grid = contentParent.GetComponent<GridLayoutGroup>();
        var padding = grid.padding;

        float paddingLeft = padding.left;
        float paddingRight = padding.right;
        float paddingTop = padding.top;
        float paddingBottom = padding.bottom;

        if (contentRect != null)
        {
            float totalWidth = columns * (slotWidth + spacingX) - spacingX + paddingLeft + paddingRight + 10;
            float totalHeight = rows * (slotHeight + spacingY) - spacingY + paddingTop + paddingBottom + 10;
            //    +10을 해주는 이유는 원래 스크롤 바가 차지하는 영역을 따로 빼고 계산한 값이기 때문
            //    GPT에게 물어보고 만든 내용이라 나중에 +10을 어떻게 하면 안넣을지 스스로 해석해야할 필요가 있음

            contentRect.sizeDelta = new Vector2(totalWidth, totalHeight);
        }

        //    인벤토리 배경 사이즈 변경
        ResizeMainInventoryBase();
    }

    public void ResizeMainInventoryBase()
    {
        var vertical = mainInventoryBaseRect.GetComponent<VerticalLayoutGroup>();
        var padding = vertical.padding;

        float currentInventoryHeight = inventoryAreaRect.sizeDelta.y;

        //    기본 베이스 높이에서 기존 인벤토리 높이를 빼고, 현재 인벤토리 높이를 더함
        float newHeight = originalBaseHeight - originalInventoryHeight + currentInventoryHeight;

        //    너비도 계산할 수 있음
        float newWidth = inventoryAreaRect.sizeDelta.x + padding.left + padding.right;

        mainInventoryBaseRect.sizeDelta = new Vector2(newWidth, newHeight);
    }


    public void GenerateSlots()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        slotList.Clear();

        int totalSlots = columns * rows;

        for (int i = 0; i < totalSlots; i++)
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

    //    테스트용 함수들

    public void GetItem1()
    {
        AddItemToFirstEmptySlot(item1);
    }

    public void GetItem2()
    {
        AddItemToFirstEmptySlot(item2);
    }

    
    private void AddItemToFirstEmptySlot(ItemBaseData item)
    //    아이템을 넣을 때 빈 슬롯을 찾아가게 만들기
    {
        if (item == null)
        {
            //    AddItemToFirstEmptySlot: item이 null!!
            return;
        }

        foreach (var slot in slotList)
        {
            if (slot == null)
            {
                //    slotList 가 null이면!
                continue;
            }

            if (slot.currentItem == null)
            {
                slot.currentItem = item;
                slot.UpdateData();
                Debug.Log($"'{item.name}' 아이템을 슬롯 {slot.slotIndex}에 넣었어!");
                break;
            }
        }
    }




}
