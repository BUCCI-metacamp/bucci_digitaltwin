using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISetting : MonoBehaviour
{
    public event Action<float> ConSpeed; //�����̾� �ӵ�
    public event Action<float> AMachineDuration; //������� �ӵ�
    public event Action<float> AMachineTime; //������� Ĩ ����
    public event Action<float> BMachineDuratio; //���Ա���� �ӵ�
    public event Action<float> CMachineDuration; //�������� �ӵ�
    public event Action<float> CMachineTime; //�������� �����ð�
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



        //�����̾�
        float Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].ConvSpeedRatio);
        ConSpeed?.Invoke(Value);

        //�������
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M01Duration);
        AMachineDuration?.Invoke(Value);

        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M01Time);
        AMachineTime?.Invoke(Value);

        //���Ա����
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M02Duration);
        BMachineDuratio?.Invoke(Value);

        //��������
        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M03Duration);
        CMachineDuration?.Invoke(Value);

        Value = float.Parse(MainValue.Instance.aiViwer[targetNumber].M03Time);
        CMachineTime?.Invoke(Value);

    }
}
