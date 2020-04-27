using UnityEngine;
using System.Collections;

public class turnLight : MonoBehaviour {

    // Use this for initialization

    public Light lamp;
    public bool switchinRange = false;

    void Start () 
    {
    }

    // Update is called once per frame
    void FixedUpdate () {
        switchinRange = GameObject.Find("Cylinder").GetComponent<PlayerinRange>().switchinRange;
        if (Input.GetKeyDown (KeyCode.Space) & switchinRange) {
            lamp.enabled = !lamp.enabled;
        }

    }
    // void OnTriggerEnter (Collider other)
    // {
    //     lamp.enabled = false;
    // }
}
