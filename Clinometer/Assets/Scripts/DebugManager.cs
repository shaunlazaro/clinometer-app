using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    [SerializeField]
    TextMeshProUGUI text;

    private void Awake()
    {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        text.text = "";
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
        text.text += $"[{DateTime.Now}]:{msg}\n";
    }
}
