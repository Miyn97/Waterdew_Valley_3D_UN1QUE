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
    public string objName;           // 건물 오브젝트 이름
    public string description;          // 건물 오브젝트 설명
    public Sprite icon;                // UI 아이콘
    //public float hitPoint;
    //public List<BuildCost> costs;
}
