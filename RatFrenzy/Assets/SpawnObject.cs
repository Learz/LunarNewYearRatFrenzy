using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject obj;
    public float rate, increase, maxRate;
    
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        rate = Mathf.Clamp(rate - increase * Time.deltaTime, maxRate, float.PositiveInfinity);
        if (spawnTime >= rate)
        {
            spawnTime = 0;
            Instantiate(obj, transform.position, transform.rotation);
        }
    }
}
