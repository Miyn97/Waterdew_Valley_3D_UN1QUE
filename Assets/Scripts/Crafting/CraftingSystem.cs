﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public JmInvenSlotManager inventory;

    // 재료 확인
    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var req in recipe.requiredItems)
        {
            bool found = false;

            foreach (var slot in inventory.slotList)
            {
                if (slot.currentItem == req.item)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return false;
        }

        return true;
    }

    // 제작 실행
    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("재료가 부족합니다");
            return;
        }

        /*
        // 재료 차감
        foreach (var req in recipe.requiredItems)
        {
            inventory.RemoveItem(req.item, req.quantity);
        }
        */

        foreach(var req in recipe.requiredItems)
        {
            foreach(var slot in inventory.slotList)
            {
                if(slot.currentItem == req.item)
                {
                    slot.ClearSlot();
                }
            }
        }

        // 결과 아이템 추가
        inventory.AddItem(recipe.resultItem);
        Debug.Log($"제작 완료: {recipe.resultItem.ItemName}");
    }
}
