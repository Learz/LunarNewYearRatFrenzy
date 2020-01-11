using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BindingsReferences", menuName = "ScriptableObjects/Bindings References", order = 2)]
public class BindingsReferenceList : ScriptableObject
{
    public List<BindingReference> bindingReferences;
}
[System.Serializable]
public class BindingReference
{
    public Binding.Action action;
    public Sprite kbmIcon, gamepadIcon;
}

