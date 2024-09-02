using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HMIM02State : MonoBehaviour
{
    public TextMeshProUGUI No2Count;
    public TextMeshProUGUI No2OperationMode;
    
    public Image ForntLamp;
    public Image BackLamp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        No2Count.text = MainValue.Instance.No2Count;

        if(MainValue.Instance.No2OperationMode == "true")
        {
            No2OperationMode.text = "모두";
        }
        else
        {
            No2OperationMode.text = "색 선별";
        }

        if(MainValue.Instance.No2InPoint == "true")
        {
            ForntLamp.color = new Color(1.0f, 0.0f, 0.0f);
            BackLamp.color = new Color(0.0f, 0.0f, 0.0f);
        }
        else if (MainValue.Instance.No2OutPoint == "true")
        {
            ForntLamp.color = new Color(0.0f, 0.0f, 0.0f);
            BackLamp.color = new Color(1.0f, 0.0f, 0.0f);
        }

    }
}
