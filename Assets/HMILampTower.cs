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
            // ���� ���� (��: ��Ȳ��)
            buttonImage[0].color = new Color(0.0f, 1.0f, 0.0f); // RGB ���� ���
        }
        else
        {
            // ���� ���� (��: ��Ȳ��)
            buttonImage[0].color = new Color(0.0f, 0.0f, 0.0f); // RGB ���� ���
        }

        if (MainValue.Instance.YellowLampState == "true") //yellow
        {
            // ���� ���� (��: ��Ȳ��)
            buttonImage[1].color = new Color(1.0f, 1.0f, 0.0f); // RGB ���� ���
        }
        else
        {
            // ���� ���� (��: ��Ȳ��)
            buttonImage[1].color = new Color(0.0f, 0.0f, 0.0f); // RGB ���� ���
        }

        if (MainValue.Instance.RedLampState == "true") //red
        {
            // ���� ���� (��: ��Ȳ��)
            buttonImage[2].color = new Color(1.0f, 0.0f, 0.0f); // RGB ���� ���
        }
        else
        {
            // ���� ���� (��: ��Ȳ��)
            buttonImage[2].color = new Color(0.0f, 0.0f, 0.0f); // RGB ���� ���
        }

    }
}
