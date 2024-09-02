using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Factory
{
    public class M01 : MonoBehaviour
    {
        public int chipCount;
        public int chipCapacity = 10; // 최대 칩 개수
        public float M1SpeedRatio;
        private float M01Duration; // 칩 나오는 시간
        public float M01Time; // 칩 생성 
        public int M01Number; // 몇 번째 기기인지.
        
        public string No1ChipEmpty;
        public string No1Push;
        public bool M01PowerState;
        //public bool M01PusherState;
        public string No1Count;
        public string No1ChipFull;

        private bool flag;
        public ManageCase2 manage;

        [SerializeField]
        private Chip stackChip;
        public Transform parentTransform; // 생성될 부모 Transform
        [SerializeField]
        private Transform chipSpawnPoint;
        [SerializeField]
        private Transform pusher;
        [SerializeField]
        private CaseConveyor conveyor; // Inspector에서 할당 필요

        public event Action<Chip> onCreateChip;
        
        private Chip sensingChip;
        
        public event Action<int, string, float> StartMoving;


        public event Action<string, string> SettingStart; //SettingView 초기입력
        private void flagState()
        {
            flag = false;
        }


        private void Start()
        {
            chipCount = 0;
            //M01Duration = -2.0f * M1SpeedRatio + 2.8f;
            var startButton = GameObject.Find("StartButton")?.GetComponent<StartButton>();
            if (startButton != null)
            {
                startButton.onStartButton += StartM01;
            }
            if (pusher!= null)
            {
                pusher.gameObject.tag = "Pusher";
            }

            manage.stopCreateChip += flagState;
            M01Duration = -0.05f * M1SpeedRatio + 6f;

            ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
            settingView.AMachineTime += OnOKButtonClickedTime; //칩간격
            settingView.AMachineDuration += OnOKButtonClickedDuration; //공정속도

            //Setting
            AISetting AISetting = GameObject.Find("AIView").GetComponent<AISetting>();
            AISetting.AMachineTime += OnOKButtonClickedTime; //칩간격
            AISetting.AMachineDuration += OnOKButtonClickedDuration; //공정속도

            SecenChange RestConnect = GameObject.Find("SecenChange").GetComponent<SecenChange>();
            RestConnect.ResetStart += RestStart;

            string strSpeed = string.Format("{0}", M1SpeedRatio);//공정속도 초기입력
            SettingStart?.Invoke("amSpeedTxt", strSpeed); //공정속도 초기입력
        }

        void OnOKButtonClickedTime(float loadCapacity) //칩간격
        {           
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            M01Time = loadCapacity;
            Debug.Log("M01Time updated to: " + M01Time);
        }

        void OnOKButtonClickedDuration(float loadCapacity)//공정속도
        {
            M1SpeedRatio = loadCapacity;
            M01Duration = -0.05f * M1SpeedRatio + 6f;
            Debug.Log("M01Duration updated to: " + M01Duration);                
        }

        void StartM01(bool state)
        {
            M01PowerState = state;
            if (state)
            {
                flag = true;
                StartCoroutine(StartEventCycleAfterDelay());
            }
        }
        
        IEnumerator StartEventCycleAfterDelay()
        {
            yield return new WaitForSeconds(3.0f);

            while (true)
            {
                if (chipCount > chipCapacity) break;
                //푸셔가 칩을 생성하고 민다.
                
                if (flag == false) break;
                CreateChip();

                // 1초 대기
               // yield return new WaitForSeconds(1f);

                flag = false;
                StartMoving?.Invoke(1, "ON", M01Duration);         
               
                
                yield return new WaitForSeconds(M01Duration); 
                
                //다시 들어간다.
                StartMoving?.Invoke(1, "OFF", M01Duration);
                
                yield return new WaitForSeconds(M01Time);
                
                flag = true;
            }
        }

        void CreateChip()
        {
            // 스폰 포인트에서 칩 생성
            //var sensingChip = Instantiate(chipSpawnPoint).GetComponent<Chip>();
            sensingChip = Instantiate(stackChip, chipSpawnPoint.position, chipSpawnPoint.rotation, parentTransform);
            sensingChip.gameObject.SetActive(true);
            onCreateChip?.Invoke(sensingChip);
            // 칩에 태그 설정
            sensingChip.gameObject.tag = "Chip";
            sensingChip.SetMachineNumber(M01Number, 1);
        }

        void RestStart(bool reset)
        {
            flag = reset;
        }
    }
}
