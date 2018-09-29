using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
    public Text time_field;
    public Transform event_horizon;
    public Transform player;
    public float inside_clock_time_speed, outside_clock_time_speed;
    public float inside_time_scale, outside_time_scale;
    public float time_slow_powerup_scale;

    float game_time = 15.0f;
    float warning_time = 10.0f;
    float current_time;

    float start_time;
    float clock_time_speed;
    float time_slow_powerup_duration;

    PlayerController player_controller;

    EventHorizonChecker event_horizon_checker;
    // Use this for initialization
    void Start () {
        event_horizon_checker = event_horizon.GetComponent<EventHorizonChecker>();
        player_controller = player.GetComponent<PlayerController>();

        current_time = 0;
        clock_time_speed = 1;
        time_slow_powerup_duration = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (time_slow_powerup_duration > 0)
        {
            Time.timeScale = time_slow_powerup_scale;
            float delta_time_multiplier = 1 / time_slow_powerup_scale;
            player_controller.set_delta_time_multiplier(delta_time_multiplier);
            time_slow_powerup_duration -= Time.deltaTime * delta_time_multiplier;
        }
        else if (event_horizon_checker.is_inside_horizon())
        {
            Time.timeScale = inside_time_scale;
            clock_time_speed = inside_clock_time_speed;
            player_controller.set_delta_time_multiplier(1);
        }
        else
        {
            Time.timeScale = outside_time_scale;
            clock_time_speed = outside_clock_time_speed;
            player_controller.set_delta_time_multiplier(1);
        }


        current_time += Time.deltaTime * clock_time_speed;

        float time_left = game_time - current_time;

        if (time_left <= warning_time + 1)
        {
            time_field.color = Color.red;
        }
        else
        {
            time_field.color = Color.black;
        }
        if (time_left<=0)
        {
            //TODO: lose?
            time_left = 0;
        }

        int minutes = (int)(time_left / 60f);
        int seconds = (int)(time_left % 60f);
        float ms = (time_left - (int)time_left)*100;
        time_field.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":"+ ms.ToString("00");
	}

    public void set_time_speed(float clock_time_speed)
    {
        this.clock_time_speed = clock_time_speed;
    }

    public void time_slow_powerup(float time_slow_powerup_duration)
    {
        this.time_slow_powerup_duration = time_slow_powerup_duration;
    }
}
