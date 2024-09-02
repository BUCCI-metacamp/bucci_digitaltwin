using Best.MQTT;
using Best.MQTT.Packets.Builders;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WI;

namespace Edukit
{
    public class UI_Edukit : MonoBehaviour, ISingle
    {
        public Image image_ConnectState;

        public TextMeshProUGUI text_ConnectState;

        public Panel_Menu panel_Menu;
        public Panel_Option panel_Option;

        public Edukit edukit;
        public Button menu;

        public override void AfterAwake()
        {
            edukit = FindObjectOfType<Edukit>();
            panel_Menu = GetComponentInChildren<Panel_Menu>();
            panel_Option = GetComponentInChildren<Panel_Option>();
            Panel_QuitChecker panel_QuitChecker = GetComponentInChildren<Panel_QuitChecker>();
            Panel_CameraPointer panel_CameraPointer = GetComponentInChildren<Panel_CameraPointer>();

            //Button menu = transform.Find(nameof(menu)).GetComponent<Button>();
            menu.onClick.AddListener(OnClickMenu);

            panel_Menu.onClickOption += () => panel_Option.gameObject.SetActive(true);
            panel_Menu.onClickQuit += () => panel_QuitChecker.gameObject.SetActive(true);

            var mqtt = FindObjectOfType<Mqtt>();
            mqtt.portParsingError += MQTTPortError;
            mqtt.onErrorEvent += MQTTErrorEvent;
            mqtt.onConnectedEvent += MQTTConnectedEvent;
            mqtt.onStateChangeEvent += MQTTStateChengeEvent;
            mqtt.onDisconnectedEvent += MQTTDisconnectedEvent;
            panel_Option.onClickSave += SaveConnectedInfo;
            panel_Option.onClickConnect += mqtt.Connect;


            panel_Menu.gameObject.SetActive(false);
            panel_Option.gameObject.SetActive(false);
            panel_QuitChecker.gameObject.SetActive(false);

           
            panel_CameraPointer.onClickM1 += edukit.CameraFocusing;
            panel_CameraPointer.onClickM2 += edukit.CameraFocusing;
            panel_CameraPointer.onClickM3 += edukit.CameraFocusing;
            panel_CameraPointer.onClickDashboard += edukit.CameraFocusing;
            panel_CameraPointer.onClickColorSensor += edukit.CameraFocusing;
            panel_CameraPointer.onClickVisionSensor += edukit.CameraFocusing;
            panel_CameraPointer.onClickConveyor += edukit.CameraFocusing;
            //panel_CameraPointer.onClickHMI += edukit.CameraFocusing;

            Panel_M1 panel_M1 = FindSingle<Panel_M1>();
            panel_M1.Connect(edukit.m1);
            panel_M1.gameObject.SetActive(false);

            Panel_M2 panel_M2 = FindSingle<Panel_M2>();
            panel_M2.Connect(edukit.m2);
            panel_M2.gameObject.SetActive(false);

            Panel_M3 panel_M3 = FindSingle<Panel_M3>();
            panel_M3.Connect(edukit.m3);
            panel_M3.gameObject.SetActive(false);

            Panel_ColorSensor panel_Color = FindSingle<Panel_ColorSensor>();
            panel_Color.Connect(edukit.colorSensor);
            panel_Color.gameObject.SetActive(false);

            Panel_VisionSensor panel_Vision = FindSingle<Panel_VisionSensor>();
            panel_Vision.Connect(edukit.visionSensor);
            panel_Vision.gameObject.SetActive(false);

            Panel_ETC panel_ETC = FindSingle<Panel_ETC>();
            panel_ETC.Connect(edukit.etc);
            panel_ETC.gameObject.SetActive(false);
            
            Panel_Conveyor panel_Conveyor = FindSingle<Panel_Conveyor>();
            panel_Conveyor.Connect(edukit.conv);
            panel_Conveyor.gameObject.SetActive(false);

            panel_Conveyor.onConveyorSpeedChange += edukit.conv.ChangeConveyorSpeed;
        }

        private void MQTTStateChengeEvent(MQTTClient client, ClientStates states1, ClientStates states2)
        {
            if (image_ConnectState == null)
                return;
            if (text_ConnectState == null)
                return;
            if (states2 == ClientStates.Connected)
            {
                image_ConnectState.color = Color.green;
                text_ConnectState.SetText("CONNECTED");
            }
            else
            {
                image_ConnectState.color = Color.red;
                text_ConnectState.SetText(states2.ToString());
            }
        }

        private void MQTTErrorEvent(MQTTClient client, string arg2)
        {
            if (image_ConnectState == null)
                return;
            if (text_ConnectState == null)
                return;
            image_ConnectState.color = Color.red;
            text_ConnectState.SetText(arg2);
        }

        private void MQTTDisconnectedEvent(MQTTClient client, DisconnectReasonCodes codes, string arg3)
        {
            if (image_ConnectState == null)
                return;
            if (text_ConnectState == null)
                return;
            image_ConnectState.color = Color.red;
            text_ConnectState.SetText(codes.ToString());
        }

        void MQTTPortError()
        {
            image_ConnectState.color = Color.red;
            text_ConnectState.SetText("UNCORRECT PORT");
        }

        void MQTTConnectedEvent(MQTTClient client)
        {
            image_ConnectState.color = Color.green;
            text_ConnectState.text = "CONNECTED";
        }
        void SaveConnectedInfo(string ip, string port, string topic)
        {
            PlayerPrefs.SetString("ip", ip);
            PlayerPrefs.SetString("port", port);
            PlayerPrefs.SetString("topic", topic);
        }

        void OnClickMenu()
        {
            panel_Menu.gameObject.SetActive(!panel_Menu.gameObject.activeSelf);
        }
    }
}