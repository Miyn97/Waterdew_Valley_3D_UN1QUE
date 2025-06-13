using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemBaseData resultItem;
    public int resultQuantity = 1;

    public List<RequiredItem> requiredItems = new();
}
