using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class Case2Lamp : CaseSubMachine
    {
        public string GreenLampState;
        public string YellowLampState;
        public string RedLampState;
        public GameObject light_Green;
        public GameObject light_Yellow;
        public GameObject light_Red;
       

        private void Start()
        {
            LEDON(0);            
            GameObject.Find("StartButton").GetComponent<StartButton>().LedChange += LEDON;
            Manage.Instance.LedChange += LEDON;
        }

        public void LEDON(int Lamp)
        {
            switch (Lamp)
            {
                // ���
                case 0:
                    light_Green.gameObject.SetActive(false);
                    light_Yellow.gameObject.SetActive(true);
                    light_Red.gameObject.SetActive(false);
                    break;
                //����
                case 1:
                    light_Green.gameObject.SetActive(true);
                    light_Yellow.gameObject.SetActive(false);
                    light_Red.gameObject.SetActive(false);
                    break;
                //�Ϸ� ����
                case 2:
                    light_Green.gameObject.SetActive(false);
                    light_Yellow.gameObject.SetActive(false);
                    light_Red.gameObject.SetActive(true);
                    break;
            }
        }       
    }
}