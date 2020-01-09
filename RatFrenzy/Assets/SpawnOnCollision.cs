using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnCollision : MonoBehaviour
{
    public GameObject prefab;
    public ParticleSystem PSystem;
    private List<ParticleCollisionEvent> CollisionEvents;
    private void Start()
    {
        CollisionEvents = new List<ParticleCollisionEvent>();
    }
    public void OnParticleCollision(GameObject other)
    {
        int collCount = PSystem.GetSafeCollisionEventSize();

        int eventCount = PSystem.GetCollisionEvents(other, CollisionEvents);

        foreach (ParticleCollisionEvent collision in CollisionEvents)
        {
            Vector3 pos = new Vector3(collision.intersection.x, collision.intersection.y - PSystem.main.startSize.constant / 2, collision.intersection.z);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
