using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    public float speed, textureDampener = 500;
    public Material mat;
    private float offset = 0;

    private void OnCollisionStay(Collision collision)
    {

        //collision.rigidbody.AddForce(-transform.forward * speed * Time.deltaTime);
        //collision.transform.Translate(-transform.forward * speed * Time.deltaTime);
    }

    private void Update()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position - transform.forward * speed * Time.deltaTime);
        speed = Time.time / 10;
        //transform.Translate(-transform.forward * speed * Time.deltaTime);
        /*offset = Time.time * speed / textureDampener;
        mat.SetTextureOffset("_DetailMap", new Vector2(0, offset));*/
    }
}
