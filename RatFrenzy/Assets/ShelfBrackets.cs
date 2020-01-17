using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfBrackets : MonoBehaviour
{
    public Transform referenceTransform;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = referenceTransform.position + new Vector3(((referenceTransform.lossyScale.x / 2) * offset.x), 0, 0);
    }
}
