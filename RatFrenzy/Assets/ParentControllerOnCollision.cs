using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentControllerOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<GenericController>() != null)
            collision.collider.transform.parent = this.transform;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.GetComponent<GenericController>() != null)
            collision.collider.transform.parent = null;
    }
}
