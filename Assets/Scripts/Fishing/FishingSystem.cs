using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FishingSystem : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    //public UIManager uiManager; // QTE UI 등
    [SerializeField] private List<FishData> fishData;
    [SerializeField] private ResourceItemData bait;

    private bool isFishing = false;
    private bool isQTEActive = false;
    private float qteTimer = 0f;
    private float qteDuration = 0f;    // 미끼 먹고 도망가는데 걸리는 시간.

    private Coroutine fishingCoroutine;

    private void OnEnable()
    {
        EventBus.SubscribeVoid("StartFishing", StartFishing);
        EventBus.SubscribeVoid("StopFishing", StopFishing);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("StartFishing", StartFishing);
        EventBus.UnsubscribeVoid("StopFishing", StopFishing);
    }

    private void Update()
    {
        Waiting();
    }

    private void Waiting()
    {
        if (isQTEActive)
        {
            qteTimer += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                if (qteTimer <= qteDuration)
                {
                    CatchFish();
                }
                else
                {
                    FailCatch();
                }
            }
            else if (qteTimer > qteDuration)
            {
                FailCatch();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            StopFishing();
        }
    }

    private void StartFishing()
    {
        if (!isFishing)
            fishingCoroutine = StartCoroutine(FishingCoroutine());
    }

    private void StopFishing()
    {
        Debug.Log("낚시 취소");
        if (fishingCoroutine != null)
            StopCoroutine(fishingCoroutine);

        isFishing = false;
        isQTEActive = false;
        EventBus.PublishVoid("ReturnToStart");
    }

    private IEnumerator FishingCoroutine()
    {
        isFishing = true;

        float waitTime = Random.Range(3f, 5f);    // 잡힐때 까지 걸리는 시간
        Debug.Log($"물고기 기다리는 중... {waitTime}초");
        yield return new WaitForSeconds(waitTime);

        //if (!inventory.UseItem(bait))
        if(true)
        {
            qteDuration = 2f;
        }
        else
        {
            qteDuration = Random.Range(0.25f, 0.75f);
        }

        isQTEActive = true;
        qteTimer = 0f;
        Debug.Log($"미끼 물음 => {qteDuration}초 안에 좌클릭을 눌러주세요");

        //uiManager.ShowQTE(qteDuration); // QTE UI 표시

        yield return new WaitForSeconds(qteDuration);

        if (isQTEActive) // 시간 내 클릭 못 했으면
        {
            FailCatch();
        }

        EventBus.PublishVoid("ReturnToStart");
    }

    private void CatchFish()
    {
        isQTEActive = false;
        isFishing = false;
        Debug.Log("낚시 성공!");

        FishData fish = GetRandomFish();
        //inventory.AddItem(fish);
        Debug.Log($"{fish.ItemName}를 잡음.");
        //uiManager.ShowCatchResult(caught);
    }

    private void FailCatch()
    {
        isQTEActive = false;
        isFishing = false;
        Debug.Log("낚시 실패");

        //uiManager.ShowFail();
    }

    FishData GetRandomFish()
    {
        float total = 0f;
        foreach (var fish in fishData)
            total += fish.probability;

        float rand = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var fish in fishData)
        {
            cumulative += fish.probability;
            if (rand <= cumulative)
                return fish;
        }

        return fishData[fishData.Count - 1]; // fallback
    }
}
