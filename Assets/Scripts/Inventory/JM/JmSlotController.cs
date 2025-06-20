﻿using UnityEngine;
using UnityEngine.EventSystems;

public class JmSlotController : MonoBehaviour, IPointerClickHandler
{
    private float lastClickTime;
    private const float doubleClickThreshold = 0.2f;

    [HideInInspector] public JmInvenManager invenManager;

    //    JmSlot 참조
    private JmSlot slot;


    private void Awake()
    {
        //    같은 오브젝트에 붙어 있다고 가정
        slot = GetComponent<JmSlot>();
        invenManager = GetComponentInParent<JmInvenManager>();
    }

    private void Update()
    {
        GetKeyAndSetQuickSlot();
        //    키 값 읽어오기
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        //    더블클릭 감지

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (timeSinceLastClick <= doubleClickThreshold)
            //    만약 더블클릭 감지 시간 내에 두번 클릭했다면?
            {
                OnDoubleClick();
            }
            else
            //    감지시간이 초기화된 상태라면(어차피 더블클릭도 여길 거친다.....)
            {
                OnSingleClick();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
        invenManager.slotManager.UpdateAllSlots();

        lastClickTime = Time.time;
    }

    private void OnSingleClick()
    {
        if (invenManager.isSelected)
        //    만약 아이템이 선택된 상태라면?
        //    놓는것도 더블클릭이면 많이 불편할 것이다
        {
            if (slot != null && slot.currentItem == null)
            //    만약 슬롯에 아이템이 없다면?
            {
                // 두 번째 슬롯 지정
                invenManager.selectedSlot2 = slot;

                //    선택된 아이템을 옮기기!
                invenManager.selectedSlot2.currentItem = invenManager.selectedSlot1.currentItem;
                invenManager.selectedSlot1.currentItem = null;

                CompliteChange();
            }
            else if (slot != null && slot.currentItem != null)
            {
                //    두 번째 슬롯 지정
                invenManager.selectedSlot2 = slot;

                //    임시 아이템 저장
                var tempItem = invenManager.selectedSlot1.currentItem;

                //    서로 교체!
                invenManager.selectedSlot1.currentItem = invenManager.selectedSlot2.currentItem;
                invenManager.selectedSlot2.currentItem = tempItem;

                CompliteChange();
            }
        }
        else
        //    하지만 아이템이 선택되지 않은 상태라면
        {
            if (slot != null && slot.currentItem != null)
            {
                //    이전에 선택됬던거 할당 해제하기!
                invenManager.nowSelectedSlot = null;

                invenManager.infoManager.UpdateInfoState(slot.currentItem);
                invenManager.nowSelectedSlot = slot;
                Debug.Log("아이템이 정보 확인하기!");
            }
        }

        invenManager.ClearDraggedItem();
    }

    void CompliteChange()
    {
        invenManager.selectedSlot1.UpdateData();
        invenManager.selectedSlot2.UpdateData();

        //    선택 초기화
        invenManager.selectedSlot1 = null;
        invenManager.selectedSlot2 = null;
        invenManager.isSelected = false;
    }

    private void OnDoubleClick()
    {
        if (!invenManager.isSelected)
        //    만약 아이템이 선택 되지 않았다면
        {
            if (slot != null && slot.currentItem != null)
            {
                Debug.Log("아이템이 선택됨!");
                invenManager.infoManager.UpdateInfoState(slot.currentItem);
                //    누른 인벤 슬롯 정보 업데이트 하기(혹여나를 위해)

                invenManager.SetCurrentDraggedItem(slot.currentItem);
                //    드래그 이미지 할당

                invenManager.selectedSlot1 = slot;
                //    인벤토리 매니저에 처음 누른 슬롯 지정하기

                invenManager.isSelected = true;
                //    아이템이 선택됬다는걸 확인하기
            }
            else
            {
                //Debug.LogWarning("클릭된 슬롯에 아이템이 없거나 슬롯 자체가 비어있어요!");
                return;
            }
        }
        
    }

    public void GetKeyAndSetQuickSlot()
    {
        if (invenManager.nowSelectedSlot != null)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !Input.GetKeyDown(KeyCode.Alpha0))
                {
                    if(invenManager.nowSelectedSlot.linkedSlot == null)
                    //    링크된 슬롯이 아직 없다면?
                    {
                        SetSlotInController(i);
                        break;
                    }
                    else
                    //    링크된 슬롯이 있다면
                    {
                        UnsetQuickSlotInController();
                        SetSlotInController(i);
                        break;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                //    할당 해제
                {
                    UnsetQuickSlotInController();
                    invenManager.nowSelectedSlot = null;
                    break;
                }
            }
        }
    }

    //========================================================================
    //    요 아래는 중복되서 함수화 시킨것들
    void SetSlotInController(int index)
    {
        invenManager.quickSlotManager.SetQuickSlot(index, invenManager.nowSelectedSlot);
        invenManager.nowSelectedSlot = null;
    }

    void UnsetQuickSlotInController()
    {
        invenManager.quickSlotManager.UnsetQuickSlot(
                        invenManager.nowSelectedSlot.linkedSlot.slotIndex,
                        invenManager.nowSelectedSlot);
    }
    //========================================================================


    private void OnRightClick()
    {
        //    우클릭 기능은 여기에!
        //    크아아악! 원래는 사용인데!!

        if (slot != null && slot.currentItem != null)
        {            
            invenManager.nowSelectedSlot = null;
            invenManager.nowSelectedSlot = slot;
            slot.ClearSlot();
            invenManager.slotManager.UpdateAllSlots();

            Debug.Log("아이템이 정보 확인하기!");
        }
    }

}
