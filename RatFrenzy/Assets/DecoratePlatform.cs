using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoratePlatform : ResettingMonoBehaviour
{
    public GameObject[] decorations;
    public Transform platform;

    private GameObject[] decorationInstances;
    // Start is called before the first frame update
    void Start()
    {
        decorationInstances = new GameObject[decorations.Length];
        for (int i = 0; i < decorations.Length; i++)
        {
            decorationInstances[i] = Instantiate(decorations[i], transform.position, transform.rotation);
            PositionObject(decorationInstances[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PositionObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Vector3 pos = new Vector3(Random.Range(platform.position.x - platform.lossyScale.x / 2.2f, platform.position.x + platform.lossyScale.x / 2.2f), platform.position.y + 0.2f, platform.position.z - 0.1f);
        rb.MovePosition(pos);
        obj.transform.position = pos;
    }

    public override void ResetOnSpawn()
    {
        for (int i = 0; i < decorationInstances.Length; i++)
        {
            PositionObject(decorationInstances[i]);
        }
    }
}
