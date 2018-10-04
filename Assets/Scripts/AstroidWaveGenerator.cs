using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstroidWaveGenerator : MonoBehaviour {
    public GameObject center;
    public Transform astroid_prefab;
    public Text wave_information_field;
    public float spawn_distance_to_center;
    public float time_between_waves;

    private bool wave_in_progress, currently_generating_wave, currently_inbetween_waves;
    private List<GameObject> objects_in_current_wave;
    private int wave_nr;
    // Use this for initialization
    void Start () {
        wave_in_progress = false;
        currently_generating_wave = false;
        currently_inbetween_waves = false;
        objects_in_current_wave = new List<GameObject>();
        wave_nr = 0;
    }
	
	// Update is called once per frame
	void Update () {
        wave_in_progress = !objects_in_current_wave.TrueForAll(x => x == null);
        if (!wave_in_progress && !currently_generating_wave && !currently_inbetween_waves) {
            wave_nr += 1;
            objects_in_current_wave.Clear();    //should be all null by now
            int nr_astroids = wave_nr * 2;
            StartCoroutine(wave(nr_astroids, 60f, 1.0f, 5.0f, 3, 0.5f, 0.5f));
        }
	}

    IEnumerator wave(int nr_astroids, float area_degrees,
        float min_delay_between_astroids, float max_delay_between_astroids,
        int blink_nr, float blink_show_time, float blink_inbetween_time)
    {
        //int count_down_from = 3;

        float total_blink_time = blink_nr * (blink_show_time + blink_inbetween_time);

        //inbetween waves delay
        currently_inbetween_waves = true;
        //yield return new WaitForSeconds(Math.Max(time_between_waves-(total_blink_time+count_down_from),0));
        yield return new WaitForSeconds(Math.Max(time_between_waves-total_blink_time,0));

        for (int i = 0; i < blink_nr; i++)
        {
            wave_information_field.color = Color.red;
            wave_information_field.text = "Wave " + wave_nr;
            yield return new WaitForSeconds(blink_show_time);
            wave_information_field.text = "";
            yield return new WaitForSeconds(blink_inbetween_time);
        }

        /*for(int i = count_down_from; i >= 1; i--)
        {
            wave_information_field.text = i.ToString();
            yield return new WaitForSeconds(0.7f);
            wave_information_field.text = "";
            yield return new WaitForSeconds(0.3f);
        }*/

        currently_inbetween_waves = false;

        StartCoroutine(generate_wave(nr_astroids, area_degrees, min_delay_between_astroids, max_delay_between_astroids));
    }

    Vector3[] select_spawn_points(int n, float degrees)
    {
        Vector3 random_on_circle = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 spawn_arc_base_point = new Vector3(random_on_circle.x, 0, random_on_circle.y);
        Debug.Log("Base Point: " + spawn_arc_base_point);
        Vector3[] spawn_point_arr = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            //create spawn point in random rotation offset from base point
            Vector3 relative_spawn_point = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, degrees), Vector3.up) * spawn_arc_base_point;
            Vector3 spawn_point = center.transform.position + relative_spawn_point * spawn_distance_to_center;
            Debug.Log(spawn_point);
            spawn_point_arr[i] = spawn_point;
        }
        return spawn_point_arr;
    }

    IEnumerator generate_wave(int nr_astroids, float area_degrees,
        float min_delay_between_astroids, float max_delay_between_astroids)
    {
        currently_generating_wave = true;
        //TODO: different waves depending on wave_nr
        Vector3[] spawn_points = select_spawn_points(nr_astroids, area_degrees);
        foreach(Vector3 spawn_point in spawn_points)
        {
            float spawn_delay = UnityEngine.Random.Range(min_delay_between_astroids, max_delay_between_astroids);
            yield return new WaitForSeconds(spawn_delay);
            Transform astroid = Instantiate(astroid_prefab, spawn_point, UnityEngine.Random.rotation);
            objects_in_current_wave.Add(astroid.gameObject);
        }
        currently_generating_wave = false;
    }
}
