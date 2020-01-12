using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollectible : MonoBehaviour
{
    public float boostAmmount;
    private void OnTriggerEnter(Collider other)
    {
        RatController ctrl = other.GetComponent<RatController>();
        if (ctrl == null) return;
        ctrl.AddBoost(boostAmmount);
        Destroy(this.gameObject);
    }
}
