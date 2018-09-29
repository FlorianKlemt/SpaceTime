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
	// Use this for initialization
	void Start () {
        lerp_to_center = false;
        lerp_target_size = Vector3.zero;
        black_hole_size_controller = GameObject.Find("GameController").GetComponent<BlackHoleSizeController>();
    }
	
	// Update is called once per frame
	void Update () {
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
}
