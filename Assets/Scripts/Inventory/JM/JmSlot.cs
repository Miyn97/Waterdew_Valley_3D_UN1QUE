using UnityEngine;
using UnityEngine.UI;

public class JmSlot : MonoBehaviour
{
    public TestItem currentItem;
    public Image iconImage;

    public int slotIndex;
    
    public void SetIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateData()
    {
        if (currentItem != null && currentItem.icon != null)
            iconImage.sprite = currentItem.icon;
        else
            iconImage.sprite = null;
    }
}
