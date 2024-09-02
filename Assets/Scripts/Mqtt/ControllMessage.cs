using System.Collections;
using System.Collections.Generic;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using Newtonsoft.Json;
using UnityEngine;
using System;
using Edukit;
using Factory;
using UnityEngine.UIElements;

public class ControllMessage : MonoBehaviour
{
    public List<ControllData> ControllDataList;
    public event Action<string> onControllMessage;
    public Case2StartButton C2StartButton;

    //
    public MqttStartButton[] startButtons;
    public StopButton[] stopButtons;
    public ResetButton[] resetButtons;
    public ColorSensorButton[] colorButtons;
    public VisionSensorButton[] visionButtons;
    public  No1Button[] no1Buttons;
    public No2Button[] no2Buttons;
    public No3Button[] no13uttons;
    // Start is called before the first frame update
    void Start()
    {
        /*ControllDataList = new List<ControllData>();
        ControllDataList.Add(new ControllData { tagId ="1", name = "StartState", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="2", name = "StopState", value = "0" } );
        ControllDataList.Add(new ControllData { tagId ="3", name = "ResetState", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="4", name = "ColorSensorState", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="5", name = "VisionSensorState", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="6", name = "No1State", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="7", name = "No2State", value = "0" });
        ControllDataList.Add(new ControllData { tagId ="8", name = "No3State", value = "0" });
        */        
        foreach (MqttStartButton button in startButtons)   
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onStartButtonChanged += GenerateData;
        }        
        foreach (StopButton button in stopButtons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onStopButtonChanged += GenerateData;
        }
       
        foreach (ResetButton button in resetButtons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onResetButtonChanged += GenerateData;
        }
        
        foreach (ColorSensorButton button in colorButtons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onColorSensorButtonChanged += GenerateData;
        }
        
        foreach (VisionSensorButton button in visionButtons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onVisionSensorButtonChanged += GenerateData;
        }
        
        foreach (No1Button button in no1Buttons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onNo1ButtonChanged += GenerateData;
        }
        
        foreach (No2Button button in no2Buttons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onNo2ButtonChanged += GenerateData;
        }
        
        foreach (No3Button button in no13uttons)
        {
            // �� ��ư�� ���� �ʿ��� �۾� ����
            button.onNo3ButtonChanged += GenerateData;
        }



        //GameObject.Find("startState").GetComponent<MqttStartButton>().onStartButtonChanged += GenerateData;
        //GameObject.Find("stopState").GetComponent<StopButton>().onStopButtonChanged += GenerateData;
        //GameObject.Find("resetState").GetComponent<ResetButton>().onResetButtonChanged += GenerateData;
        //GameObject.Find("colorSensorState").GetComponent<ColorSensorButton>().onColorSensorButtonChanged += GenerateData;
        //GameObject.Find("visionSensorState").GetComponent<VisionSensorButton>().onVisionSensorButtonChanged += GenerateData;
        //GameObject.Find("no1State").GetComponent<No1Button>().onNo1ButtonChanged += GenerateData;
        //GameObject.Find("no2State").GetComponent<No2Button>().onNo2ButtonChanged += GenerateData;
        //GameObject.Find("no3State").GetComponent<No3Button>().onNo3ButtonChanged += GenerateData;
        if(C2StartButton)
            C2StartButton.GetComponent<Case2StartButton>().onStartButtonChanged  += GenerateData;



    }
    
    public void GenerateData(int tagId, string value)
    {
        string[] name = {"startState","stopState","resetState","colorSensorState","visionSensorState","no1State","no2State","no3State" };
        ControllData data = new ControllData { tagId = tagId.ToString(), name = name[tagId-1], value = value };
        SerializeMessage(data);
    }

    private void SerializeMessage(ControllData data)
    {
        string message = JsonConvert.SerializeObject(data);
       //Debug.Log(message);
        onControllMessage?.Invoke(message);
    }
}
