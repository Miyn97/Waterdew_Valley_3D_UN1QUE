using UnityEngine;
using System.Collections;

public class Bobber : MonoBehaviour
{
    private Transform raftTransform;
    private Vector3 offsetFromRaft;
    private bool hasBait = false;

    [SerializeField] private float minBiteTime = 10f;
    [SerializeField] private float maxBiteTime = 30f;
    [SerializeField] private float qteWindow = 0.5f;

    [SerializeField] private FishData[] fishTable;
    [SerializeField] private GameObject splashEffect;

    private bool isQTEActive = false;
    private bool fishCaught = false;

    public void Initialize(Vector3 initialPosition, Transform raft, bool bait)
    {
        transform.position = initialPosition;
        raftTransform = raft;
        offsetFromRaft = initialPosition - raft.position;
        hasBait = bait;

        if (hasBait)
        {
            StartCoroutine(FishBiteRoutine());
        }
    }

    void Update()
    {
        // 배 따라 위치 조정
        if (raftTransform != null)
        {
            transform.position = raftTransform.position + offsetFromRaft;
        }

        // QTE 처리
        if (isQTEActive && Input.GetMouseButtonDown(0))
        {
            isQTEActive = false;
            fishCaught = true;
            OnFishCaught();
        }
    }

    IEnumerator FishBiteRoutine()
    {
        float waitTime = Random.Range(minBiteTime, maxBiteTime);
        yield return new WaitForSeconds(waitTime);

        isQTEActive = true;
        float qteTime = Random.Range(0.25f, 0.75f);
        yield return new WaitForSeconds(qteTime);

        if (!fishCaught)
        {
            isQTEActive = false;
            Debug.Log("물고기 놓침!");
            Destroy(gameObject);
        }
    }

    void OnFishCaught()
    {
        // 확률 기반 물고기 결정
        FishData caught = GetRandomFish();
        Debug.Log($"물고기 잡힘: {caught.itemName}");

        // TODO: 인벤토리 추가, UI 표시 등
        Destroy(gameObject);
    }

    FishData GetRandomFish()
    {
        float total = 0f;
        foreach (var fish in fishTable) total += fish.probability;
        float rand = Random.Range(0, total);

        float cumulative = 0f;
        foreach (var fish in fishTable)
        {
            cumulative += fish.probability;
            if (rand <= cumulative)
                return fish;
        }

        return fishTable[0]; // fallback
    }
}
