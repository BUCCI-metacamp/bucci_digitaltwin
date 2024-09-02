using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace Factory
{
    public class CaseConveyor : MonoBehaviour
    {
        
        [SerializeField]
        private MeshRenderer mr;
        public Transform startPoint;
        public Transform endPoint;

        public M02 m2;
        public M03 m3;

        public M02[] M02List;
        public M03[] M03List;

        private bool convPowerState;
        
        public List<Chip> onRailChipList = new List<Chip>();
        private float maxSpeed;
        public float ConvSpeedRatio; //0과 1사이의 값.
        private float convSpeed;

        


       Vector3 conveyorDir { get { return (endPoint.position - startPoint.position).normalized; } }

        private void Awake()
        {
            GameObject.Find("StartButton").GetComponent<StartButton>().onStartButton += ConvPowerState;

            foreach(var M02v in M02List)
            {
                M02v.convStop += convStop;
            }

            foreach (var M03v in M03List)
            {
                M03v.convStop += convStop;
            }

            //m2.convStop += convStop;
            //m3.convStop += convStop;
            
            maxSpeed = 0.07395005f;
            convSpeed = maxSpeed / (-2.5f * (ConvSpeedRatio / 100) + 3.5f);
            convPowerState = false;
            mr = GetComponent<MeshRenderer>();
        }
        private void Start()
        {
            //Setting
            ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
            settingView.ConSpeed += OnOKButtonClickedSpeed; //컨베이어속도

            //Setting
            AISetting AISetting = GameObject.Find("AIView").GetComponent<AISetting>();
            AISetting.ConSpeed += OnOKButtonClickedSpeed; //컨베이어속도

        }

        void OnOKButtonClickedSpeed(float loadCapacity) // 가공시간 
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            ConvSpeedRatio = loadCapacity;
            maxSpeed = 0.07395005f;
            convSpeed = maxSpeed / (-2.5f * (ConvSpeedRatio / 100) + 3.5f);
            Debug.Log("M03Time updated to: " + convSpeed);
        }

        private void ConvPowerState(bool state)
        {
            convPowerState = state;
        }
        

        void Update()
        {
            if (convPowerState)
            {
                UpdateTextureOffset();
                Running(convSpeed);
            }
        }

        private void UpdateTextureOffset()
        {
            var u = mr.material.GetTextureOffset("_BaseMap");
            u.y -= convSpeed/10; // 건너 뛴다.
            mr.material.SetTextureOffset("_BaseMap", u);
        }

        void Running(float speed)
        {
            foreach (var c in onRailChipList)  
            {
                if (c.onHand)
                    continue;
                if(c != null)
                {
                    var dir = conveyorDir;
                    var pos = c.transform.position;
                    pos += dir * speed * Time.deltaTime;
                    c.transform.position = pos;
                }               
            }
        }
        
        
        // 컨베이어 작동 중지: 지정 시간 동안 컨베이어가 작동을 멈춘다.
        private void convStop(float duration)
        {
            StartCoroutine(ConveyorStopEvent(duration));
        }
        private IEnumerator ConveyorStopEvent(float duration) 
        {
            convPowerState = false;
            yield return new WaitForSeconds(duration);
            convPowerState = true;
        }
        
        
        // 푸셔 힘 구현
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Chip>(out var chip))
                return;
            StartCoroutine(AddChipToRailListWithDelay(chip, 0.9f));
        }
        private IEnumerator AddChipToRailListWithDelay(Chip chip, float delay)
        {
            yield return new WaitForSeconds(delay); 
            onRailChipList.Add(chip);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Chip>(out var chip))
                return;
            onRailChipList.Remove(chip);
        }
    }
}
