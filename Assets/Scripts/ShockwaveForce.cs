using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveForce : MonoBehaviour
{

    public float radius = 10f;
    public float force = 5f;
    public LayerMask shockwave_layer_mask;

    private float time_since_explosion_start;

    private void Start()
    {
        time_since_explosion_start = 0;
    }

    private void Update()
    {
        List<Rigidbody> affected_rbs = new List<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, shockwave_layer_mask);
        foreach (Collider col in colliders)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                affected_rbs.Add(rigidbody);
            }
        }

        time_since_explosion_start += Time.deltaTime;

        float explosion_speed = 9.0f;   //this is evaluated experimentally - @future self: dont judge me
        for(int i=affected_rbs.Count-1; i>=0; i--)
        {
            Rigidbody rb = affected_rbs[i];
            if (rb == null) //affected rigidbody might have been destroyed
            {
                affected_rbs.RemoveAt(i);
            }
            else
            {
                float explosion_distance_travelled = time_since_explosion_start * explosion_speed;
                float current_distance_to_explosion_center = (rb.transform.position - transform.position).magnitude;

                if (current_distance_to_explosion_center < explosion_distance_travelled)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                    rb.GetComponent<Collider>().enabled = false;
                    Destroy(rb.gameObject, 2f);
                    affected_rbs.RemoveAt(i);
                }
            }
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collision! "+other.tag+" "+other.layer);
    }

    /*public void ShockWave()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, shockwave_layer_mask);

        foreach (Collider col in colliders)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                affected_rbs.Add(rigidbody);
                //float distance = (col.transform.position - transform.position).magnitude;
                //StartCoroutine(DelayedForce(rigidbody, distance*0.1f));
            }
        }
    }

    IEnumerator DelayedForce(Rigidbody rb, float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        //the rigidbody or its gameObject might have been destroyed in the wait_time
        if (rb != null && rb.gameObject != null)
        {
            rb.AddExplosionForce(force, transform.position, radius);
            rb.GetComponent<Collider>().enabled = false;
            Destroy(rb.gameObject, 2f);
        }
    }*/
}
