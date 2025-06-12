using System.Collections.Generic;

using UnityEngine;

//[System.Serializable]
//public class BuildCost
//{
//    public ItemData item;
//    public int amount;
//}

[CreateAssetMenu(menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string itemName;
    //public float hitPoint;
    //public List<BuildCost> costs;
}
