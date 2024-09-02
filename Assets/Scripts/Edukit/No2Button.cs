using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class No2Button : MonoBehaviour
{
    public event Action<int, string> onNo2ButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button no2Button;
    void Start()
    {
       // Button no2Button = GameObject.Find("no2State").GetComponent<Button>();
        if (no2Button != null)
        {
            no2Button.onClick.AddListener(OnButtonPressed);
        }
        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = no2Button.GetComponent<Image>();
    }

    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onNo2ButtonChanged?.Invoke(7, "1");
        onNo2ButtonChanged?.Invoke(7, "0");
    }

    private void Update()
    {
        if (MainValue.Instance.M2State == "true")
        {

            buttonImage.color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {

            buttonImage.color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }
    }
}
