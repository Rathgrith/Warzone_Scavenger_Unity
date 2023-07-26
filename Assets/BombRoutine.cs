using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRoutine : MonoBehaviour
{
    public GameObject explosion;
    public GameObject signalPrefab; // The prefab that represents the signal
    private GameObject signalInstance; // The instance of the signal in the scene

    // Start is called before the first frame update
    void Start()
    {
        signalInstance = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
            if (signalInstance != null)
                Destroy(signalInstance);
        }

        // Cast a ray downwards from the bomb
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            // If the ray hits the ground
            if (hit.collider.CompareTag("ground"))
            {
                // If the signal instance doesn't exist, create one
                if (signalInstance == null)
                {
                    signalInstance = Instantiate(signalPrefab, hit.point, Quaternion.identity);
                }
                // Otherwise, move the existing signal instance to the hit location
                else
                {
                    signalInstance.transform.position = hit.point;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            Destroy(this.gameObject);
            if (signalInstance != null)
                Destroy(signalInstance);
            GameObject explode = Instantiate(explosion, transform.position - new Vector3(0, 2, 0), Quaternion.identity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            Destroy(this.gameObject);
            Debug.Log("ignited");
            if (signalInstance != null)
                Destroy(signalInstance);
            GameObject explode = Instantiate(explosion, transform.position - new Vector3(0, 2, 0), Quaternion.identity);
        }
    }
}

