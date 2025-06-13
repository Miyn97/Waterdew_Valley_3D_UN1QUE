using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipment,
    Consumable,
    Useable
}

public enum EquipmentSlot
{
    Weapon,
    Tool,
    Armor
}

public interface IItemData
{
    string ItemName { get; }
    Sprite Icon { get; }
    GameObject ItemPrefab { get; }
    ItemType Type { get; }
    bool IsStackable { get; }
    int MaxStack {  get; }
}
