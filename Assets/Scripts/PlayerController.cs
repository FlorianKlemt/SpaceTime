using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jump_velocity, clip_radius, max_left_speed,
        damping_on_grounded, trampoline_velocity;
    public GameObject center;
    public LayerMask layers_to_clip_to;
    public Transform shockwave_prefab;

    private Rigidbody player_rb;
    private bool is_grounded;
    private float distToGround;
    private GameObject clippable_obj;
    private bool shock = true;

    void Start()
    {
        player_rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.z;
        player_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        //handle platform clipping
        if (clippable_obj != null)
        {
            clippable_obj.GetComponent<PlatformController>().update_platform_state();
        }

        Collider[] colliders_in_clip_range = 
            Physics.OverlapSphere(transform.position, clip_radius, layers_to_clip_to);
        if (colliders_in_clip_range.Length == 0)
        {
            clippable_obj = null;
        }else
        {
            clippable_obj = colliders_in_clip_range[0].gameObject;
            clippable_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            if (colliders_in_clip_range.Length >= 2)
            {
                Debug.Log("Multiple clippable objects! Should not be the case.");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction_to_center = (center.transform.position - transform.position).normalized;
        //get current moving direction
        Vector3 current_left = Vector3.Cross(direction_to_center, Vector3.down).normalized;
        float current_left_speed = Vector3.Dot(current_left, player_rb.velocity);

        //handle left-right movement
        if (Input.GetKey(KeyCode.A))
        { 
            if (current_left_speed < max_left_speed)
            {
                player_rb.velocity += current_left * speed;
            }
            //transform.position += current_left * speed;
            //player_rb.AddForce(current_left * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if(current_left_speed > -max_left_speed)
            {
                player_rb.velocity += -current_left * speed;
            }
            //transform.position += -current_left * speed;
            //player_rb.AddForce(-current_left * speed);
        }

        float gravity = 10f;
        //handle jumping / player gravity / speed damping when grounded
        if (is_grounded)
        {
            player_rb.velocity *= damping_on_grounded;
            if (Input.GetKey(KeyCode.Space))
            {
                player_rb.velocity = -direction_to_center * jump_velocity;
            }
        }
        else
        {
            player_rb.velocity += direction_to_center * gravity * Time.fixedDeltaTime;
        }

        //handle platform clipping
        if (Input.GetKey(KeyCode.C) && clippable_obj!=null)
        {
            player_rb.velocity = Vector3.zero;
            transform.position = clippable_obj.transform.position + clippable_obj.transform.up*0.4f;
        }

        if (Input.GetKeyUp(KeyCode.S) && shock)
        {
            shock = false;
            Transform shockwave = Instantiate(shockwave_prefab, transform.position, Quaternion.identity);
            shockwave.transform.eulerAngles = new Vector3(-90, 0, 0);
            shockwave.GetComponent<ShockwaveForce>().ShockWave();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
            is_grounded = true;
        }
        if(collision.transform.tag == "Astroid")
        {
            Destroy(collision.gameObject);
            //TODO: make astroid fly off in direction of collision
            //collision.gameObject.GetComponent<Rigidbody>().
        }
        if(collision.transform.tag == "Trampoline")
        {
            Vector3 direction_to_center = (center.transform.position - transform.position).normalized;
            player_rb.velocity = -direction_to_center * trampoline_velocity;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
            is_grounded = false;
        }
    }
}
