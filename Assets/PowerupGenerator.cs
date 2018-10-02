using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupGenerator : MonoBehaviour {
    public Transform black_hole;
    public Transform solarflare_powerup_prefab, timeslow_powerup_prefab, bounce_powerup_prefab;
    public int max_simultaneous_powerups;
    public float spawn_prob_per_sec_solarflare, spawn_prob_per_sec_timeslow, spawn_prob_per_sec_bounce;

    private GameObject[] platforms;
    private int nextUpdate = 1;
    private Dictionary<GameObject,GameObject> platform_powerup_map;
    // Use this for initialization
    void Start () {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        platform_powerup_map = new Dictionary<GameObject, GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            update_powerups();
        }
    }

    void update_powerups()
    {
        //renew platform list
        GameObject[] new_platforms = GameObject.FindGameObjectsWithTag("Platform");

        //update platform_powerup map
        List<GameObject> invalidated_platforms = platforms.Except(new_platforms).ToList();
        foreach(GameObject invalid_platform in invalidated_platforms)
        {
            platform_powerup_map.Remove(invalid_platform);
        }
        
        //renew platform list part 2
        platforms = new_platforms;


        if (Random.value < spawn_prob_per_sec_solarflare)
        {
            spawn_powerup(solarflare_powerup_prefab);
        }
        if (Random.value < spawn_prob_per_sec_timeslow)
        {
            spawn_powerup(timeslow_powerup_prefab);
        }
        if (Random.value < spawn_prob_per_sec_bounce)
        {
            spawn_powerup(bounce_powerup_prefab);
        }
    }

    void spawn_powerup(Transform powerup_prefab)
    {
        GameObject spawn_platform;
        Vector3 spawn_position;
        List<GameObject> available_platforms = platforms.Except(platform_powerup_map.Keys).ToList();

        //check if available platform exists
        if(available_platforms.Count == 0)
        {
            return;
        }

        spawn_platform = get_spawn_platform(available_platforms);
        spawn_position = get_spawn_position(spawn_platform);

        Transform powerup = Instantiate(powerup_prefab, spawn_position, get_spawn_rotation(spawn_position));
        platform_powerup_map.Add(spawn_platform, powerup.gameObject);
    }

    GameObject get_spawn_platform(List<GameObject> available_platforms)
    {
        int random_index = Random.Range(0, available_platforms.Count);
        Debug.Log(available_platforms.Count + " " + random_index);
        return available_platforms[random_index];
    }

    Vector3 get_spawn_position(GameObject platform)
    {
        return platform.transform.position + platform.transform.up * 1f;
    }

    Quaternion get_spawn_rotation(Vector3 spawn_position)
    {
        Vector3 direction_towards_center = (spawn_position - black_hole.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(-direction_towards_center);
        return rotation;
    }


    public void powerup_taken(GameObject powerup)
    {
        GameObject corresponding_platform = platform_powerup_map.FirstOrDefault(e => e.Value == powerup).Key;
        if (corresponding_platform != null)
        {
            platform_powerup_map.Remove(corresponding_platform);
        }
    }
}
