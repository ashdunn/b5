using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerinRange : MonoBehaviour
{
    public bool playerinRange = false;

    public bool switchinRange = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            playerinRange = (other.transform.parent.name == "Player");
            // Debug.Log(other.transform.parent.name);
            if (this.transform.parent.name == "Lamp")
            {
                switchinRange = true;
                Debug.Log(this.transform.parent.name + ": " + playerinRange);
            }
        }
        catch
        {
        }
    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            if (this.transform.parent.name == "Lamp")
            {
                switchinRange = false;
                Debug.Log(this.transform.parent.name + ": " + switchinRange);
            }
            else if (this.transform.parent.name == "Player")
            {
                playerinRange = false;
            }
        }
        catch
        {
        }
    }
}