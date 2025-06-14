using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ToolType
{
    Hook,
    FishingRod,
    Axe,
    Hammer,
    Paddle,
    Spear,
    Bow
}

[CreateAssetMenu(menuName = "Item/Useable")]
public class UseItemData : ItemBaseData, IUsable
{
    public ToolType toolType;
    public int durability = 100;
    public GameObject modelPrefab;

    public void Use(GameObject user)
    {

        switch (toolType)
        {
            case ToolType.Hook:
                break;

            case ToolType.FishingRod:
                break;

            case ToolType.Axe:
                break;

            case ToolType.Hammer:
                break;

            case ToolType.Paddle:
                break;
            case ToolType
            .Spear:
                break;

            case ToolType.Bow:
                break;
        }
    }
}
