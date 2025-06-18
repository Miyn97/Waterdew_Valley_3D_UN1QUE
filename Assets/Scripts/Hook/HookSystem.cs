using System.Collections.Generic;

using UnityEngine;

public class HookSystem : MonoBehaviour
{
    [SerializeField] private LayerMask itemLayer; // 붙을 수 있는 오브젝트의 레이어

    private Hook hook;
    private bool isRunning = false;

    private List<Transform> attachedItems = new List<Transform>();

    private void Awake()
    {
        hook = GetComponent<Hook>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (isRunning && Input.GetMouseButtonDown(0))
        {
            hook.ReturnToHand();
        }
    }

    public void HookSystemOn()
    {
        isRunning = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isRunning) return;

        if (((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            if (!attachedItems.Contains(other.transform))
            {
                attachedItems.Add(other.transform);
                other.transform.SetParent(transform);

                Rigidbody rb = other.attachedRigidbody;
                if (rb != null)
                    rb.isKinematic = true;
            }
        }
    }

    public void HookSystemOff()
    {
        if (!isRunning) return;
        isRunning = false;

        foreach (var item in attachedItems)
        {
            if (item == null) continue;

            item.SetParent(null);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
        }

        attachedItems.Clear();
    }
}
