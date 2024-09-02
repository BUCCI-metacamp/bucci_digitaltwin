using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Factory;

public class StartButton : MonoBehaviour
{
    public Button startBtn;
    public event Action<bool> onStartButton;
    public event Action<int> LedChange;
    public int caseType = 0;
    void Start()
    {
        if (startBtn != null)
        {
            startBtn.onClick.AddListener(onButtonClicked);
        }
    }

    void onButtonClicked()
    {
        if(caseType == 0)
        {
            M02 MainContent = GameObject.Find("M02").GetComponent<M02>();
            if (MainContent.GetContent() == 0)
            {
                Modal.Instance.ShowModal("품목이 선택이 되지 않아 시작이 불가능 합니다.");
                return;
            }
        }
        else if(caseType == 1)
        {
            Case2M02 MainContent = GameObject.Find("M02").GetComponent<Case2M02>();
            if (MainContent.GetContent() == 0)  
            {
                 
                Modal.Instance.ShowModal("품목이 선택이 되지 않아 시작이 불가능 합니다.");
                return;
            }
        }
        

        onStartButton?.Invoke(true);
        LedChange?.Invoke(2);
    }
}
