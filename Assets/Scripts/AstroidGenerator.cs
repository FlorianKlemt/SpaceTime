using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidGenerator : MonoBehaviour {
    public GameObject astroid_prefab;
    public GameObject center;
    public float spawn_distance_to_center;

    private float time_till_astroid;
    // Use this for initialization
    void Start () {
        time_till_astroid = 2.0f;
    }
	
	// Update is called once per frame
	void Update () {
		if((time_till_astroid-=Time.deltaTime) <= 0)
        {
            generateAstroid();
            time_till_astroid = 2.0f;
        }
	}

    void generateAstroid()
    {
        Vector3 random_on_circle = Random.insideUnitCircle.normalized;
        Vector3 spawn_offset = new Vector3(random_on_circle.x, 0, random_on_circle.y) * spawn_distance_to_center;
        Debug.Log(spawn_offset);
        Vector3 spawn_position = center.transform.position + spawn_offset;
        Instantiate(astroid_prefab, spawn_position, Random.rotation);
    }
}
