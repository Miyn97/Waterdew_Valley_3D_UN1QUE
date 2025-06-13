using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform categoryBar;                      // 카테고리 버튼 부모
    public Transform recipeScrollContent;              // 레시피 목록 Content
    public Transform requireItemScrollContent;         // 재료 목록 Content

    public GameObject categoryButtonPrefab;
    public GameObject recipeButtonPrefab;
    public GameObject requireItemUIPrefab;

    public Image recipeIcon;
    public TextMeshProUGUI recipeText;
    public TextMeshProUGUI descriptionText;
    public Image categoryIconImage;                    // 선택된 카테고리 아이콘

    [Header("Data")]
    public List<CraftingRecipe> allRecipes;
    public List<Sprite> categoryIcons; // enum 순서대로 등록 (Building, Food, Tools, Weapon, Equipment)

    private void Start()
    {
        LoadCategoryButtons();
    }

    void LoadCategoryButtons()
    {
        for (int i = 0; i < categoryIcons.Count; i++)
        {
            CraftCategory category = (CraftCategory)i;
            Sprite icon = categoryIcons[i];

            GameObject btn = Instantiate(categoryButtonPrefab, categoryBar);
            var categoryUI = btn.GetComponent<CategoryButtonUI>();
            categoryUI.Initialize(category, icon);

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnCategorySelected(categoryUI.category, categoryUI.categoryIcon);
            });
        }
    }

    public void OnCategorySelected(CraftCategory category, Sprite categoryIcon)
    {
        categoryIconImage.sprite = categoryIcon;

        // 기존 레시피 버튼 초기화
        foreach (Transform child in recipeScrollContent)
            Destroy(child.gameObject);

        // 선택된 카테고리의 레시피 필터링
        List<CraftingRecipe> filteredRecipes = new List<CraftingRecipe>();
        foreach (var recipe in allRecipes)
        {
            if (recipe.category == category)
            {
                filteredRecipes.Add(recipe);
            }
        }

        // 버튼 생성
        foreach (var recipe in filteredRecipes)
        {
            GameObject go = Instantiate(recipeButtonPrefab, recipeScrollContent);
            go.GetComponentInChildren<TextMeshProUGUI>().text = recipe.resultItem.ItemName;

            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowRecipeDetail(recipe);
            });
        }
    }

    public void ShowRecipeDetail(CraftingRecipe recipe)
    {
        recipeIcon.sprite = recipe.resultItem.Icon;
        recipeText.text = recipe.resultItem.ItemName;
        descriptionText.text = recipe.resultItem.description;

        // 기존 재료 UI 제거
        foreach (Transform child in requireItemScrollContent)
            Destroy(child.gameObject);

        // 필요 재료 UI 생성
        foreach (var req in recipe.requiredItems)
        {
            GameObject go = Instantiate(requireItemUIPrefab, requireItemScrollContent);
            go.transform.Find("Icon").GetComponent<Image>().sprite = req.item.Icon;
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = req.item.ItemName;
            go.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = $"x{req.quantity}";
        }
    }
}
