﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment")]
public class EquipmentItemData : ItemBaseData, IEquipable
{
    [Header("장비 속성")]
    [SerializeField] private EquipmentSlot slot;

    public EquipmentSlot Slot => slot;

    public void Equip(GameObject user)
    {
        // 장비 효과
    }

    public void Unequip(GameObject user)
    {
        // 장비 효과 해제
    }
}
