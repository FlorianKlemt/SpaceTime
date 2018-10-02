using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupGenerator : MonoBehaviour {
    public Transform black_hole;
    public Transform solarflare_powerup_prefab, timeslow_powerup_prefab, bounce_powerup_prefab;
    public int max_simultaneous_powerups;
    public float spawn_prob_per_sec_solarflare, spawn_prob_per_sec_timeslow, spawn_prob_per_sec_bounce;

    private List<GameObject> platforms;
    private int update_each = 1;
    private float time_to_next_update;
    private Dictionary<GameObject,GameObject> platform_powerup_map;
    private List<GameObject> blocked_platforms;
    
    // Use this for initialization
    void Start () {
        platforms = GameObject.FindGameObjectsWithTag("Platform").ToList();
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


        List<GameObject> new_platforms = GameObject.FindGameObjectsWithTag("Platform").ToList();
        platforms = new_platforms;

        List<GameObject> current_platform_powerup_keys = platform_powerup_map.Keys.ToList();
        foreach(GameObject key in current_platform_powerup_keys)
        {
            if (!platforms.Contains(key))
            {
                Destroy(platform_powerup_map[key]);
                platform_powerup_map.Remove(key);
                blocked_platforms.Remove(key);
            }
        }
    }

    void update_powerups()
    {
        bool spawn_solarflare = Random.value < spawn_prob_per_sec_solarflare;
        bool spawn_timeslow = Random.value < spawn_prob_per_sec_timeslow;
        bool spawn_bounce = Random.value < spawn_prob_per_sec_bounce;

        List<Transform> prefabs_to_spawn_this_update = new List<Transform>{};
        if(spawn_solarflare)
            prefabs_to_spawn_this_update.Add(solarflare_powerup_prefab);
        if (spawn_timeslow)
            prefabs_to_spawn_this_update.Add(timeslow_powerup_prefab);
        if (spawn_bounce)
            prefabs_to_spawn_this_update.Add(bounce_powerup_prefab);
        //random order
        prefabs_to_spawn_this_update = prefabs_to_spawn_this_update.OrderBy(x => Random.value).ToList();

        foreach (Transform t in prefabs_to_spawn_this_update)
        {
            spawn_powerup(t);
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
            if (powerup != null && platform_powerup_map != null)
            {
                platform_powerup_map.Add(spawn_platform, powerup.gameObject);
                blocked_platforms.Add(spawn_platform);
            }
            else
            {
                Debug.Log("Map: " + platform_powerup_map + "   Is Null: " + (platform_powerup_map == null));
                Debug.Log("Powerup: "+powerup +"   Is Null: "+ (powerup==null));
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
