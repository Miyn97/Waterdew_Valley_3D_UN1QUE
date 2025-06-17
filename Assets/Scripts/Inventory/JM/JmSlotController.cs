using UnityEngine;
using UnityEngine.EventSystems;

public class JmSlotController : MonoBehaviour, IPointerClickHandler
{
    private float lastClickTime;
    private const float doubleClickThreshold = 0.2f;

    [HideInInspector] public JmInvenManager invenManager;

    // JmSlot 참조
    private JmSlot slot;


    private void Awake()
    {
        // 같은 오브젝트에 붙어 있다고 가정
        slot = GetComponent<JmSlot>();
        invenManager = GetComponentInParent<JmInvenManager>();
    }

    private void Update()
    {
        if (invenManager.nowSelectedSlot != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    invenManager.quickSlotManager.SetQuickSlot(i, invenManager.nowSelectedSlot);

                    Debug.Log($"퀵슬롯 {i + 1}번에 {invenManager.nowSelectedSlot} 등록!");

                    invenManager.nowSelectedSlot = null;
                    break;
                }
            }
        }
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
                //Debug.Log("좌클릭 두 번 감지!");
                OnDoubleClick();
            }
            else
            {
                //Debug.Log("좌클릭 감지!");
                OnSingleClick();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Debug.Log("우클릭 감지!");
            OnRightClick();
        }

        lastClickTime = Time.time;
    }

    private void OnSingleClick()
    {
        if (invenManager.isSelected)
        //    만약 아이템이 선택된 상태라면?
        //    놓는것도 더블클릭이면 많이 불편할 것이다
        {
            if (slot != null && slot.currentItem == null)
            {
                // 두 번째 슬롯 지정
                invenManager.selectedSlot2 = slot;

                //    선택된 아이템을 옮기기!
                invenManager.selectedSlot2.currentItem = invenManager.selectedSlot1.currentItem;
                invenManager.selectedSlot1.currentItem = null;

                invenManager.selectedSlot1.UpdateData();
                invenManager.selectedSlot2.UpdateData();

                //    상태 초기화
                invenManager.selectedSlot1 = null;
                invenManager.selectedSlot2 = null;
                invenManager.isSelected = false;

                Debug.Log("아이템 옮기기 완료!");
            }
            else if (slot != null && slot.currentItem != null)
            {
                // 두 번째 슬롯 지정
                invenManager.selectedSlot2 = slot;

                // 임시 아이템 저장
                var tempItem = invenManager.selectedSlot1.currentItem;

                // 서로 교체!
                invenManager.selectedSlot1.currentItem = invenManager.selectedSlot2.currentItem;
                invenManager.selectedSlot2.currentItem = tempItem;

                invenManager.selectedSlot1.UpdateData();
                invenManager.selectedSlot2.UpdateData();

                // 선택 초기화
                invenManager.selectedSlot1 = null;
                invenManager.selectedSlot2 = null;
                invenManager.isSelected = false;

                Debug.Log("아이템 교체 완료!");
            }
        }
        else
        //    하지만 선택되지 않은 상태라면
        {
            if (slot != null && slot.currentItem != null)
            {
                invenManager.nowSelectedSlot = null;
                //    이전에 선택됬던거 할당 해제하기!

                invenManager.infoManager.UpdateInfoState(slot.currentItem);
                invenManager.nowSelectedSlot = slot;
                Debug.Log("아이템이 정보 확인하기!");
            }
        }
       
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

    private void OnRightClick()
    {
        // 우클릭 기능은 여기에!
    }
}
