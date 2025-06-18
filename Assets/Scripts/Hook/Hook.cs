using System.Collections;

using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private LayerMask itemLayer; // 붙을 수 있는 오브젝트의 레이어
    public Transform startPosition;
    private Rigidbody rb;
    private HookSystem hookSystem;

    private bool isFlying = false;
    private bool isReturning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hookSystem = GetComponent<HookSystem>();
    }

    public void Throw(Vector3 direction, float power)
    {
        if (isFlying || isReturning) return; // 중복 방지
        isFlying = true;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(direction * power, ForceMode.Impulse);
    }

    public void ReturnToHand()
    {
        if (isReturning) return; // 중복 방지
        isReturning = true;
        StartCoroutine(MoveBackToHand());
    }

    private IEnumerator MoveBackToHand()
    {
        float speed = 10f;
        float minDistance = 0.05f;

        while (Vector3.Distance(transform.position, startPosition.position) > minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = startPosition.position;
        isReturning = false;

        EventBus.PublishVoid("ThrowingExit");
        hookSystem.HookSystemOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFlying) return;

        isFlying = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (other.CompareTag("Water") || ((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            hookSystem.HookSystemOn();
        }
        else
        {
            ReturnToHand();
        }
    }
}
