using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { Hook, Spear, FishingRod, Axe }

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
            case ToolType.Spear:
                // 공격 처리
                break;
            case ToolType.Hook:
                // 자원 끌기
                break;
            case ToolType.FishingRod:
                // 낚시 처리
                break;
        }
    }
}
