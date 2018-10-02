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

    private int nextUpdate = 1;
    // Use this for initialization
    void Start () {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        platform_powerup_map = new Dictionary<GameObject, GameObject>();
        time_to_next_update = update_each;
    }

    //Note: we make a late update here so that platform removals are already registered for this update
    void LateUpdate() {
        /*if (Time.time >= nextUpdate) {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            update_powerups();
        }*/

        if (time_to_next_update <= 0)
        {
            update_powerups();
            time_to_next_update = update_each;
        }
        else
        {
            time_to_next_update -= Time.deltaTime;
        }
    }

    void update_powerups()
    {
        //NOTE: this part is necessary even though platform removement should be already handled
        //Unity is weird and stuff is done at seemingly random times to just update stuff here
        //renew platform list
        GameObject[] new_platforms = GameObject.FindGameObjectsWithTag("Platform");
        Debug.Log("New Platforms is null: " + (new_platforms == null));

        //update platform_powerup map
        List<GameObject> invalidated_platforms = platforms.Except(new_platforms).ToList();
        foreach(GameObject invalid_platform in invalidated_platforms)
        {
            platform_powerup_map.Remove(invalid_platform);
            Debug.Log("Initial Remove: " + platform_powerup_map + " Size: " + platform_powerup_map.Count);
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
        List<GameObject> available_platforms = platform_powerup_map!=null&&platform_powerup_map.Count>0?
                                               platforms.Except(platform_powerup_map.Keys).ToList():
                                               platforms.ToList();

        Debug.Log("Platform Powerup Map is Null: " + (platform_powerup_map == null)
            + "  Available Platforms is Null: " + (available_platforms == null));
        Debug.Log("Counts: " + platform_powerup_map.Count + " " + available_platforms.Count);
        //if less then max allowed powerups and available platforms exist
        if (platform_powerup_map.Count < max_simultaneous_powerups && available_platforms.Count > 0)
        {
            GameObject spawn_platform = get_spawn_platform(available_platforms);
            Vector3 spawn_position = get_spawn_position(spawn_platform);

            Transform powerup = Instantiate(powerup_prefab, spawn_position, get_spawn_rotation(spawn_position));
            platform_powerup_map.Add(spawn_platform, powerup.gameObject);
        }
    }

    GameObject get_spawn_platform(List<GameObject> available_platforms)
    {
        int random_index = Random.Range(0, available_platforms.Count);
        return available_platforms[random_index];
    }

    Vector3 get_spawn_position(GameObject platform)
    {
        Debug.Log("In spawn position is null: " + (platform == null));
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

    public void platform_removed(GameObject platform)
    {
        Debug.Log("In platform removed: Map exists: " + (platform_powerup_map == null)
            + " Platform exists: " + (platform == null));
        if (platform != null && platform_powerup_map.ContainsKey(platform))
        {
            //if there is a powerup on the platform destroy it
            GameObject powerup_to_destroy = platform_powerup_map[platform];
            platform_powerup_map.Remove(platform);
            Destroy(powerup_to_destroy);
        }
        Destroy(platform);
    }
}
