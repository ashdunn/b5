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
            Debug.Log(other.transform);
            playerinRange = (other.transform.parent.name == "Player");
                     
        }
        catch
        {}

    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            Debug.Log(other.transform.parent.name);
            playerinRange = !(other.transform.parent.name == "Player");
                        
        }
        catch
        {}
    }
}
