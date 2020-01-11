using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBindingObj : MonoBehaviour
{
    public BindingsReferenceList referenceList;
    public Image icon;
    public TMPro.TMP_Text label;
    private Binding.Action curAction;
    private bool gp;
    private float startTime;
    public void SetData(Binding.Action action, string desc)
    {
        curAction = action;
        label.text = desc;
        startTime = Time.time;
        gp = true;
        icon.sprite = referenceList.bindingReferences[(int)curAction].gamepadIcon;
    }
    private void Update()
    {
        // TODO : Alternate between kb/m icon and controller icon
        if (Mathf.Floor((Time.time - startTime) * 0.5f % 2) == 0)
        {
            if (gp) return;
            gp = true;
            icon.sprite = referenceList.bindingReferences[(int)curAction].gamepadIcon;
        }
        else
        {
            if (!gp) return;
            gp = false;
            icon.sprite = referenceList.bindingReferences[(int)curAction].kbmIcon;
        }
    }
}
