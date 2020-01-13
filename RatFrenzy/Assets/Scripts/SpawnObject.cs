using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    public int poolSize = 15;
    public Vector3 poolPosition = new Vector3(-500f, -500f, -500f);
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
    private GameObject[][] instancePool;
    private int[] currentObjectInPool;
    private Vector3[] originalScales;

    // Start is called before the first frame update
    void Start()
    {
        instancePool = new GameObject[objects.Length][];
        originalScales = new Vector3[objects.Length];
        for (int i = 0; i < instancePool.Length; i++)
        {
            instancePool[i] = new GameObject[poolSize];
            for (int x = 0; x < instancePool[i].Length; x++)
            {
                instancePool[i][x] = Instantiate(objects[i], poolPosition, Quaternion.identity);
            }
            originalScales[i] = objects[i].transform.localScale;
        }

        currentObjectInPool = new int[poolSize];
        for (int i = 0; i < currentObjectInPool.Length; i++)
        {
            currentObjectInPool[i] = 0;
        }
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
            SpawnNextInPool(Random.Range(0, objects.Length),
                new Vector3
                (
                    transform.position.x + Random.Range(-area.x / 2, area.x / 2),
                    transform.position.y + Random.Range(-area.y / 2, area.y / 2),
                    transform.position.z + Random.Range(-area.z / 2, area.z / 2)
                ),
                transform.rotation,
                new Vector3(
                    Random.Range(1, sizeVariance.x) * Mathf.Lerp(minimumSize.x, maximumSize.x, t / sizeInterpolationTime.x),
                    Random.Range(1, sizeVariance.y) * Mathf.Lerp(minimumSize.y, maximumSize.y, t / sizeInterpolationTime.y),
                    Random.Range(1, sizeVariance.z) * Mathf.Lerp(minimumSize.z, maximumSize.z, t / sizeInterpolationTime.z)
                )
            );
        }
    }

    private GameObject SpawnNextInPool(int i, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        GameObject objToSpawn = instancePool[i][currentObjectInPool[i]];
        ResettingMonoBehaviour resetter = objToSpawn.GetComponent<ResettingMonoBehaviour>();

        if (resetter != null && !resetter.readyToRespawn)
        {
            int poolId = 0;
            while (!resetter.readyToRespawn)
            {
                currentObjectInPool[i] = (currentObjectInPool[i] + 1) % poolSize;
                objToSpawn = instancePool[i][currentObjectInPool[i]];

                resetter = objToSpawn.GetComponent<ResettingMonoBehaviour>();
                poolId++;
                if (poolId > poolSize)
                {
                    Debug.Log("No more items available to spawn, consider increasing the pool size or make sure the resetting objects have a way to be respawned (readyToRespawn).");
                    return null;
                }
            }
        }

        Rigidbody rb = objToSpawn.GetComponent<Rigidbody>();
        if (resetter != null) resetter.ResetOnSpawn();
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;
        objToSpawn.transform.localScale = Vector3.Scale(originalScales[i], scale);
        currentObjectInPool[i] = (currentObjectInPool[i] + 1) % poolSize;
        if (rb != null) rb.velocity = Vector3.zero;
        return objToSpawn;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, area);
    }
}
