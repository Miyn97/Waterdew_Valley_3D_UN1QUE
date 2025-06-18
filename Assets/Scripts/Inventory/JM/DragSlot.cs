using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public Image itemImage;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void SetItem(Sprite icon)
    {
        itemImage.sprite = icon;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position = Input.mousePosition;
        }
    }
}
