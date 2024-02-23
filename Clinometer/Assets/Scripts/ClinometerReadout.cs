using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Stores clinometer values and handles updating the UI.
// (Bundled together the UI manager and the data holding singleton)
public class ClinometerReadout : MonoBehaviour
{
    public static ClinometerReadout Instance;

    [SerializeField]
    TextMeshProUGUI readoutText;

    private float _bottomAngle;
    public float BottomAngle
    {
        get => _bottomAngle;
        set
        {
            DebugManager.Instance.Log($"Setting new value for bottom angle: {value}");
            _bottomAngle = value;
            Refresh();
        }
    }

    private float _topAngle;
    public float TopAngle 
    { 
        get => _topAngle;
        set
        {
            DebugManager.Instance.Log($"Setting new value for top angle: {value}");
            _topAngle = value;
            Refresh();
        }
    }

    private Vector3 _currentAngle;
    public Vector3 CurrentAngle
    {
        get => _currentAngle;
        set 
        {
            DebugManager.Instance.Log($"Setting new value for current angle: {value}");
            if(value != _currentAngle )
            {
                _currentAngle = value;
                Refresh();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Refresh()
    {
        readoutText.text = $"Clinometer Readout:\n\n-----\nTOP ANGLE: {_topAngle}\n-----\nBOTTOM ANGLE: {_bottomAngle}\n-----\nCURRENT ANGLE: {_currentAngle}";
        if(_bottomAngle != _topAngle) { readoutText.text = $"{readoutText.text}\nANGLE DIFFERENCE: {(_topAngle - _bottomAngle):0.00}"; }
    }
}
