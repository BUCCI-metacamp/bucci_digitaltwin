using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System;
using Factory;
using WI;


public class ControlSettingView : BaseMono<ControlSettingView>
{

    public CanvasController panelActive;


    [Header("MainControl_Left")]
    public TMP_InputField loadCapacity; // �� ���귮
    public TextMeshProUGUI a1Value;
    public TextMeshProUGUI a2Value;
    public TextMeshProUGUI a3Value;
    public event Action<string> TotalOutput; //�� ���귮


    [Header("MainControl_Right")]
    public TMP_Dropdown mainContent; //ǰ��
    public TMP_Dropdown mainCapacity; //�뷮
    public TMP_InputField cost; //����
    public event Action<Chip.InsideState> Item; //ǰ��
    public event Action<float> volume; //�뷮


    [Header("ConControl")]
    public TMP_InputField conSpeedTxt; //�����̾� �ӵ� txt
    public Slider conSpeedSli; //�����̾� �ӵ� Slider
    public event Action<float> ConSpeed; //�����̾� �ӵ�


    [Header("AMControl")]
    public TMP_InputField amSpeedTxt; //������� �ӵ� �ؽ�Ʈ
    public TMP_InputField amIntervalTxt;//������� Ĩ���� �ؽ�Ʈ
    public Slider amSpeedSli;//������� �ӵ� �����̵�
    public Slider amIntervalSli; //������� Ĩ���� �����̵�
    public event Action<float> AMachineDuration; //������� �ӵ�
    public event Action<float> AMachineTime; //������� Ĩ ����


    [Header("BMControl")]
    public TMP_InputField bmSpeedTxt; //���Ա� �ӵ� �ؽ�Ʈ
    public Slider bmSpeedSli; //���Ա� �ӵ� �����̴�
    public event Action<float> BMachineDuratio; //���Ա���� �ӵ�


    [Header("CMControl")]
    public TMP_InputField cmSpeedTxt; //������ �ӵ� �ؽ�Ʈ
    public TMP_InputField cmManufacturingTxt; //������ ���� �ؽ�Ʈ
    public Slider cmSpeedSli; //������ �ӵ� �����̵� 
    public Slider cmManufacturingSli; //������ ���� �����̵� 
    public event Action<float> CMachineDuration; //�������� �ӵ�
    public event Action<float> CMachineTime; //�������� �����ð�


    [Header("RMControl")]
    public TMP_InputField rb1stSpeedTxt; // 1�� �ӵ� �ؽ�Ʈ
    public TMP_InputField rb2stSpeedTxt; // 2�� �ӵ� �ؽ�Ʈ
    public Slider rb1stSpeedSli; //1��ӵ� �����̵�
    public Slider rb2stSpeedSli; //2��ӵ� �����̵�
    public event Action<float> RMachine1stSpeed; //1�� �ӵ�
    public event Action<float> RMachine2stSpeed; //2�� �ӵ�


    [Header("MainButton")]
    public Button OKButton;

    private const float minSliderValue = 0f;
    private const float maxSliderValue = 100f;


    [Header("LoadObject")]
    public ManageCase2 Manage;
    public M01 AMachine;
    public M02 BMachine;
    public M03 CMachine;
    public Case2Robot RMachine;
    public Chip chips;

    //���� ���빰
    Chip.InsideState contentType;
    public Manage Case2Mange;
  

