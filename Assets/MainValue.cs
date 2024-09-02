using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainValue : BaseMono<MainValue>
{
    public int sucessCount;
    public int AIsucessCount;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Count;

    public string M1State;
    public string M2State;
    public string M3State;
    public string StartState;
    public string ResetState;
    public string EmergencyState;
    public string InputLimit;
    public string DataTime;
    public string ColorState;
    public string VosionState;
    public string DiceValue;
    public string Sen2PowerState;
    public string DiceComparisonValue;
    public string Sen1PowerState;
    public string ColorSensorSensing;
    public string GreenLampState;
    public string YellowLampState;
    public string RedLampState;
    public string No1DelayTime;
    public string No1Push;
    public string No1Count;
    public string No1ChipFull;
    public string No2Count;
    public string No2Chip;
    public string No2CubeFull;
    public string No2InPoint;
    public string No2OutPoint;
    public string No2Sol;
    public string No2SolAction;
    public string No2BackToSquare;
    public string No2OperationMode;
    public string No3ChipArrival;
    public string No3Count;
    public string No3Motor1Position;
    public string No3Motor2Position;
    public string No3Gripper;
    public string No3Motor1Action;
    public string No3Motor2Action;


    public List<AIReceiveData> aiViwer = new List<AIReceiveData>();

    public int GetSucessCount()
    {
        return sucessCount;
    }

    public int AIGetSucessCount()
    {
        return AIsucessCount;
    }
    // Start is called before the first frame update
    void Start()
    {
        sucessCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!Time)
        //    return;
        if (!Count)
            return;
        string str = string.Format("{0}", sucessCount);
        Count.text = str;

        //// ���� �ð� ��������
        //DateTime currentTime = DateTime.Now;
        //// ���� �ð� ���
        ////Debug.Log("���� �ð�: " + currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //str = string.Format(currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //Time.text = str;
    }

    //public void OnGUI(string str)
    //{
    //    // �ڽ��� ��ġ�� ũ�⸦ �����մϴ�.
    //    Rect boxRect = new Rect(50, 100, 400, 200);
    //    // �ڽ��� �׸��ϴ�.
    //    GUI.Box(boxRect, "Box Title");  

    //    // �ؽ�Ʈ�� �ڽ� �ȿ� ǥ���մϴ�.
    //    // �ؽ�Ʈ�� �ڽ��� ��ġ�� ũ�⿡ ��������� ǥ�õ˴ϴ�.
    //    Rect textRect = new Rect(boxRect.x + 10, boxRect.y + 30, boxRect.width - 20, boxRect.height - 40);
       
    //    GUI.Label(textRect, str);
    //}
}
