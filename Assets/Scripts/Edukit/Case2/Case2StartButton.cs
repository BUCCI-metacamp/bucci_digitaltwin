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
        public List<Case2AMachine> AMachines; // Case2AMachine �迭 ��� ����Ʈ ���

        public List<Case2BMachine> BMachines; // Case2AMachine �迭 ��� ����Ʈ ���
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
            // ��ư Ŭ�� �� ������ ����
            //Debug.Log("Button clicked!");

            // ��� AMachine�� StartRun() �޼��� ȣ��
            foreach (var machine in AMachines)
            {
                machine.StartRun();
            }

            foreach (var machine in BMachines)
            {
                machine.RunStart();
            }
            LampTower.LEDON(1);
            // ��ư ���� Ȯ��
            // Debug.Log("Button interactable: " + myButton.interactable);     
        }
    }
}

