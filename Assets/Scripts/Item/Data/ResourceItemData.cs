using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Resource")]
public class ResourceItemData : ItemBaseData
{
    [Header("Resource 속성")]
    public string resourceType;

    // 확장 가능
}
