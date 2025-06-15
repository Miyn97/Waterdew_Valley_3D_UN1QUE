using System.Collections;

using UnityEngine;

public class FishingSystem : Singleton<FishingSystem>
{

    FishType GetRandomFish()
    {
        float rand = Random.value;
        if (rand <= 0.4f) return FishType.Mackerel;     // 40%
        else if (rand <= 0.75f) return FishType.Herring; // 35%
        else return FishType.Salmon;                     // 25%
    }
}
