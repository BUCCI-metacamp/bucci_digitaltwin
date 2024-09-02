using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamView : MonoBehaviour
{
    public RawImage webcamDisplay;

    void Start()
    {
        // 모든 웹캠 장치 목록을 가져옵니다.
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No webcam detected.");
            return;
        }

        // 첫 번째 웹캠을 사용하여 WebCamTexture를 생성합니다.
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
        // RawImage 컴포넌트에 웹캠 영상을 할당합니다.
        webcamDisplay.texture = webcamTexture;

        // 웹캠을 시작합니다.
        webcamTexture.Play();
    }
}
