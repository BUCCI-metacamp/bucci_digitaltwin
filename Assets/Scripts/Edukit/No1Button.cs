using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class No1Button : MonoBehaviour
{
    public event Action<int, string> onNo1ButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button no1Button;

    void Start()
    {
        //Button no1Button = GameObject.Find("no1State").GetComponent<Button>();
        if (no1Button != null)
        {
            no1Button.onClick.AddListener(OnButtonPressed);
        }

        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = no1Button.GetComponent<Image>();

    }
    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onNo1ButtonChanged?.Invoke(6, "1");
        onNo1ButtonChanged?.Invoke(6, "0");
    }

    private void Update()
    {
        if (MainValue.Instance.M1State == "true")
        {
            
            buttonImage.color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            
            buttonImage.color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }
    }
}
