using System.Collections;

using UnityEngine;

public class FishingSystem : MonoBehaviour
{
    public void StartFishBite(Bobber bobber)
    {
        StartCoroutine(OnFishBite(bobber));
    }

    public IEnumerator OnFishBite(Bobber bobber)
    {
        float qteTime = Random.Range(0.25f, 0.75f);
        float timer = 0f;
        bool success = false;

        while (timer < qteTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                success = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        if (success)
        {
            FishType caught = GetRandomFish();
            Debug.Log($"{caught} 잡음");
            // TODO: UI 표시, 인벤토리 추가 등
        }
        else
        {
            Debug.Log("낚시 실패");
            // TODO: 실패 UI
        }

        // 찌를 다시 원위치
    }

    FishType GetRandomFish()
    {
        float rand = Random.value;
        if (rand <= 0.4f) return FishType.Mackerel;     // 40%
        else if (rand <= 0.75f) return FishType.Herring; // 35%
        else return FishType.Salmon;                     // 25%
    }
}
