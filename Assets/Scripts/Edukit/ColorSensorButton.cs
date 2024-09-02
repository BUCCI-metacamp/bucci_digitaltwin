using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class ColorSensorButton : MonoBehaviour
{
    public event Action<int, string> onColorSensorButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button colorSensorButton;
    void Start()
    {
        //Button colorSensorButton = GameObject.Find("colorSensorState").GetComponent<Button>();
        if (colorSensorButton != null)
        {
            colorSensorButton.onClick.AddListener(OnButtonPressed);
        }

        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = colorSensorButton.GetComponent<Image>();

    }

    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onColorSensorButtonChanged?.Invoke(4, "1");
        onColorSensorButtonChanged?.Invoke(4, "0");
    }

    private void Update()
    {
        if (MainValue.Instance.Sen1PowerState == "true")
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
