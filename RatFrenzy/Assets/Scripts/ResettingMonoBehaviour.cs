using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ResettingMonoBehaviour : MonoBehaviour
{
    public abstract void ResetOnSpawn();
}