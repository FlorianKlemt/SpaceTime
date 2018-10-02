using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstroidController : MonoBehaviour {
    public float lerp_to_center_speed;
    
    private bool lerp_to_center;
    private Vector3 lerp_target;
    private Vector3 lerp_target_size;
    private BlackHoleSizeController black_hole_size_controller;
    private Rigidbody rb;

    private bool timeslow_active;
    private float time_slow_duration_left, slow_factor;
    private Vector3 last_update_velocity_during_slow;
	// Use this for initialization
	void Start () {
        lerp_to_center = false;
        lerp_target_size = Vector3.zero;
        black_hole_size_controller = GameObject.Find("GameController").GetComponent<BlackHoleSizeController>();
        rb = GetComponent<Rigidbody>();
        timeslow_active = false;
        time_slow_duration_left = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (timeslow_active)
        {
            if (time_slow_duration_left > 0)
            {
                Vector3 velocity_difference = rb.velocity - last_update_velocity_during_slow;
                rb.velocity = last_update_velocity_during_slow + velocity_difference * slow_factor;
                time_slow_duration_left -= Time.deltaTime;
            }
            else
            {
                rb.velocity *= 1/slow_factor;
                timeslow_active = false;
            }
        }

        if (lerp_to_center)
        {
            if ((transform.position - lerp_target).magnitude < 0.01)
            {
                Destroy(gameObject);
            }
            else
            {
                float lerp_progress = Time.deltaTime * lerp_to_center_speed;
                transform.position = Vector3.Lerp(transform.position, lerp_target, lerp_progress);
                transform.localScale = Vector3.Lerp(transform.localScale, lerp_target_size,
                                                    lerp_progress);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BlackHole")
        {
            black_hole_size_controller.addSize(1.0f);

            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Collider>().enabled = false;
            lerp_to_center = true;
            lerp_target = other.transform.position;
        }
    }

    public void set_time_slow_powerup(float powerup_time, float slow_factor) {
        //if time-slow powerup is already running only prolong effect (do not slow multiplicative)
        if (time_slow_duration_left <= 0)
        {
            rb.velocity *= slow_factor;
        }

        timeslow_active = true;
        last_update_velocity_during_slow = rb.velocity;

        this.time_slow_duration_left = powerup_time;
        this.slow_factor = slow_factor;
    }
}
