using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour
{
    public void UseItem(ItemBaseData data)
    {
        if (data is IUsable usable)
        {
            usable.Use(gameObject);
        }

        if (data is IEquipable equipable)
        {
            equipable.Equip(gameObject);
        }
    }
}
