using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ResettingMonoBehaviour : MonoBehaviour
{
    public bool readyToRespawn = true;
    public abstract void ResetOnSpawn();
}