using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    public int platform_lives;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Astroid")
        {
            platform_lives -= 1;
            update_platform_state();
        }
    }

    public void update_platform_state()
    {
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        switch (platform_lives)
        {
            case 2:
                mesh_renderer.material.color = Color.white;
                break;
            case 1:
                mesh_renderer.material.color = Color.grey;
                break;
            case 0:
                Destroy(gameObject);
                break;
        }
    }
}
