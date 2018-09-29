using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSizeController : MonoBehaviour {
    private float black_hole_size;
    private Text black_hole_size_text;
    private string size_message;
	// Use this for initialization
	void Start () {
        size_message = "Lost: ";
        black_hole_size = 0;
        black_hole_size_text = GameObject.Find("BlackHoleSizeField").GetComponent<Text>();
        black_hole_size_text.text = size_message + black_hole_size;
    }

    private void Update()
    {
        black_hole_size_text.text = size_message + black_hole_size;
    }

    public float size()
    {
        return black_hole_size;
    }

    public void addSize(float to_add)
    {
        black_hole_size += to_add;
    }
}
