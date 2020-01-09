using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Moving : MonoBehaviour
{
    public float speed, acceleration, maxSpeed;
    public Vector3 translation, rotation;
    public bool dies;
    public float lifeSpan;

    private float realspeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation * realspeed * Time.deltaTime);
        transform.Rotate(rotation * realspeed * Time.deltaTime);
        lifeSpan -= Time.deltaTime;
        if (dies && lifeSpan <= 0) Destroy(this.gameObject);
        realspeed = Mathf.Clamp(speed + Time.timeSinceLevelLoad * acceleration, 0, maxSpeed);
    }


}
