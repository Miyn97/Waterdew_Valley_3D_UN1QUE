using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ConsumableType
{
    Water,
    Food
}

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItemData : ItemBaseData
{
    public ConsumableType consumableType;
    public int recoverAmount;
}
