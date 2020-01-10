using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    public Vector3 area = new Vector3(1, 1, 1);
    public float maxRate;
    [Tooltip("Spawn rate (seconds)")]
    public float rate;
    [Tooltip("Spawn rate deviation amount (additive)")]
    public float rateVariance;
    [Tooltip("Rate increase per second")]
    public float rateInterpolationTime;
    public Vector3 sizeVariance = new Vector3(1, 1, 1);
    public Vector3 minimumSize = new Vector3(1, 1, 1);
    public Vector3 maximumSize = new Vector3(1, 1, 1);
    public Vector3 sizeInterpolationTime = new Vector3(1, 1, 1);

    private float spawnTime, realRate;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        spawnTime -= Time.deltaTime;
        realRate = Mathf.Lerp(rate, maxRate, t / rateInterpolationTime);
        if (spawnTime <= 0)
        {
            spawnTime = realRate;// + Random.Range(0, rateVariance);
            GameObject obj = Instantiate(objects[Random.Range(0, objects.Length)],
                new Vector3
                (
                    transform.position.x + Random.Range(-area.x / 2, area.x / 2),
                    transform.position.y + Random.Range(-area.y / 2, area.y / 2),
                    transform.position.z + Random.Range(-area.z / 2, area.z / 2)
                ),
                transform.rotation);
            obj.transform.localScale = Vector3.Scale(obj.transform.localScale, 
                new Vector3(
                    Random.Range(1, sizeVariance.x) * Mathf.Lerp(minimumSize.x, maximumSize.x, t / sizeInterpolationTime.x), 
                    Random.Range(1, sizeVariance.y) * Mathf.Lerp(minimumSize.y, maximumSize.y, t / sizeInterpolationTime.y), 
                    Random.Range(1, sizeVariance.z) * Mathf.Lerp(minimumSize.z, maximumSize.z, t / sizeInterpolationTime.z)
                )
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, area);
    }
}
