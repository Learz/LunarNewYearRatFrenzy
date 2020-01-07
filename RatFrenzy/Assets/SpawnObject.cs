using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    public Vector3 area = new Vector3(1,1,1);
    public float maxRate;
    [Tooltip("Spawn rate (seconds)")]
    public float rate;
    [Tooltip("Rate increase per second")]
    public float increase;
    
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
            Instantiate(objects[Random.Range(0,objects.Length-1)], 
                new Vector3
                (
                    transform.position.x + Random.Range(-area.x / 2, area.x / 2), 
                    transform.position.y + Random.Range(-area.y / 2, area.y / 2), 
                    transform.position.z + Random.Range(-area.z / 2, area.z / 2)
                ), 
                transform.rotation);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, area);
    }
}
