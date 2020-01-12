using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SportsBall : MonoBehaviour
{
    private Rigidbody rb;
    public DecalProjector decalProjector;
    public float contactForce, explosionRadius, upwardsForce;
    private Cinemachine.CinemachineImpulseSource impulse;
    private Vector3 spawnPos;
    private Renderer rend;
    private void Start()
    {
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        spawnPos = transform.position;
    }
    private void Update()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out hit))
        {
            decalProjector.size = new Vector3(1 + hit.distance, 1 + hit.distance, hit.distance * 2);
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
    public void Goal()
    {
        // TODO : Add explosion effect and respawn
        rend.enabled = false;
        rb.isKinematic = true;
        decalProjector.enabled = false;
        impulse.GenerateImpulse();
        Invoke("Respawn", 2f);
    }
    private void Respawn()
    {
        transform.position = spawnPos;
        rend.enabled = true;
        decalProjector.enabled = true;
        rb.isKinematic = false;

    }
}
