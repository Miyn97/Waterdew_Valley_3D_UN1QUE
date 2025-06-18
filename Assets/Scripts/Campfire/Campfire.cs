using System.Collections;
using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable
{
    [SerializeField] private float cookTime = 15f;
    private bool isCooking = false;

    [Header("필요 재료")]
    [SerializeField] private ItemBaseData plankData;
    [SerializeField] private ConsumableItemData rawFishData;

    [Header("결과물")]
    [SerializeField] private ConsumableItemData cookedFishData;

    public void Interact()
    {
        TryCookFish();
    }

    public string GetInteractPrompt()
    {
        return isCooking ? "요리 중..." : "E 키를 눌러 생선 굽기";
    }

    public void TryCookFish()
    {
        if (isCooking) return;

        Inventory inventory = Inventory.Instance;

        if (!inventory.HasItem(plankData, 1))
        {
            Debug.Log("연료(Plank)가 부족합니다.");
            return;
        }

        if (!inventory.HasItem(rawFishData, 1))
        {
            Debug.Log("요리할 생선이 없습니다.");
            return;
        }

        // 재료 소모
        inventory.RemoveItem(plankData, 1);
        inventory.RemoveItem(rawFishData, 1);

        StartCoroutine(CookRoutine());
    }

    private IEnumerator CookRoutine()
    {
        isCooking = true;
        Debug.Log("생선 굽는 중...");

        yield return new WaitForSeconds(cookTime);

        Inventory.Instance.AddItem(cookedFishData, 1);
        Debug.Log("요리 완료! 구운 생선을 획득했습니다.");

        isCooking = false;
    }
}
