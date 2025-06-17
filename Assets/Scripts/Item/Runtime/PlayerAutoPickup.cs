using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask itemLayer;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, itemLayer);

        foreach (Collider hit in hits)
        {
            var item = hit.GetComponent<ItemPickup>();
            if (item != null)
            {
                item.PickUp(gameObject);
            }
        }
    }
}