    void Start()
    {
        // Add listeners for TMP_InputField
        AddInputFieldListeners(loadCapacity);
        AddInputFieldListeners(cost);
        AddInputFieldListeners(conSpeedTxt);
        AddInputFieldListeners(amSpeedTxt);
        AddInputFieldListeners(amIntervalTxt);
        AddInputFieldListeners(bmSpeedTxt);
        AddInputFieldListeners(cmSpeedTxt);
        AddInputFieldListeners(cmManufacturingTxt);
        AddInputFieldListeners(rb1stSpeedTxt);
        AddInputFieldListeners(rb2stSpeedTxt);

        // Add listeners for TMP_Dropdown
        mainContent.onValueChanged.AddListener(OnMainContentChanged);
        mainCapacity.onValueChanged.AddListener(OnMainCapacityChanged);

        // Add listeners for Slider
        conSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(conSpeedTxt, value));
        amSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(amSpeedTxt, value));
        amIntervalSli.onValueChanged.AddListener(value => OnSliderValueChanged(amIntervalTxt, value));
        bmSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(bmSpeedTxt, value));
        cmSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(cmSpeedTxt, value));
        cmManufacturingSli.onValueChanged.AddListener(value => OnSliderValueChanged(cmManufacturingTxt, value));
        rb1stSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(rb1stSpeedTxt, value));
        rb2stSpeedSli.onValueChanged.AddListener(value => OnSliderValueChanged(rb2stSpeedTxt, value));

        // Set slider range
        SetSliderRange(conSpeedSli);
        SetSliderRange(amSpeedSli);
        SetSliderRange(amIntervalSli);
        SetSliderRange(bmSpeedSli);
        SetSliderRange(cmSpeedSli);
        SetSliderRange(cmManufacturingSli);
        SetSliderRange(rb1stSpeedSli);
        SetSliderRange(rb2stSpeedSli);

        // Add listener for the OK button   
        OKButton.onClick.AddListener(OnOKButtonClick);

        GameObject.Find("CnavasControll").GetComponent<CanvasController>().SettingStart += LoadInitialSettings;

        
    }
    

    void SetSliderRange(Slider slider) 
    {
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
    }

    void AddInputFieldListeners(TMP_InputField inputField)
    {
        inputField.onValueChanged.AddListener(value => OnInputFieldValueChanged(inputField, value));
    }

    // TMP_InputField listeners
    void OnInputFieldValueChanged(TMP_InputField inputField, string value)
    {
        // Restrict input to numeric values only
        //value = Regex.Replace(value, "[^0-9]", "");
        //inputField.text = value;
        //float floatValue = /*string.IsNullOrEmpty(value) ? 0 :*/ float.Parse(value);
        //floatValue = Mathf.Clamp(floatValue, 0, 100);

        //// Find the corresponding slider and update its value  
        //if (inputField == cost) amSpeedSli.value = floatValue;
        //else if (inputField == conSpeedTxt) conSpeedSli.value = floatValue;
        //else if (inputField == amSpeedTxt) amSpeedSli.value = floatValue;
        //else if (inputField == amIntervalTxt) amIntervalSli.value = floatValue;
        //else if (inputField == bmSpeedTxt) bmSpeedSli.value = floatValue;
        //else if (inputField == cmSpeedTxt) cmSpeedSli.value = floatValue;
        //else if (inputField == cmManufacturingTxt) cmManufacturingSli.value = floatValue;
        //else if (inputField == rb1stSpeedTxt) rb1stSpeedSli.value = floatValue;
        //else if (inputField == rb2stSpeedTxt) rb2stSpeedSli.value = floatValue;

        Debug.Log(inputField.name + " Changed: " + value);
        
    }

    void OnSliderValueChanged(TMP_InputField inputField, float value)
    {
        // Update the corresponding TMP_InputField
        inputField.text = value.ToString("0");
        Debug.Log(inputField.name + " Slider Changed: " + value);
    }

    // TMP_Dropdown listeners
    void OnMainContentChanged(int value)
    {
        Debug.Log("Main Content Changed: " + value);
    }

    void OnMainCapacityChanged(int value)
    {
        Debug.Log("Main Capacity Changed: " + value);
    }

    // Function to be called when the OK button is clicked
    void OnOKButtonClick()
    {
        // Read and log values from TMP_InputFields
        Debug.Log("Load Capacity: " + loadCapacity.text);
        Debug.Log("Cost: " + cost.text);
        Debug.Log("Con Speed Txt: " + conSpeedTxt.text);
        Debug.Log("AM Speed Txt: " + amSpeedTxt.text);
        Debug.Log("AM Interval Txt: " + amIntervalTxt.text);
        Debug.Log("BM Speed Txt: " + bmSpeedTxt.text);
        Debug.Log("CM Speed Txt: " + cmSpeedTxt.text);
        Debug.Log("CM Manufacturing Txt: " + cmManufacturingTxt.text);
        Debug.Log("RB 1st Speed Txt: " + rb1stSpeedTxt.text);
        Debug.Log("RB 2nd Speed Txt: " + rb2stSpeedTxt.text);

        // Read and log values from TMP_Dropdowns
        Debug.Log("Main Content: " + mainContent.value);
        Debug.Log("Main Capacity: " + mainCapacity.value);

        // Read and log values from Sliders
        Debug.Log("Con Speed Sli: " + conSpeedSli.value);
        Debug.Log("AM Speed Sli: " + amSpeedSli.value);
        Debug.Log("AM Interval Sli: " + amIntervalSli.value);
        Debug.Log("BM Speed Sli: " + bmSpeedSli.value);
        Debug.Log("CM Speed Sli: " + cmSpeedSli.value);
        Debug.Log("CM Speed Sli: " + cmManufacturingSli.value);        
        Debug.Log("RB 1st Speed Sli: " + rb1stSpeedSli.value);
        Debug.Log("RB 2nd Speed Sli: " + rb2stSpeedSli.value);

        //����
        TotalOutput?.Invoke(loadCapacity.text);

        if (System.Enum.IsDefined(typeof(Chip.InsideState), mainContent.value))
        {
            Chip.InsideState newState = (Chip.InsideState)mainContent.value;
            Item?.Invoke(newState);
            Debug.Log("New InsideState set: " + newState);
        }

        float bTime = (mainCapacity.value + 1) * 3;
        volume?.Invoke(bTime);

        //�����̾�
        float Value = float.Parse(conSpeedTxt.text);
        ConSpeed?.Invoke(Value);
        Value = float.Parse(amSpeedTxt.text);
        //�������
        AMachineDuration?.Invoke(Value);
        Value = float.Parse(amIntervalTxt.text);        
        AMachineTime?.Invoke(Value);
        //���Ա����
        Value = float.Parse(bmSpeedTxt.text);
        BMachineDuratio?.Invoke(Value);
        //��������
        Value = float.Parse(cmSpeedTxt.text);
        CMachineDuration?.Invoke(Value);
        Value = float.Parse(cmManufacturingTxt.text);
        CMachineTime?.Invoke(Value);
        //�κ��Ȱ���
        float m_Axis1 = (rb1stSpeedSli.value / 100);
        float m_Axis2 = (rb2stSpeedSli.value / 100);
        RMachine1stSpeed?.Invoke(m_Axis1); 
        RMachine2stSpeed?.Invoke(m_Axis2);
          
        panelActive.HideCanvas();
    }
    void LoadInitialSettings()
    {
        //GameObject.Find("M01").GetComponent<ManageCase2>().SettingStart += ApplySetting;
        string strobj; //���� ����

        if(Case2Mange)
        {
            //�Ѱ���
            strobj = string.Format("{0}", Case2Mange.TotalAmount);
            ApplySetting("loadCapacity", strobj);
        }   
        else
        {
            //�Ѱ���
            strobj = string.Format("{0}", GameObject.Find("CASE").GetComponent<ManageCase2>().TotalAmount);
            ApplySetting("loadCapacity", strobj);
        }
        

        //����
        strobj = string.Format("{0}", GameObject.Find("M02").GetComponent<M02>().GetContent()); 
        ApplySetting("mainContent", strobj);

        //�뷮
        strobj = string.Format("{0}", (GameObject.Find("M02").GetComponent<M02>().M02Time/3)-1);
        ApplySetting("mainCapacity", strobj);

        //�����̾�
        strobj = string.Format("{0}", GameObject.Find("Conveyor").GetComponent<CaseConveyor>().ConvSpeedRatio);
        ApplySetting("conSpeedTxt", strobj);
         
        //AMachine
        strobj = string.Format("{0}", GameObject.Find("M01").GetComponent<M01>().M1SpeedRatio);
        ApplySetting("amSpeedTxt", strobj);

        strobj = string.Format("{0}", GameObject.Find("M01").GetComponent<M01>().M01Time);
        ApplySetting("amIntervalTxt", strobj);

        //BMachine
        strobj = string.Format("{0}", GameObject.Find("M02").GetComponent<M02>().M2SpeedRatio);
        ApplySetting("bmSpeedTxt", strobj);

        //CMachine
        strobj = string.Format("{0}", GameObject.Find("M03").GetComponent<M03>().M3SpeedRatio);
        ApplySetting("cmSpeedTxt", strobj);

        strobj = string.Format("{0}", GameObject.Find("M03").GetComponent<M03>().M03Time);
        ApplySetting("cmManufacturingTxt", strobj);

        //RMachine
        strobj = string.Format("{0}", GameObject.Find("M03_ani").GetComponent<Case2Robot>().Axis1stmoveSpeed*100);
        ApplySetting("rb1stSpeedSli", strobj);

        strobj = string.Format("{0}", GameObject.Find("M03_ani").GetComponent<Case2Robot>().Axis2stmoveSpeed*100);
        ApplySetting("rb2stSpeedSli", strobj);



    }


    void ApplySetting(string id, string value)
    {
        switch (id)
        {
            case "loadCapacity":
                loadCapacity.text = value;
                break;
            case "cost":
                cost.text = value; 
                break;
            case "conSpeedTxt":
                conSpeedTxt.text = value;
                break;
            case "amSpeedTxt":
                amSpeedTxt.text = value;
                break;
            case "amIntervalTxt":
                amIntervalTxt.text = value;
                break;
            case "bmSpeedTxt":
                bmSpeedTxt.text = value;
                break;
            case "cmSpeedTxt":
                cmSpeedTxt.text = value;
                break;
            case "cmManufacturingTxt":
                cmManufacturingTxt.text = value;
                break;
            case "rb1stSpeedTxt":
                rb1stSpeedTxt.text = value;
                break;
            case "rb2stSpeedTxt":
                rb2stSpeedTxt.text = value;
                break;
            case "mainContent":
                mainContent.value = ((int)GameObject.Find("M02").GetComponent<M02>().GetContent());
                break;
            case "mainCapacity":
                mainCapacity.value = int.Parse(value);
                break;
            case "conSpeedSli":
                conSpeedSli.value = float.Parse(value);
                break;
            case "amSpeedSli":
                amSpeedSli.value = float.Parse(value);
                break;
            case "amIntervalSli":
                amIntervalSli.value = float.Parse(value);
                break;
            case "bmSpeedSli":
                bmSpeedSli.value = float.Parse(value);
                break;
            case "cmSpeedSli":
                cmSpeedSli.value = float.Parse(value);
                break;
            case "rb1stSpeedSli":
                rb1stSpeedSli.value = float.Parse(value);
                break;
            case "rb2stSpeedSli":
                rb2stSpeedSli.value = float.Parse(value);
                break;
        }
    }


    // Function to be called when the OK button is clicked  
    //void OnClick()
    //{
    //    // Invoke the event and pass the loadCapacity value
    //    OnOKButtonClickEvent?.Invoke(loadCapacity.text);
    //}  
}
