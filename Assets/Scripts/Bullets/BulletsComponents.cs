using System.Collections.Generic;
using UnityEngine;

public class BulletsComponents : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _scripts;

    public void SetActiveScripts()
    {
        foreach (var script in _scripts)
            script.enabled = !script.isActiveAndEnabled;
    }
}