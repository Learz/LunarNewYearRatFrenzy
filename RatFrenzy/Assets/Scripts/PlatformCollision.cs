using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformCollision : MonoBehaviour
{
    public Collider platform;
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb.velocity.y > 0)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), platform, true);
            }
            if (rb.velocity.y <= 0)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), platform, false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb.velocity.y <= 0)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), platform, false);
            }
        }
    }


    
}