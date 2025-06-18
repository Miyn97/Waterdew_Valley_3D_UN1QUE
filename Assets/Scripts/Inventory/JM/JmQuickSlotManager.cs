using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmQuickSlotManager : MonoBehaviour
{
    public List<JmQuickSlot> quickSlots = new();

    public GameObject slotPrefab;
    public Transform contentParent;


    public JmQuickSlot currSlot;

    void Start()
    {
        GenerateSlots(9);
        //    우선 9개로 지정
        //    이 부분은 변동이 없어보임
    }

    void GenerateSlots(int index)
    {
        for (int i = 0; i < index; i++)
        {
            GameObject obj = Instantiate(slotPrefab, contentParent);
            JmQuickSlot slot = obj.GetComponent<JmQuickSlot>();
            slot.SetIndex(i);
            quickSlots.Add(slot);
        }
    }

    private void Update()
    {
        for (int i = 0; i < quickSlots.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TryActivateQuickSlot(i);
            }
        }
    }

    void TryActivateQuickSlot(int index)
    //    퀵슬롯 사용 함수
    {
        JmQuickSlot slot = quickSlots[index];

        OffAllHighlight();
        if (slot.linkedSlot != null && slot.linkedSlot.currentItem != null)
        {
            currSlot = quickSlots[index];
            Debug.Log($"[퀵슬롯] {index + 1}번 슬롯: {slot.linkedSlot.currentItem.itemName} 사용!");

            quickSlots[index].selectedHighlight.SetActive(true);
        }
        else
        {
            currSlot = quickSlots[index];
            Debug.Log($"[퀵슬롯] {index + 1}번 슬롯에 아무것도 연결되지 않음!");

            quickSlots[index].selectedHighlight.SetActive(true);
        }
    }

    void OffAllHighlight()
    //    혹시 몰라 함수화
    {
        foreach (JmQuickSlot slot in quickSlots)
        {
            slot.selectedHighlight.SetActive(false);
        }
    }

    public void SetQuickSlot(int index, JmSlot slot)
    //    퀵슬롯 할당
    {
        if (slot == null)
        {
            Debug.LogWarning("SetQuickSlot: 전달된 slot이 null이야!");
            return;
        }

        //    이미 퀵슬롯에 등록된 슬롯이 있다면, 그 슬롯의 연결 끊기!
        if (slot.linkedSlot != null)
        {
            //    이전에 연결된 퀵슬롯을 가져와서 해당 퀵슬롯의 linkedSlot 해제
            slot.linkedSlot.linkedSlot = null;
            slot.linkedSlot.UpdateData();

            slot.linkedSlot = null;
        }

        //    새로 할당될 퀵슬롯
        JmQuickSlot newSlot = quickSlots[index];

        //    이 퀵슬롯이 이미 다른 슬롯과 연결되어 있으면 그 연결도 해제
        if (newSlot.linkedSlot != null)
        {
            newSlot.linkedSlot.linkedSlot = null;
            newSlot.linkedSlot.quickSlotHighlight.SetActive(false);
            newSlot.linkedSlot.UpdateData();

            newSlot.linkedSlot = null;
        }

        //    연결 고리 만들기
        newSlot.linkedSlot = slot;
        slot.linkedSlot = newSlot;

        //    퀵슬롯 등록 표시
        slot.quickSlotHighlight.SetActive(true);

        //    정보 갱신
        slot.UpdateData();
        newSlot.UpdateData();
    }


    public void UnsetQuickSlot(int index, JmSlot slot)
    {
        //    퀵슬롯에 등록되지 않았으니 표시 끄기
        slot.quickSlotHighlight.SetActive(false);

        //    편하게 쓰기 위해 변수화
        JmQuickSlot newSlot = quickSlots[index];

        //    서로의 연결고리 끊어주기
        newSlot.linkedSlot.linkedSlot = null;
        newSlot.linkedSlot = null;

        //    정보 업데이트
        slot.UpdateData();
        newSlot.UpdateData();
    }


}
