using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains logic for gyro/accelerometer, button handling, etc.
public class ClinometerManager : MonoBehaviour
{
    Gyroscope gyro;
    [SerializeField]
    float updateCurrentAngleIntervalSecs;
    float updateCurrentAngleTimer = 0;

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
    }

    float GetCurrentVerticalAngle() => gyro.attitude.eulerAngles[1];

    public void SetTopAngle()
    {
        ClinometerReadout.Instance.TopAngle = GetCurrentVerticalAngle();
    }

    public void SetBottomAngle() 
    { 
        ClinometerReadout.Instance.BottomAngle = GetCurrentVerticalAngle();
    }
}
