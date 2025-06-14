using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform categoryBar;                    // 카테고리 버튼 부모
    public Transform recipeScrollContent;            // 레시피 목록 Content
    public Transform requireItemScrollContent;       // 재료 목록 Content

    public GameObject categoryButtonPrefab;
    public GameObject recipeButtonPrefab;
    public GameObject requireItemUIPrefab;

    public Image recipeIcon;
    public TextMeshProUGUI recipeText;
    public TextMeshProUGUI descriptionText;
    public Image categoryIconImage;                  // 선택된 카테고리 아이콘
    public TextMeshProUGUI categoryText;             // 선택된 카테고리 텍스트

    [Header("Dynamic Panels")]
    public GameObject recipeListPanel;               // 좌측 레시피 패널 전체
    public GameObject descriptionPanel;              // 상세 설명 패널 전체

    [Header("Data")]
    public List<CraftingRecipe> allRecipes;
    public List<Sprite> categoryIcons;               // enum 순서대로 등록

    public Button craftButton;
    public CraftingSystem craftingSystem;
    private CraftingRecipe currentSelectedRecipe;

    private void Start()
    {
        recipeListPanel.SetActive(false);
        descriptionPanel.SetActive(false);
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
        categoryText.text = category.ToString();

        recipeListPanel.SetActive(true);      // 레시피 목록 활성화
        descriptionPanel.SetActive(false);    // 설명 패널 초기에는 숨김

        // 기존 레시피 버튼 제거
        foreach (Transform child in recipeScrollContent)
            Destroy(child.gameObject);

        // 해당 카테고리의 레시피만 필터링
        List<CraftingRecipe> filteredRecipes = new List<CraftingRecipe>();
        foreach (var recipe in allRecipes)
        {
            if (recipe.category == category)
                filteredRecipes.Add(recipe);
        }

        // 레시피 버튼 생성
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
        descriptionPanel.SetActive(true);
        currentSelectedRecipe = recipe;

        recipeIcon.sprite = recipe.resultItem.Icon;
        recipeText.text = recipe.resultItem.ItemName;
        descriptionText.text = recipe.resultItem.description;

        foreach (Transform child in requireItemScrollContent)
            Destroy(child.gameObject);

        foreach (var req in recipe.requiredItems)
        {
            if (req.item == null) continue;

            GameObject go = Instantiate(requireItemUIPrefab, requireItemScrollContent);
            go.transform.Find("Icon").GetComponent<Image>().sprite = req.item.Icon;
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = req.item.ItemName;
            go.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = $"x{req.quantity}";
        }

        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(OnClickCraft);
    }

    public void OnClickCraft()
    {
        if (currentSelectedRecipe == null) return;

        if (craftingSystem.CanCraft(currentSelectedRecipe))
        {
            craftingSystem.Craft(currentSelectedRecipe);
            ShowRecipeDetail(currentSelectedRecipe); // UI 갱신
        }
        else
        {
            Debug.LogWarning("재료가 부족합니다.");
        }
    }
}
