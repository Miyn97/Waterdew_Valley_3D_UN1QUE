using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    EquipmentSlot Slot { get; }
    void Equip(GameObject user);
    void Unequip(GameObject user);
}
