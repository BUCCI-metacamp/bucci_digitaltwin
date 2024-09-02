using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMILampTower : MonoBehaviour
{
    public List<Image> buttonImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainValue.Instance.GreenLampState == "true") //green
        {
            // 색상 변경 (예: 주황색)
            buttonImage[0].color = new Color(0.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            // 색상 변경 (예: 주황색)
            buttonImage[0].color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }

        if (MainValue.Instance.YellowLampState == "true") //yellow
        {
            // 색상 변경 (예: 주황색)
            buttonImage[1].color = new Color(1.0f, 1.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            // 색상 변경 (예: 주황색)
            buttonImage[1].color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }

        if (MainValue.Instance.RedLampState == "true") //red
        {
            // 색상 변경 (예: 주황색)
            buttonImage[2].color = new Color(1.0f, 0.0f, 0.0f); // RGB 값을 사용
        }
        else
        {
            // 색상 변경 (예: 주황색)
            buttonImage[2].color = new Color(0.0f, 0.0f, 0.0f); // RGB 값을 사용
        }

    }
}
