using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ConsumableType
{
    Water,
    Food,
    Both
}

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItemData : ItemBaseData
{
    public ConsumableType consumableType;
    public int thirstRecoveryAmount;
    public int hungerRecoveryAmount;

}
