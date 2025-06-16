using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform rodTip;
    [SerializeField] private Transform bobber;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.useWorldSpace = true;
    }

    private void Update()
    {
        if (rodTip == null || bobber == null)
            return;

        // 항상 낚싯대와 찌 사이를 선으로 그림
        line.SetPosition(0, rodTip.position);
        line.SetPosition(1, bobber.position);
    }
}
