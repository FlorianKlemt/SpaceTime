using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveForce : MonoBehaviour
{

    public float radius = 10f;
    public float force = 5f;
    public LayerMask shockwave_layer_mask;

    // Use this for initialization
    void Start()
    {

    }

    public void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collision! "+other.tag+" "+other.layer);
    }

    public void ShockWave()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, shockwave_layer_mask);

        foreach (Collider col in colliders)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                float distance = (col.transform.position - transform.position).magnitude;
                StartCoroutine(DelayedForce(rigidbody, distance*0.1f));
            }
        }
    }

    IEnumerator DelayedForce(Rigidbody rb, float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        rb.AddExplosionForce(force, transform.position, radius);
    }
}
