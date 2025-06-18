using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JmQuickSlot : JmSlot
{
    public GameObject selectedHighlight;

    new public void UpdateData()
    {
        if (linkedSlot == null)
        {
            iconImage.sprite = defaultSprite;
        }
        else
        {
            iconImage.sprite = linkedSlot.currentItem.Icon;
        }   
    }

    public int GetIndex()
    {
        return slotIndex;
    }
}
