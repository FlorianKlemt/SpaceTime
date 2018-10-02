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
    private int update_each = 1;
    private float time_to_next_update;
    private Dictionary<GameObject,GameObject> platform_powerup_map;
    private List<GameObject> blocked_platforms;
    
    // Use this for initialization
    void Start () {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        platform_powerup_map = new Dictionary<GameObject, GameObject>();
        blocked_platforms = new List<GameObject>();
        time_to_next_update = update_each;
    }

    //Note: we make a late update here so that platform removals are already registered for this update
    void LateUpdate() {
        if (time_to_next_update <= 0)
        {
            update_powerups();
            time_to_next_update = update_each;
        }
        else
        {
            time_to_next_update -= Time.deltaTime;
        }

        //NOTE: this part is necessary even though platform removement should be already handled
        //Unity is weird and stuff is done at seemingly random times to just update stuff here
        //renew platform list
        GameObject[] new_platforms = GameObject.FindGameObjectsWithTag("Platform");
        Debug.Log("New Platforms is null: " + (new_platforms == null));

        //update platform_powerup map
        List<GameObject> invalidated_platforms = platforms.Except(new_platforms).ToList();
        foreach (GameObject invalid_platform in invalidated_platforms)
        {
            if (blocked_platforms.Contains(invalid_platform))
            {
                GameObject powerup_to_destroy = platform_powerup_map[invalid_platform];
                blocked_platforms.Remove(invalid_platform);
                platform_powerup_map.Remove(invalid_platform);
                if (powerup_to_destroy != null)
                {
                    Destroy(powerup_to_destroy);
                }
                Debug.Log("Initial Remove: " + platform_powerup_map + " Size: " + platform_powerup_map.Count);
            }
        }

        //renew platform list part 2
        platforms = new_platforms;
    }

    void update_powerups()
    { 
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
        List<GameObject> available_platforms = platforms.Except(blocked_platforms).ToList();
        
        //if less then max allowed powerups and available platforms exist
        if (blocked_platforms.Count < max_simultaneous_powerups && available_platforms.Count > 0)
        {
            GameObject spawn_platform = get_spawn_platform(available_platforms);
            Vector3 spawn_position = get_spawn_position(spawn_platform);

            Transform powerup = Instantiate(powerup_prefab, spawn_position, get_spawn_rotation(spawn_position));
            if (powerup != null)
            {
                platform_powerup_map.Add(spawn_platform, powerup.gameObject);
                blocked_platforms.Add(spawn_platform);
            }
        }
    }

    GameObject get_spawn_platform(List<GameObject> available_platforms)
    {
        int random_index = Random.Range(0, available_platforms.Count);
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
        GameObject corresponding_platform = platform_powerup_map.First(e => e.Value == powerup).Key;
        if (corresponding_platform != null)
        {
            platform_powerup_map.Remove(corresponding_platform);
            blocked_platforms.Remove(corresponding_platform);
        }
    }
}
