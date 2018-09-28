using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
    float game_time = 15;
    float warning_time = 10;

    float start_time;
    Text text_obj;

	// Use this for initialization
	void Start () {
        start_time = Time.time;
        text_obj = GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        float current_time = Time.time - start_time;

        float time_left = game_time - current_time;

        if (time_left <= warning_time + 1)
        {
            text_obj.color = Color.red;
        }
        else
        {
            text_obj.color = Color.black;
        }
        if (time_left<=0)
        {
            //TODO: lose?
            time_left = 0;
        }

        int minutes = (int)(time_left / 60f);
        int seconds = (int)(time_left % 60f);
        text_obj.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
