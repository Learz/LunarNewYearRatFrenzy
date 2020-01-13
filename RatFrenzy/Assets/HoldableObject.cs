using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class HoldableObject : ResettingMonoBehaviour
{
    public bool changeToPlayerColor;

    [HideInInspector]
    public RatController lastHolder { get; private set; }

    private RatController heldBy;
    private Collider ignoredCollision;
    private Rigidbody rb;
    private float holdingCooldown;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (heldBy != null)
        {
            rb.MovePosition(heldBy.holdingPoint.position);
            rb.MoveRotation(heldBy.holdingPoint.rotation);
        }
        else
        {
            holdingCooldown -= Time.deltaTime;
            if (holdingCooldown <= 0 && ignoredCollision != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), ignoredCollision, false);
                ignoredCollision = null;
            }
        }
    }

    public void Drop()
    {
        if (heldBy)
        {
            holdingCooldown = 2;
            heldBy.heldObject = null;
            heldBy = null;
            rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RatController player = collision.gameObject.GetComponent<RatController>();
        if (player != null && player.heldObject == null && heldBy == null)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            ignoredCollision = collision.collider;
            heldBy = player;
            lastHolder = player;
            player.heldObject = this;
            rb.isKinematic = true;
            readyToRespawn = false;
            Color col = lastHolder.mgr.GetPlayerColor();
            rend.material.SetColor("_BaseColor", col);
        }
    }

    public override void ResetOnSpawn()
    {
        rb.velocity = Vector3.zero;
        Drop();
        lastHolder = null;
        rend.material.SetColor("_BaseColor", Color.white);
    }
}
