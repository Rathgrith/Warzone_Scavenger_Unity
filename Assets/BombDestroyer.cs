using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     // This method is called automatically by Unity when the explosion collider (set as trigger) enters another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the "ground" tag
        if(other.gameObject.CompareTag("ground"))
        {
            //Debug.Log("Destroyed");
            // Destroy the ground object
            Destroy(other.gameObject);
        }
    }
}
