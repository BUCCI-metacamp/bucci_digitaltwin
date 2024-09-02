using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Factory;

public class AIReButton : MonoBehaviour
{
    public Button Btn;
    public event Action<int> onAIRe;  
   
    void Start()
    {
        if (Btn != null)
        {
            Btn.onClick.AddListener(onButtonClicked);
        }
    }

    void onButtonClicked()
    {
        onAIRe?.Invoke(0);        
    }
}
