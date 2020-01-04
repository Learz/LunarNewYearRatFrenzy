using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public RatManager mgr;
    public float speed, jumpHeight;
    public bool isJumping;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (mgr == null) throw new System.Exception("Error: Rat manager not defined");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir;
        Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
        dir = Camera.main.transform.TransformDirection(movement);
        dir.y = 0;
        rb.AddForce(dir * speed);
        Debug.DrawRay(transform.position, dir, Color.green);
    }
    public void Jump()
    {
        if (!isJumping)
        {
            rb.AddForce(Vector3.up * jumpHeight);
            isJumping = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }
}
