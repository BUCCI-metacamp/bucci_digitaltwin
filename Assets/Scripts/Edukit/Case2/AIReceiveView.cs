using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System;
using Factory;
using WI;


public class AIReceiveView : BaseMono<AIReceiveView>
{

    public CanvasController panelActive;
    public AISetting AISetting;


    [Header("MainControl_Left")]
    public TextMeshProUGUI AIcost; // �� ���귮    



    [Header("MainControl_Right")]
    public TMP_Dropdown mainContent; //ǰ��
    public TMP_Dropdown mainCapacity; //�뷮
    public TextMeshProUGUI cost; //����



    [Header("ConControl")]
    public TextMeshProUGUI conSpeedTxt; //�����̾� �ӵ� txt
   




    [Header("AMControl")]
    public TextMeshProUGUI amSpeedTxt; //������� �ӵ� �ؽ�Ʈ
    public TextMeshProUGUI amIntervalTxt;//������� Ĩ���� �ؽ�Ʈ
    




    [Header("BMControl")]
    public TextMeshProUGUI bmSpeedTxt; //���Ա� �ӵ� �ؽ�Ʈ
    public TextMeshProUGUI bmIntervalTxt;//���Ա� ���� 
    



    [Header("CMControl")]
    public TextMeshProUGUI cmSpeedTxt; //������ �ӵ� �ؽ�Ʈ
    public TextMeshProUGUI cmManufacturingTxt; //������ ���� �ؽ�Ʈ
    




    [Header("Line")]
    public TextMeshProUGUI rb1stSpeedTxt; // 1�� �ӵ� �ؽ�Ʈ
    public TextMeshProUGUI rb2stSpeedTxt; // 2�� �ӵ� �ؽ�Ʈ
    public TextMeshProUGUI rb3stSpeedTxt; // 2�� �ӵ� �ؽ�Ʈ
    
    

    [Header("MainButton")]
    public Button OKButton;
    public Button CancleButton;


    public int targetNumber;
    public GameObject Buttons;
    public GameObject headerBaer;


    void Start()
    {
        CancleButton.onClick.AddListener(OnCancleButtonClick);
        OKButton.onClick.AddListener(OnOKButtonClick);
        // ManageCase2.Instance.AiVIewer += CreateTable;
        CreateTable();
    }
    
    void CreateTable()
    {

        if (targetNumber == 0)
        {
            ApplySetting("AICost", MainValue.Instance.aiViwer[targetNumber].targetValue+"�� (1��)");
        }
        else if (targetNumber == 1)
        {
            ApplySetting("AICost", MainValue.Instance.aiViwer[targetNumber].targetValue+"��");
        }
        else if (targetNumber == 2)
        {
            ApplySetting("AICost", MainValue.Instance.aiViwer[targetNumber].targetValue+"%");
        }
          
        ApplySetting("conSpeedTxt", MainValue.Instance.aiViwer[targetNumber].ConvSpeedRatio);          
        ApplySetting("amSpeedTxt", MainValue.Instance.aiViwer[targetNumber].M01Duration);
        ApplySetting("amIntervalTxt", MainValue.Instance.aiViwer[targetNumber].M01Time);
        ApplySetting("bmSpeedTxt", MainValue.Instance.aiViwer[targetNumber].M02Duration);
        ApplySetting("bmIntervalTxt", MainValue.Instance.aiViwer[targetNumber].M02Time);
        ApplySetting("cmSpeedTxt", MainValue.Instance.aiViwer[targetNumber].M03Duration);
        ApplySetting("cmManufacturingTxt", MainValue.Instance.aiViwer[targetNumber].M03Time);
        ApplySetting("rb1stSpeedTxt", MainValue.Instance.aiViwer[targetNumber].Line01GoodProductRatio);
        ApplySetting("rb2stSpeedTxt", MainValue.Instance.aiViwer[targetNumber].Line02GoodProductRatio);
        ApplySetting("rb3stSpeedTxt", MainValue.Instance.aiViwer[targetNumber].Line03GoodProductRatio);

        
        

    }
    void OnCancleButtonClick()
    {
        HideCanvas();
    }

    void OnOKButtonClick()
    {


        AISetting.AIOKButton(targetNumber);
        HideCanvas();
    }
       
    void HideCanvas()
    {
            gameObject.SetActive(false);
            Buttons.SetActive(false);
            headerBaer.SetActive(true);
       
    }
    void ApplySetting(string id, string value)
    {
        switch (id)
        {
            case "AICost":
                AIcost.text = value;
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
            case "bmIntervalTxt":
                bmIntervalTxt.text = value;
                break;
            case "cmSpeedTxt":
                cmSpeedTxt.text = value;
                break;
            case "cmManufacturingTxt":
                cmManufacturingTxt.text = value;
                break;
            case "rb1stSpeedTxt":
                rb1stSpeedTxt.text = value + "%";
                break;
            case "rb2stSpeedTxt":
                rb2stSpeedTxt.text = value + "%";
                break;
            case "rb3stSpeedTxt":
                rb3stSpeedTxt.text = value + "%";
                break;
            case "mainContent":
                mainContent.value = int.Parse(value);
                break;
            case "mainCapacity":
                mainCapacity.value = int.Parse(value);
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
