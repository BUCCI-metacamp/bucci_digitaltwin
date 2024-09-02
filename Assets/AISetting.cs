using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISetting : MonoBehaviour
{
    public event Action<float> ConSpeed; //컨베이어 속도
    public event Action<float> AMachineDuration; //반출공정 속도
    public event Action<float> AMachineTime; //반출공정 칩 간격
    public event Action<float> BMachineDuratio; //투입기공정 속도
    public event Action<float> CMachineDuration; //가공공정 속도
    public event Action<float> CMachineTime; //가공공정 가공시간
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AIOKButton(int targetNumber)
    {



        //컨베이어
        float Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].ConvSpeedRatio);
        ConSpeed?.Invoke(Value);

        //반출공정
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M01Duration);
        AMachineDuration?.Invoke(Value);

        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M01Time);
        AMachineTime?.Invoke(Value);

        //투입기공정
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M02Duration);
        BMachineDuratio?.Invoke(Value);

        //가공공정
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M03Duration);
        CMachineDuration?.Invoke(Value);

        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M03Time);
        CMachineTime?.Invoke(Value);

    }
}
