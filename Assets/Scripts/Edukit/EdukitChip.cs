using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edukit
{
    public class EdukitChip : MonoBehaviour
    {
        public enum State
        {
            Unknown,
            Red,
            White,
        }

        public enum Fruit
        {
            Unknown,
            Apple,
            Banana,
            Orange,
            Strawberry,
        }


        public State currentState;

        public Fruit setFruit;

        public MeshRenderer mr;
        public Material mat_Red;
        public Material mat_White;
        public Material mat_Unknown;
        public Material mat_Yellow;
        public Material mat_Orange;
        public Material mat_Pink;
        public Transform dicePos;
        internal bool isSensing;

        public bool Chip3 = false;


        public int serialNumber; // 고유 번호를 저장할 변수
        public int serialMachineNumber; // 고유 번호를 저장할 변수

        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
            isSensing = false;
        }

        public void SetState(State s)
        {
            switch (s)
            {
                case State.Unknown:
                    mr.material = mat_Unknown;
                    break;
                case State.Red:
                    mr.material = mat_Red;
                    break;
                case State.White:
                    mr.material = mat_White;
                    break;
            }
            currentState = s;
        }

        public void SetFruit(Fruit s)
        {
            switch (s)
            {
                case Fruit.Unknown:
                    mr.material = mat_Unknown;
                    break;
                case Fruit.Apple:
                    mr.material = mat_Red;
                    break;
                case Fruit.Banana:
                    mr.material = mat_Yellow;
                    break;
                case Fruit.Orange:
                    mr.material = mat_Orange;
                    break;
                case Fruit.Strawberry:
                    mr.material = mat_Pink;
                    break;
            }
            setFruit = s;
        }

        public void SetSerialNumber(int machine,int number)
        {

            serialMachineNumber = machine;
            serialNumber = number;
            string str = string.Format("ChipNumber :::: {0},{1}", machine, number);
            Debug.Log(str);
        }


        public bool onHand;
        public void OnGrab(Transform grabPoint)
        {
            onHand = true;
            transform.position = grabPoint.position;
            transform.SetParent(grabPoint);
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic= true;
        }

        internal void SetDice(EdukitDice frontDice)
        {
            frontDice.transform.SetParent(transform);
            frontDice.transform.position = dicePos.position;
        }

        internal void HandOff()
        {
            //onHand = false;
            transform.SetParent(null);
            transform.position = transform.position;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
          
        }

        
    }
}