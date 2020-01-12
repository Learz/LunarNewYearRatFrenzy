using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsBall : MonoBehaviour
{
    private Rigidbody rb;
    public float contactForce, explosionRadius, upwardsForce;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<GenericController>() != null)
        {
            Debug.Log("Contact");
            rb.AddExplosionForce(contactForce, collision.GetContact(0).point, explosionRadius, upwardsForce);
        }
    }
}
