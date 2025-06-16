using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float damage = 25f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
            //if (hp != null)
            //{
            //    hp.TakeDamage(damage);
            //}
        }

        Destroy(gameObject, 1f); // 충돌 후 제거
    }
}
