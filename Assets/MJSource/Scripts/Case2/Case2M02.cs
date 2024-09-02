using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Factory
{
    public class Case2M02 : MonoBehaviour
    {

        // 기기 관리
        public float M2SpeedRatio;
        private float M02Duration;
        public float M02Time;
        public event Action<int, string, float> StartMoving;
        public event Action<float> convStop;
        public int M02Number;

        // 내용물 관리
        //public int diceCount; //투입 완료 한 주사위의 개수
        //public int diceCapacity; //투입 가능한 주사위의 총량
        public bool M2PowerState;

        public bool No2PusherState;
        //public bool No2CubeFull;
        //public bool No2CubeEmpty;
        //public event Action<bool> inPutting;

        public Chip sensingChip;
        public GameObject unknownCube;
        //private bool flag;

        //투입 내용물
        private Chip.InsideState contentType;
        public GameObject[] FruitCube;

        private bool Nextchip;
        private void Start()
        {
            M02Duration = -0.05f * M2SpeedRatio + 6f;
            Nextchip = true;
            GameObject.Find("StartButton").GetComponent<StartButton>().onStartButton += No2Power;

            ControlSettingVIewCase2 settingView = GameObject.Find("SettingView").GetComponent<ControlSettingVIewCase2>();

            settingView.BMachineDuratio += SettingClickedDuration; //공정속도

            settingView.Item += SetInsideState; //품목
            settingView.volume += SettingClickedTime; //용량

            foreach (GameObject cube in FruitCube)
            {
                if (cube == null)
                    continue;
                cube.SetActive(false);
            }
        }

        void SettingClickedDuration(float loadCapacity)//공정속도
        {
            M2SpeedRatio = loadCapacity;
            M02Duration = -0.05f * M2SpeedRatio + 6f;
            Debug.Log("M02Duration updated to: " + M02Duration);
        }

        void SettingClickedTime(float loadCapacity)//공정속도
        {
            M02Time = loadCapacity;
            Debug.Log("M02Time updated to: " + M02Time);
        }

        IEnumerator WaitAppleCoroutine(float appletime)
        {
            Chip.InsideState Content = GetContent();
            Debug.Log(Content);
            for (int i = 0; i < 4; i++)
            {
                FruitCube[((int)Content)].SetActive(true); 
                FruitCube[((int)Content)].SetActive(false);
                yield return new WaitForSecondsRealtime(appletime/4);
            }
            FruitCube[((int)Content)].SetActive(false);
            StartMoving?.Invoke(M02Number, "OFF", M02Duration);
        }
        
        IEnumerator SettingAppleCoroutine(float time)
        {
            Chip.InsideState Content = GetContent();
            yield return new WaitForSecondsRealtime(time);
            FruitCube[((int)Content)].SetActive(true);
            StartMoving?.Invoke(M02Number, "ON", M02Duration);
            
            yield return new WaitForSecondsRealtime(M02Duration);
            Nextchip = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Chip>(out var c) || !Nextchip || c.GetMachineNumber(2) != 0 || c.GetMachineNumber(1)!=M02Number ) return;
            sensingChip = c;
            Nextchip = false;
            convStop?.Invoke(M02Time); //컨베이어 벨트를 정지한다 
            c.appleState(GetContent(), M02Time); //내용물을 투입한다.
            c.SetMachineNumber(M02Number, 2); //기기 번호 입력
            StartCoroutine(WaitAppleCoroutine(M02Time));
            StartCoroutine(SettingAppleCoroutine(M02Time+M02Duration));
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Chip>(out var c))
            {
                sensingChip = null;
            }
        }
        private void No2Power(bool state)
        {
            StartMoving?.Invoke(M02Number, "ON", M02Duration);
        }

        public void SetInsideState(Chip.InsideState newState)
        {
            contentType = newState;
        }


        public Chip.InsideState GetContent()
        {
            return contentType;
        }

    }
}
