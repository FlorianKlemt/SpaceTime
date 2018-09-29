using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
    public Text time_field;

    float game_time = 15.0f;
    float warning_time = 10.0f;
    float current_time;

    float start_time;
    float time_speed;
	// Use this for initialization
	void Start () {
        current_time = 0;
        time_speed = 1;
    }
	
	// Update is called once per frame
	void Update () {
        current_time += Time.deltaTime * time_speed;

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

    public void set_time_speed(float time_speed)
    {
        this.time_speed = time_speed;
    }
}
