using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    private Rigidbody rb;

    private bool isFlying = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventBus.SubscribeVoid("ReturnToStart", ReturnToStart);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("ReturnToStart", ReturnToStart);
    }

    public void Throw(Vector3 direction, float power)
    {
        if (isFlying) return; // 중복 방지
        isFlying = true;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(direction * power, ForceMode.Impulse);
    }

    public void ReturnToStart()
    {
        isFlying = false;
        rb.isKinematic = true;
        transform.position = startPosition.position;
        EventBus.PublishVoid("FishingExit");
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (other.CompareTag("Water"))
        {
            // 낚시 이벤트 시작
            EventBus.PublishVoid("StartFishing");
        }
        else
        {
            ReturnToStart();
        }
    }
}
