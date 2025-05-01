using System;
using UnityEngine;
using System.Collections.Generic;

public class BulletsComponents : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _scripts;

    public void SetComponent(Type t)
    {
        foreach (var script in _scripts)
        {
            if (script.GetType() == t)
                script.enabled = !script.isActiveAndEnabled;
        }
    }

    public void Reset2()
    {
        foreach(var script in _scripts)
            script.enabled = false;
    }
}