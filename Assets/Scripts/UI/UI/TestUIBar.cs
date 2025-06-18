using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestUIBar : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 offset;
    private RectTransform targetWindow;  //    움직일 창
    private Canvas canvas;

    private void Awake()
    {
        targetWindow = transform.GetComponent<RectTransform>(); //    창 전체를 움직이도록
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetWindow, eventData.position, eventData.pressEventCamera, out offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                                    canvas.transform as RectTransform,
                                                    eventData.position,
                                                    eventData.pressEventCamera,
                                                    out Vector2 localMousePos))
        {
            targetWindow.anchoredPosition = localMousePos - offset;
        }
    }
}
