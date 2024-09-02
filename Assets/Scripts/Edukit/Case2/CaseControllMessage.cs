using System.Collections;
using System.Collections.Generic;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using Newtonsoft.Json;
using UnityEngine;
using System;
using Edukit;
using Factory;
using UnityEngine.UIElements;

public class CaseControllMessage : MonoBehaviour
{
    public List<ControllData> ControllDataList;
    public event Action<string> onControllMessage;
    public Case2StartButton C2StartButton;
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

        GameObject.Find("AIRequest").GetComponent<AIReButton>().onAIRe += GenerateData;
      



    }
    
    public void GenerateData(int value)
    {
        //string[] name = {"startState","stopState","resetState","colorSensorState","visionSensorState","no1State","no2State","no3State" };
        AIRequestData data = new AIRequestData { request = value};
        SerializeMessage(data);
    }

    private void SerializeMessage(AIRequestData data)
    {
        string message = JsonConvert.SerializeObject(data);
       //Debug.Log(message);
        onControllMessage?.Invoke(message);
    }
}
