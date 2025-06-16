using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreecherController : EnemyController
{
    [Header("Screecher Settings")]
    public GameObject rockPrefab;
    public Transform rockHoldPoint;

    private GameObject heldRock;

    public override void Attack(Transform target)
    {
        if (heldRock != null)
        {
            GameObject rock = heldRock;
            heldRock = null;
            rock.transform.parent = null;

            Rigidbody rb = rock.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 15f, ForceMode.VelocityChange);

            Debug.Log("돌 투하");
        }
    }

    public void PickUpRock()
    {
        if (rockPrefab != null && heldRock == null)
        {
            heldRock = Instantiate(rockPrefab, rockHoldPoint.position, Quaternion.identity, rockHoldPoint);
            Rigidbody rb = heldRock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Debug.Log("Screecher가 돌 획득");
        }
    }

    public bool HasRock()
    {
        return heldRock != null;
    }

    public void ResetToPatrol()
    {
        ChangeState(new ScreecherPatrolState(this));
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(new ScreecherPatrolState(this));
    }
}
