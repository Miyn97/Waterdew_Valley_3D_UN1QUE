using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JmQuickSlotManager : MonoBehaviour
{
    public List<JmQuickSlot> quickSlots = new();

    public GameObject slotPrefab;
    public Transform contentParent;

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
    {
        JmQuickSlot slot = quickSlots[index];
        if (slot.linkedSlot != null && slot.linkedSlot.currentItem != null)
        {
            Debug.Log($"[퀵슬롯] {index + 1}번 슬롯: {slot.linkedSlot.currentItem.itemName} 사용!");
            // 여기서 아이템 사용 로직 호출하면 돼!
        }
        else
        {
            Debug.Log($"[퀵슬롯] {index + 1}번 슬롯에 아무것도 연결되지 않음!");
        }
    }

}
