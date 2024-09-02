using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Best.MQTT;
using UnityEngine.UI;

namespace Factory
{
    public class Case2StartButton : MonoBehaviour
    {
        public Case2MainControl mainControl;
        public Button myButton;
        public event Action<int, string> onStartButtonChanged; //int: tagId, string: value
        public List<Case2AMachine> AMachines; // Case2AMachine 배열 대신 리스트 사용

        public List<Case2BMachine> BMachines; // Case2AMachine 배열 대신 리스트 사용
        public Case2AMachineNodeMove aMachineNode;
        public Case2CMachine CMachine;
        public Case2Lamp LampTower;

        // Start is called before the first frame update
        void Start()
        {

            if (myButton != null)
            {
                myButton.onClick.AddListener(OnButtonClick);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnButtonClick()
        {
            // 버튼 클릭 시 수행할 동작
            //Debug.Log("Button clicked!");

            // 모든 AMachine의 StartRun() 메서드 호출
            foreach (var machine in AMachines)
            {
                machine.StartRun();
            }

            foreach (var machine in BMachines)
            {
                machine.RunStart();
            }
            LampTower.LEDON(1);
            // 버튼 상태 확인
            // Debug.Log("Button interactable: " + myButton.interactable);     
        }
    }
}

