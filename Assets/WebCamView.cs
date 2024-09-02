using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamView : MonoBehaviour
{
    public RawImage webcamDisplay;

    void Start()
    {
        // ��� ��ķ ��ġ ����� �����ɴϴ�.
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No webcam detected.");
            return;
        }

        // ù ��° ��ķ�� ����Ͽ� WebCamTexture�� �����մϴ�.
        WebCamTexture webcamTexture = null;
        foreach (WebCamDevice cam in devices)
        {
            if(cam.name == "GENERAL WEBCAM")
            {
                webcamTexture = new WebCamTexture(cam.name);
                Debug.Log(cam.name);
            }
        }

        if (webcamTexture == null)
            return;
        // RawImage ������Ʈ�� ��ķ ������ �Ҵ��մϴ�.
        webcamDisplay.texture = webcamTexture;

        // ��ķ�� �����մϴ�.
        webcamTexture.Play();
    }
}
