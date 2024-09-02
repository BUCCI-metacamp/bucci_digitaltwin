using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Factory
{
    public class Case2M01 : MonoBehaviour
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
        public Manage manage;

        [SerializeField]
        private Chip stackChip;
        [SerializeField]
        private Transform chipSpawnPoint;
        [SerializeField]
        private Transform pusher;
        [SerializeField]
        private CaseConveyor conveyor; // Inspector에서 할당 필요

        public event Action<Chip> onCreateChip;
        
        private Chip sensingChip;
        
        public event Action<int, string, float> StartMoving;

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
                yield return new WaitForSeconds(1f);

                flag = false;
                StartMoving?.Invoke(M01Number, "ON", M01Duration);                
                
                
                yield return new WaitForSeconds(M01Duration); 
                
                //다시 들어간다.
                StartMoving?.Invoke(M01Number, "OFF", M01Duration);
                
                yield return new WaitForSeconds(M01Time);
                
                flag = true;
            }

            void CreateChip()
            {
                // 스폰 포인트에서 칩 생성
                //var sensingChip = Instantiate(chipSpawnPoint).GetComponent<Chip>();
                sensingChip = Instantiate(stackChip, chipSpawnPoint.position, chipSpawnPoint.rotation);
                sensingChip.gameObject.SetActive(true);
                onCreateChip?.Invoke(sensingChip);
                // 칩에 태그 설정
                sensingChip.gameObject.tag = "Chip";
                sensingChip.SetMachineNumber(M01Number, 1);
            }
        }
    }
}
