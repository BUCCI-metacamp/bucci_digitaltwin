using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class VisionSensorButton : MonoBehaviour
{
    public event Action<int, string> onVisionSensorButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button visionSensorButton;
    void Start()
    {
        //Button visionSensorButton = GameObject.Find("visionSensorState").GetComponent<Button>();
        if (visionSensorButton != null)
        {
            visionSensorButton.onClick.AddListener(OnButtonPressed);
        }

        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = visionSensorButton.GetComponent<Image>();

    }
    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onVisionSensorButtonChanged?.Invoke(5, "1");
        onVisionSensorButtonChanged?.Invoke(5, "0");
    }


    private void Update()
    {
        if (MainValue.Instance.Sen2PowerState == "true")
        {
            // 색상 변경 (예: 주황색)
            buttonImage.color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            // 색상 변경 (예: 주황색)
            buttonImage.color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }
    }
}
