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
    [SerializeField]
    TMP_InputField verticalDistanceField;
    [SerializeField]
    TMP_InputField horizontalDistanceField;

    bool _verbose;
    public bool Verbose { get => _verbose; set => _verbose = value; }

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

    // Default Value = 1m;
    private float _verticalDistance = 1;
    public float VerticalDistance
    {
        get => _verticalDistance;
        set
        {
            DebugManager.Instance.Log($"Setting new value for vertical distance: {value}");
            _verticalDistance = value;
            Refresh();
        }
    }

    // Default Value = 5m;
    private float _horizontalDistance = 5;
    public float HorizontalDistance
    {
        get => _horizontalDistance;
        set
        {
            DebugManager.Instance.Log($"Setting new value for horizontal distance: {value}");
            _horizontalDistance = value;
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

    private void Start()
    {
        verticalDistanceField.SetTextWithoutNotify(_verticalDistance.ToString());
        horizontalDistanceField.SetTextWithoutNotify(_horizontalDistance.ToString());
    }

    void Refresh()
    {
        const string SET = "Marked";
        const string UNSET = "Not Marked Yet";

        if (_verbose) readoutText.text = $"Clinometer Readout:\n\n-----\nTOP ANGLE: {_topAngle}\n-----\nBOTTOM ANGLE: {_bottomAngle}\n-----\nCURRENT ANGLE: {_currentAngle}";
        else readoutText.text = $"Clinometer Readout:\n\nTop Angle: {(_topAngle != 0 ? SET : UNSET)} \n" +
                $"Bottom Angle: {(_bottomAngle != 0 ? SET : UNSET)}";
        if(_bottomAngle != 0 && _topAngle != 0) 
        {
            float topAngle = Mathf.Abs(_topAngle - 90);
            float bottomAngle = Mathf.Abs(90 - _bottomAngle);
            float height = Mathf.Tan(topAngle * Mathf.Deg2Rad) * _horizontalDistance + Mathf.Tan(bottomAngle * Mathf.Deg2Rad) * _horizontalDistance;
            if(_verbose) readoutText.text = $"{readoutText.text}\nANGLE DIFFERENCE: {(_topAngle - _bottomAngle):0.00}";
            if(_verbose) readoutText.text = $"{readoutText.text}\nANGLES TO HORIZONTAL: ({topAngle}), ({bottomAngle})";
            readoutText.text = $"{readoutText.text}\nHEIGHT: {height:F3}m";
        }
    }

    public void OnUpdatedDistances()
    {
        ClinometerReadout.Instance.HorizontalDistance = float.Parse(horizontalDistanceField.text);
        ClinometerReadout.Instance.VerticalDistance = float.Parse(verticalDistanceField.text);
    }    
}
