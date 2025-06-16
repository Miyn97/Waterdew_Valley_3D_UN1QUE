using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JmDragManager : MonoBehaviour
{
    public static JmDragManager Instance;

    public bool IsDragging => draggingItem != null;
    public JmSlot originSlot;

    public Image draggingItemImage; // Canvas 위에 떠있는 이미지
    private TestItem draggingItem;

    void Awake() => Instance = this;

    public void BeginDrag(JmSlot fromSlot)
    {
        if (fromSlot.currentItem == null)
        {
            Debug.Log("BeginDrag 실패: currentItem이 null입니다!");
            return;
        }

        originSlot = fromSlot;
        draggingItem = fromSlot.currentItem;

        draggingItemImage.sprite = draggingItem.icon;
        draggingItemImage.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (IsDragging)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                draggingItemImage.canvas.transform as RectTransform,
                Input.mousePosition,
                draggingItemImage.canvas.worldCamera,
                out pos
            );
            draggingItemImage.rectTransform.anchoredPosition = pos;
        }
    }

    public void EndDrag()
    {
        draggingItemImage.gameObject.SetActive(false);
        draggingItem = null;
        originSlot = null;
    }

    public void CancelDrag()
    {
        // 아무 데도 못 놓은 경우
        EndDrag();
    }
}
