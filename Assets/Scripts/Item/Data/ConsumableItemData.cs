using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItemData : ItemBaseData, IUsable
{
    [Header("소모 효과")]
    public float fullAmount;

    public void Use(GameObject user)
    {
        //user.GetComponent<PlayerStats>().Eat(fullAmount);
    }
}
