using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonUI : MonoBehaviour
{
    public CraftCategory category;
    public Sprite categoryIcon;

    public void Initialize(CraftCategory category, Sprite icon)
    {
        this.category = category;
        categoryIcon = icon;
        GetComponent<Image>().sprite = icon;
    }
}
