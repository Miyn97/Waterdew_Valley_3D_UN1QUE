using System.Collections;

using UnityEngine;

public class WaterPurifier : MonoBehaviour, IInteractable
{
    [SerializeField] private float purifyTime = 3f;
    private bool isPurifying = false;

    [Header("필요 재료")]
    [SerializeField] private ItemBaseData plankData;
    [SerializeField] private ConsumableItemData saltWaterData;

    [Header("결과물")]
    [SerializeField] private ConsumableItemData cleanWaterData;

    public void Interact()
    {
        TryPurifyWater();
    }

    public string GetInteractPrompt()
    {
        return isPurifying ? "정수 중..." : "E 키를 눌러 정수 시작";
    }

    public void TryPurifyWater()
    {
        if (isPurifying) return;

        Inventory inventory = Inventory.Instance;

        if (!inventory.HasItem(plankData, 1))
        {
            Debug.Log("연료(Plank)가 부족합니다.");
            return;
        }

        if (!inventory.HasItem(saltWaterData, 1))
        {
            Debug.Log("바닷물이 없습니다.");
            return;
        }

        // 아이템 소모
        inventory.RemoveItem(plankData, 1);
        inventory.RemoveItem(saltWaterData, 1);

        StartCoroutine(PurifyRoutine());
    }

    private IEnumerator PurifyRoutine()
    {
        isPurifying = true;
        Debug.Log("정수 중...");

        yield return new WaitForSeconds(purifyTime);

        Inventory.Instance.AddItem(cleanWaterData, 1);
        Debug.Log("정수 완료! 정제수를 획득했습니다.");

        isPurifying = false;
    }
}
