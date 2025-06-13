using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftCategory
{
    Building,
    Food,
    Tools,
    Resource,
    Weapon,
    Equipment
}

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public CraftCategory category;
    public ItemBaseData resultItem;
    public int resultQuantity = 1;

    public List<RequiredItem> requiredItems = new();
}
