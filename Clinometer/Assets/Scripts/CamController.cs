using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    WebCamTexture camTexture;
    [SerializeField]
    RawImage camImage;

    private void Awake()
    {
        camImage.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        string frontCamName = null;
        string backCamName = null;
        foreach(var camDevice in WebCamTexture.devices)
        {
            DebugManager.Instance.Log($"Device: {camDevice.name}, Front: {camDevice.isFrontFacing}");
            if(camDevice.isFrontFacing) { frontCamName = camDevice.name; }
            else { backCamName = camDevice.name; }
        }

        if (string.IsNullOrEmpty(frontCamName)) return;

        camTexture = new WebCamTexture(backCamName);
        DebugManager.Instance.Log($"Width, Height: ({camTexture.width}, {camTexture.height}), \nRequested:" +
            $" ({camTexture.requestedWidth}, {camTexture.requestedHeight})");
        camImage.texture = camTexture;
        camTexture.Play();
    }

}
