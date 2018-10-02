using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;

	// Use this for initialization
	void Start () {
		
	}

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 30f;
 
    void Update()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
        Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 180+player.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
    }
}
