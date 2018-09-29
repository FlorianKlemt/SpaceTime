using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHorizonChecker : MonoBehaviour {

    bool inside_horizon;

	// Use this for initialization
	void Start () {
        inside_horizon = true;
	}

    public bool is_inside_horizon()
    {
        return inside_horizon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            inside_horizon = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inside_horizon = false;
        }
    }
}
