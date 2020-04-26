using UnityEngine;
using System.Collections;

public class turnLight : MonoBehaviour {

    // Use this for initialization

    public Light lamp;
    void Start () {
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (Input.GetKeyDown (KeyCode.Space)) {
            lamp.enabled = !lamp.enabled;
        }

    }
    // void OnTriggerEnter (Collider other)
    // {
    //     lamp.enabled = false;
    // }
}
