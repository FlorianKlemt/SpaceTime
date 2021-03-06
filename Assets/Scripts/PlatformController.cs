﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    public int platform_lives;
    public float reflected_astroid_death_time, astroid_reflection_speed_multiplier;

    private float bounce_powerup_duration;
    private bool bounce_active, is_clippable;
    private PowerupGenerator powerup_generator;
    // Use this for initialization
    void Start () {
        bounce_powerup_duration = 0;
        bounce_active = false;

        //player = GameObject.FindGameObjectWithTag("Player");
        powerup_generator = GameObject.FindGameObjectWithTag("GameController")
                                      .GetComponent<PowerupGenerator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (bounce_powerup_duration > 0)
        {
            bounce_powerup_duration -= Time.deltaTime;
            if (!bounce_active)
            {
                bounce_active = true;
            }
        }
        else if(bounce_active)
        {
            bounce_active = false;
            bounce_powerup_duration = 0;
        }

        update_platform_state();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Astroid")
        {
            if (!bounce_active)
            {
                Destroy(collision.gameObject);
                platform_lives -= 1;
            }
            else
            {
                collision.rigidbody.velocity = -collision.rigidbody.velocity*astroid_reflection_speed_multiplier;
                Destroy(collision.gameObject, reflected_astroid_death_time);
            }
        }
    }

    public void update_platform_state()
    {
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        if (is_clippable)
        {
            mesh_renderer.material.color = Color.green;
        }
        else if (bounce_active)
        {
            mesh_renderer.material.color = Color.magenta;
        }
        else
        {
            switch (platform_lives)
            {
                case 2:
                    mesh_renderer.material.color = Color.white;
                    break;
                case 1:
                    mesh_renderer.material.color = Color.grey;
                    break;
                case 0:
                    Destroy(gameObject);
                    break;
            }
        }
    }

    public void set_bounce_powerup(float bounce_powerup_duration)
    {
        this.bounce_powerup_duration = bounce_powerup_duration;
    }

    public void set_clippable(bool is_clippable)
    {
        this.is_clippable = is_clippable;
    }
}
