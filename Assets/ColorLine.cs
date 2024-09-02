using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorLine : MonoBehaviour
{
    public Image Lamp;
    public TextMeshProUGUI ColorState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MainValue.Instance.ColorSensorSensing == "true")
        {
            Lamp.color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            Lamp.color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }

        //if(MainValue.Instance.ColorState == "true")
        //{
        //    ColorState.text = MainValue.Instance.ColorState;
        //}
        //else
        //{
        //    ColorState.text = MainValue.Instance.ColorState;
        //}
    }
}
