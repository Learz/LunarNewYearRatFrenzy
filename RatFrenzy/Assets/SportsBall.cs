using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SportsBall : MonoBehaviour
{
    private Rigidbody rb;
    public DecalProjector decalProjector;
    public float contactForce, explosionRadius, upwardsForce;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out hit))
        {
            decalProjector.size = new Vector3(1 + hit.distance, 1 + hit.distance, hit.distance*2);
        }
        decalProjector.transform.position = this.transform.position + new Vector3(0, this.transform.lossyScale.y / 2f - decalProjector.size.z / 2f, 0);
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
