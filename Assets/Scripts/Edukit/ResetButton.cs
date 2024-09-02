using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public event Action<int, string> onResetButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button resetButton;
    void Start()
    {
        //Button resetButton = GameObject.Find("resetState").GetComponent<Button>();
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnButtonPressed);
        }
        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = resetButton.GetComponent<Image>();

    }

    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onResetButtonChanged?.Invoke(3, "1");
        onResetButtonChanged?.Invoke(3, "0");
    }

    private void Update()
    {
        if (MainValue.Instance.ResetState == "true")
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
