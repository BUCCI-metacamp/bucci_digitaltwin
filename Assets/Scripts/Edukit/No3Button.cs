using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class No3Button : MonoBehaviour
{
    public event Action<int, string> onNo3ButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    Image buttonImage;
    public Button no3Button;
    void Start()
    {
        //Button no3Button = GameObject.Find("no3State").GetComponent<Button>();
        if (no3Button != null)
        {
            no3Button.onClick.AddListener(OnButtonPressed);
        }

        // 버튼의 Image 컴포넌트 가져오기
        buttonImage = no3Button.GetComponent<Image>();

    }

    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onNo3ButtonChanged?.Invoke(8, "1");
        onNo3ButtonChanged?.Invoke(8, "0");
    }

    private void Update()
    {
        if (MainValue.Instance.M3State == "true")
        {

            buttonImage.color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {

            buttonImage.color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }
    }
}
