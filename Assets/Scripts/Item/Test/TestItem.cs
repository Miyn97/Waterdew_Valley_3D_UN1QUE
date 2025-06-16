using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "JM/Item")]
public class TestItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string des;
    public int itemID;
}
