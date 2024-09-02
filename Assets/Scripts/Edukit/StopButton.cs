using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    public event Action<int, string> onStopButtonChanged; //int: tagId, string: value
    public Mqtt mqtt;
    public Button stopButton;
    void Start()  
    {
        //stopButton = GameObject.Find("stopState").GetComponent<Button>();
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnButtonPressed);
        }

    }

    void OnButtonPressed() //버튼이 눌렸을 때 이밴트 발생
    {
        onStopButtonChanged?.Invoke(2, "1");
        onStopButtonChanged?.Invoke(2, "0");
    }
}
