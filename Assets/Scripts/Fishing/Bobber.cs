using UnityEngine;

public class Bobber : MonoBehaviour
{
    public Rigidbody rb;
    public float returnSpeed = 5f;

    private Vector3 originalPosition;
    private bool isFlying = false;

    void Awake()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction, float power)
    {
        if (isFlying) return; // 중복 방지
        isFlying = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;

        rb.AddForce(direction * power, ForceMode.Impulse);
    }

    public void ReturnToStart()
    {
        isFlying = false;
        rb.isKinematic = true;
        transform.position = originalPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ocean"))
        {
            // 낚시 시작 로직
        }
        else
        {
            // 바다가 아니면 실패
            ReturnToStart();
        }
    }
}
