using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBaseData : ScriptableObject, IItemData
{
    [Header("공통 속성")]
    [SerializeField] public string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ItemType type;
    [SerializeField] private bool isStackable = true;
    [SerializeField] private int maxStack = 99;

    public string ItemName => itemName;

    public Sprite Icon => icon;

    public GameObject ItemPrefab => prefab;

    public ItemType Type => type;

    public bool IsStackable => isStackable;

    public int MaxStack => maxStack;
}
