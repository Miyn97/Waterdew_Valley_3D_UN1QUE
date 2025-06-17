using UnityEngine;
using UnityEngine.UI;

public class JmSlot : MonoBehaviour
{
    public ItemBaseData currentItem;
    public Image iconImage;

    public int slotIndex;


    public void SetIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateData()
    {
        if (currentItem != null && currentItem.Icon != null)
            iconImage.sprite = currentItem.Icon;
        else
            iconImage.sprite = null;
    }
}
