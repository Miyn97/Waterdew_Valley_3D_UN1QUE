using UnityEngine;

//[System.Serializable]
//public class BuildCost
//{
//    public ItemData item;
//    public int amount;
//}

[CreateAssetMenu(menuName = "Build/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string objName;           // 건물 오브젝트 이름
    public string description;       // 건물 오브젝트 설명
    public Sprite icon;              // UI 아이콘
    public GameObject prefab;        // 실체 프리팹
    public GameObject preview;       // 프리뷰 프리팹
    public bool isEdgeBuilding;
    //public float hitPoint;
    //public List<BuildCost> costs;
}
