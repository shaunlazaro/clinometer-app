using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains logic for gyro/accelerometer, public methods for button handling, etc.
public class ClinometerManager : MonoBehaviour
{
    Gyroscope gyro;

    [SerializeField]
    GameObject distancePanel;
    [SerializeField]
    List<GameObject> crosshairs;

    [SerializeField]
    float updateCurrentAngleIntervalSecs;
    float updateCurrentAngleTimer = 0;

    [SerializeField]
    public bool gyroCompatibilityMode;
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;

        bool hasRealGyro = false;
        bool supportGyro = SystemInfo.supportsGyroscope;

        // Test if we have real values
#if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitForSeconds(1f);
        var gyrodata1 = gyro.attitude.eulerAngles * 10;
        yield return new WaitForSeconds(1f);
        var gyrodata2 = gyro.attitude.eulerAngles * 10;
        yield return new WaitForSeconds(1f);
        var gyrodata3 = gyro.attitude.eulerAngles * 10;
        hasRealGyro = (gyrodata1 != gyrodata2) && (gyrodata1 != gyrodata3) && (gyrodata2 != gyrodata3);
        Debug.Log(gyrodata1 + " = " + gyrodata2 + " = " + gyrodata3);
        Debug.Log("ANDROID GYRO DETECTED: " + hasRealGyro);
#endif

        DebugManager.Instance.Log($"Gyro status: (Real: {hasRealGyro}), (Supported: {supportGyro})");
        yield return null;
    }

    private void Update()
    {
        updateCurrentAngleTimer += Time.deltaTime;
        if(updateCurrentAngleTimer > updateCurrentAngleIntervalSecs)
        {
            updateCurrentAngleTimer = 0;
            ClinometerReadout.Instance.CurrentAngle = gyro.attitude.eulerAngles;
        }

        if(gyroCompatibilityMode)
        {
            yRotation += -Input.gyro.rotationRateUnbiased.y;
            xRotation += -Input.gyro.rotationRateUnbiased.x;
            ClinometerReadout.Instance.CurrentAngle = new Vector3(xRotation, yRotation, 0);
        }
    }

    float GetCurrentVerticalAngle() => gyroCompatibilityMode ? yRotation : gyro.attitude.eulerAngles[1];

    public void SetTopAngle()
    {
        ClinometerReadout.Instance.TopAngle = GetCurrentVerticalAngle();
    }

    public void SetBottomAngle() 
    { 
        ClinometerReadout.Instance.BottomAngle = GetCurrentVerticalAngle();
    }

    public void OnUpdatedDistances()
    {
        ClinometerReadout.Instance.OnUpdatedDistances(); // Ugly, but trying to keep this the controller/place for all public button callbacks.
    }

    public void ToggleDistancePanel()
    {
        bool setPanelActive = !distancePanel.activeSelf;
        distancePanel.SetActive(setPanelActive);
        foreach(var obj in crosshairs)
        {
            obj.SetActive(!setPanelActive);
        }
    }
}
