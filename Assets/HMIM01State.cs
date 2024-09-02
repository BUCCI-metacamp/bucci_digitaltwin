using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HMIM01State : MonoBehaviour
{
    public TextMeshProUGUI ChipCount;
    public TextMeshProUGUI ChipSensing;
    public TextMeshProUGUI ChipDelay;
    public Image Lamp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChipCount.text = MainValue.Instance.No1Count;
        ChipSensing.text = MainValue.Instance.No1ChipFull;
        ChipDelay.text = MainValue.Instance.No1DelayTime;
        if(MainValue.Instance.No1Push == "true")
        {
            Lamp.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else
        {
            Lamp.color = new Color(0.0f, 0.0f, 0.0f);
        }



    }
}
