using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerinRange : MonoBehaviour
{
    public bool playerinRange = false;
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
        }
        catch
        {}
    }
}
