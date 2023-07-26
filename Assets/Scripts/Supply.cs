using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : MonoBehaviour
{
    public GameObject parachute;
    // Start is called before the first frame update
    void Start()
    {
    //     parachute = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    { if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }

    }
   
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("bottom")|| other.gameObject.CompareTag("Supply"))
        {
            Transform parachute = transform.Find("Parachute");
            if (parachute != null) // Make sure the Parachute exists
            {
                Destroy(parachute.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameStat.Instance.IncreaseDestroyedSuppliesCount();
            Destroy(this.gameObject);
        }
    }


}
