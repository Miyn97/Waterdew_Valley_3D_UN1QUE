using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JmQuickSlot : JmSlot
{
    public JmSlot linkedSlot = null;

    new public void UpdateData()
    {
        iconImage.sprite = linkedSlot.currentItem.Icon;
    }
}
