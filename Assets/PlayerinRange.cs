using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerinRange : MonoBehaviour
{
    public bool playerinRange = false;
    public bool QAtrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P) & playerinRange)
        // {
        //     QAtrigger = true;
        // }
        // Debug.Log(this.transform.parent.name + ": " + QAtrigger);
        
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            
            playerinRange = (other.transform.parent.name == "Player");     
            // Debug.Log(other.transform.parent.name);
            Debug.Log(this.transform.parent.name + ": " + playerinRange);
        }
        catch
        {}

    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            // Debug.Log(other.transform.parent.name);
            playerinRange = !(other.transform.parent.name == "Player");
            Debug.Log(this.transform.parent.name + ": " + playerinRange);
            // QAtrigger = false;

        }
        catch
        {}
    }
}
