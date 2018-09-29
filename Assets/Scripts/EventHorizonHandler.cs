using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHorizonHandler : MonoBehaviour {
    public Transform event_horizon;
    public float inside_time_speed, outside_time_speed;

    EventHorizonChecker event_horizon_checker;
    GameTimer game_timer;
	// Use this for initialization
	void Start () {
        event_horizon_checker = event_horizon.GetComponent<EventHorizonChecker>();
        game_timer = GetComponent<GameTimer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (event_horizon_checker.is_inside_horizon())
        {
            game_timer.set_time_speed(inside_time_speed);
        }
        else
        {
            game_timer.set_time_speed(outside_time_speed);
        }
	}
}
