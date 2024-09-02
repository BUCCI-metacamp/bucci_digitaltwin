using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WI;

namespace Factory
{
    public class EdukitCase2 : MonoBehaviour
    {
        public Case2AMachine m1;
        public Case2BMachine m2;
        public Case2CMachine m3;
        public Case2Robot m7;
        
        public CaseConveyor conv;
        public CaseVisionSensor visionSensor;
        public bool virtualMode;
        public float virtualModeSpeed;
        public string dataPath;
        public List<Chip> runningChips = new();

        CaseEdukitDice frontDice;
        Chip sensingChip;
        List<CaseEdukitDice> runningDices = new();
        List<List<CaseEdukitData>> dataList = new();
        Dictionary<string, CaseSubMachine> subMachineFieldTable = new();
        public event Action<CaseSubMachine> onCameraFoucs;

        //public CaseUI_Edukit edukitUI;
        
        void LoadVirtualData()
        {
            var rawdataStream = File.ReadAllText(dataPath);
            var split = rawdataStream.Split('/');

            for (int i = 0; i < split.Length; ++i)
            {
                if (split[i].Length == 0)
                    continue;
                var obj = JsonConvert.DeserializeObject<List<CaseEdukitData>>(split[i]);
                dataList.Add(obj);
            }
        }

        private void Awake() 
        {
            //m1 = GetComponentInChildren<Case2AMachine>();
            //m2 = GetComponentInChildren<Case2BMachine>();
            //m3 = GetComponentInChildren<Case2CMachine>();
            //m7 = GetComponentInChildren<Case2Robot>();
            //etc = GetComponentInChildren<ETC>();
            //conv = GetComponentInChildren<Conveyor>();
            //colorSensor = GetComponentInChildren<ColorSensor>();
            //visionSensor = GetComponentInChildren<VisionSensor>();

            //edukitUI = FindObjectOfType<UI_Edukit>();

            //m1.onCreateChip += runningChips.Add;
            //m1.onCountReset += ResetEdukit;
            //m2.onCreateDice += OnCreateDice;
            //colorSensor.onSensingChip += GetSensingChip;
            //colorSensor.onColorSensorSensing += UpdateSensingChipState;
            //visionSensor.onChangeDiceValue += UpdateFrontDice;
            ////etc.onResetEvent += ResetEdukit;
            ////etc.onGoStopEvent += conv.AnimState;
            //GenerateSubmachineVariableTable();

            //var topic = PlayerPrefs.GetString("topic");
            //FindObjectOfType<Mqtt>().SubscriptionTopic(topic, ReceiveData);
//            FindSingle<UI_Edukit>().panel_Option.onClickConnect += ConnectMQTT;
            //if (virtualMode)
            //{
            //    StartSimulation();
            //}
            //else
            //{
               //ConnectMQTT();
            //}
        }

        void ConnectMQTT()
        {
            var defalutHost = PlayerPrefs.GetString("ip");
            var defalutPort = PlayerPrefs.GetString("port");
            var defalutTopics = PlayerPrefs.GetString("topic").Split(',');

            var mqtt = FindObjectOfType<Mqtt>();
            Debug.Log($" Connect to {defalutHost}:{defalutPort} {defalutTopics[0]}");
            mqtt.Connect(defalutHost, defalutPort, defalutTopics[0]);
            //FindObjectOfType<Panel_Option>().SetDefaultValues(defalutHost, defalutPort, defalutTopics[0]);
        }
        void ResetEdukit()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        //public void StartSimulation()
        //{
        //    LoadVirtualData();
        //    StartCoroutine(VirtualDataUpdate());
        //}

        void UpdateFrontDice(string v)
        {
            if(frontDice ==null)
            {
                //Debug.Log($"Front Dice is null");
                return;
            }

            frontDice.SetValue(int.Parse(v));
        }
        
        void OnCreateDice(CaseEdukitDice dice)
        {
            runningDices.Add(dice);
            frontDice = dice;
            if (sensingChip == null)
            {
                //Debug.Log($"Error:Chip on Dice");
                return;
            }
            //sensingChip.SetDice(frontDice);
        }

        void GetSensingChip(Chip chip)
        {
            //Debug.Log($"Color Sensor Range In");
            sensingChip = chip;
        }

        void UpdateSensingChipState(bool value)
        {
            if (sensingChip == null)
            {
                //Debug.Log($"Sensing Error");
                return;
            }
            if (sensingChip.isSensing)
                return;

            //if(colorSensor.isTrue(colorSensor.ColorSensorSensing))
            //{
            //    sensingChip.isSensing = true;
            //    sensingChip.SetState(EdukitChip.State.Red);
            //    //Debug.Log($"Sensing Red Chip");
            //}
        }
        
        //IEnumerator VirtualDataUpdate()
        //{
        //    for (int i = 0; i < dataList.Count; ++i)
        //    {
        //        var data = dataList[i];
        //        foreach (var d in data)
        //        {
        //            if (subMachineFieldTable.ContainsKey(d.name))
        //            {
        //                subMachineFieldTable[d.name].SetValue(d.name, d.value);
        //            }
        //        }
        //        yield return new WaitForSecondsRealtime(virtualModeSpeed);
        //    }
        //    //Debug.Log($"End");
        //}

        void GenerateSubmachineVariableTable()
        {
            var submachineFields = this.GetType().GetFields().Where(f => f.FieldType.IsSubclassOf(typeof(CaseSubMachine)));
            foreach (var submachineField in submachineFields)
            {
                var submachineRef = submachineField.GetValue(this);
                var stVariables = submachineRef.GetType().GetFields().Where(f => f.FieldType.Equals(typeof(string))).Select(f2 => f2.Name);
                foreach (var n in stVariables)
                {
                    subMachineFieldTable.Add(n, submachineRef as CaseSubMachine);
                }
            }
        }

        public void ReceiveData(List<JsonData> datas)
        {
            foreach (var d in datas)
            {
                if (subMachineFieldTable.ContainsKey(d.name))
                {
                    //Debug.Log($"{d.name} = {d.value}");
                    subMachineFieldTable[d.name].SetValue(d.name, d.value);
                }
            }
        }

        internal void CameraFocusing(CaseSubMachine target)
        {
            var camPos = target.CameraPoint;
            if (target != conv)
                FindSingle<OrbitalController>().MoveTo(camPos, 0.3f);
            else
                FindSingle<OrbitalController>().MoveTo(camPos, 1f);

            target.ActiveDashboard();
            onCameraFoucs?.Invoke(target);
        }
    }
}