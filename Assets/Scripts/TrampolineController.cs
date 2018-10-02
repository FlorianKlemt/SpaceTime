using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour {

    public float reflected_astroid_death_time, astroid_reflection_speed_multiplier;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Astroid")
        {
            collision.rigidbody.velocity = -collision.rigidbody.velocity * astroid_reflection_speed_multiplier;
            Destroy(collision.gameObject, reflected_astroid_death_time);
        }
    }
}
