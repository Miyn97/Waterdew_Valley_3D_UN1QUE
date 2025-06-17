using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JmQuickSlot : JmSlot
{
    public JmSlot linkedSlot = null;

    public void SetLinkedSlot(JmSlot targetSlot)
    {
        if(linkedSlot != null)
        {
            linkedSlot = null;
        }

        linkedSlot = targetSlot;
    }

}
