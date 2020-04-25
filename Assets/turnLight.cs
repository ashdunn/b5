using UnityEngine;
using System.Collections;

public class turnLight : MonoBehaviour {

	// Use this for initialization

	public Light lamp;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			lamp.enabled = true;
		}
	
	}

	void OnTriggerEnter (Collider other)
	{
			lamp.enabled = false;
	}
}
