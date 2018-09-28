using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//regular gravity is supposed to pull everything EXCEPT the player
//the player has its own non-realistic gravity implementation due to gameplay reasons 

public class Gravity : MonoBehaviour {
    public float pullRadius = 100;
    public float pullForce = 1;
    public LayerMask layersToPull;

    // Use this for initialization
    void Start()
    {

    }

    public void FixedUpdate()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius, layersToPull)) {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb == null)
                continue; // Can only pull objects with Rigidbody

            // calculate direction from target to this object
            Vector3 forceDirection = transform.position - collider.transform.position;

            // apply force on target towards this object
            rb.AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
        }
    }
}
