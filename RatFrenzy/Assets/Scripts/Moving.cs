using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Moving : MonoBehaviour
{
    public float speed;
    public Vector3 translation, rotation;

    public float lifeSpan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation * speed * Time.deltaTime);
        transform.Rotate(rotation * speed * Time.deltaTime);
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0) Destroy(this.gameObject);
    }

    
}
