using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Factory
{
    public class Case2Conveyor : CaseSubMachine
    {
        public string TotalDistance;
        public string RevPerSecond;
        public string Velocity;

        public bool realSpeed;
        public float speed;
        public float fullSpeed;
        [SerializeField]
        MeshRenderer mr;
        public Transform startPoint;
        public Transform endPoint;

        public Case2AMachine aMachine;
        public Case2BMachine bMachine;
        public Case2CMachine cMachine;
        public GameObject MachineObject;

        public int MachineType;


        public bool isAnim;


        public bool Test_Checkl = false;

        private float maxSpeed;
        public float ConvSpeedRatio; //0과 1사이의 값.
        private float convSpeed;

        Vector3 conveyorDir
        {
            get
            {
                return (endPoint.position - startPoint.position).normalized;
            }
        }
        private void Start()
        {
           if(MachineType == 1)
            {
                aMachine = MachineObject.GetComponent<Case2AMachine>();
            }
           else if(MachineType == 2)
            {
                bMachine = MachineObject.GetComponent<Case2BMachine>();
            }
           else if(MachineType == 3)
            {
                cMachine = MachineObject.GetComponent<Case2CMachine>();
            }
            maxSpeed = 0.07395005f;
            convSpeed = maxSpeed / (-2.5f * (ConvSpeedRatio / 100) + 3.5f);
            convSpeed = convSpeed * 2;
        }
        private new void Awake()
        {
            base.Awake();
            mr = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            UpdateTextureOffset();
            Running(convSpeed);

            //if (MachineType == 1)
            //{              
            //    if (!aMachine.Test_Check)
            //        TestRun();
            //}
            //else if (MachineType == 2)
            //{
            //    if (!bMachine.Test_Check)
            //        TestRun();
            //}
            //else if (MachineType == 3)
            //{                
            //    if (!cMachine.Test_Check)
            //        TestRun();
            //}
            //else if(MachineType == 100)
            //{
            //    TestRun();
            //}
            //else
            //{
            //    TestRun();
            //}

        }
        private void UpdateTextureOffset()
        {
            var u = mr.material.GetTextureOffset("_BaseMap");
            u.y -= convSpeed / 10; // 건너 뛴다.
            mr.material.SetTextureOffset("_BaseMap", u);
        }

        void TestRun()
        {
            if (MachineType == 100)
            {
                int a = 100;
                if(a == 100)
                {
                    a = 101;
                }
            }

            if (MachineType == 101)
            {
                int a = 100;
                if (a == 100)
                {
                    a = 101;
                }
            }
            float temp = this.speed;
            if (realSpeed)
            {
                float currentDistance = 10 * 0.166f;
                currentMoveLength = currentDistance - prevMoveLength;
                prevMoveLength = currentDistance;
                var speed = (currentMoveLength / 1000f);
                this.speed = speed;
            }
            else
            {
                this.speed /= 1000f;
            }

            Running(speed);
            conveyorSpeed = speed * 3.2f;
            var u = mr.material.GetTextureOffset("_BaseMap");
            u.y -= conveyorSpeed;
            mr.material.SetTextureOffset("_BaseMap", u);
            this.speed = temp;
        }


        public float prevMoveLength;
        public float currentMoveLength;
        public float conveyorSpeed;
        

        void Running(float speed)
        {
            foreach (var c in onRailChipList)
            {
                if (c.onHand)
                    continue;
                if (c != null)
                {
                    var dir = conveyorDir;
                    var pos = c.transform.position;
                    pos += dir * speed * Time.deltaTime;
                    c.transform.position = pos;
                }
            }

        }

        public void AnimState(bool value)
        {
            isAnim = value;
        }
        public void ChangeConveyorSpeed(float value)
        {
            speed = fullSpeed * (value);
        }

        public List<Chip> onRailChipList = new();
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("감지가 되었습니다.");
            if (MachineType == 100)
            {
                int b = 100;
                if(b == 100)
                {
                    b = 101;
                }
            }
            if (!other.TryGetComponent<Chip>(out var ec))
                return;
            onRailChipList.Add(ec);
        }
        public void ExitTrigger(Collider other)
        {
            OnTriggerExit(other);
        }
        private void OnTriggerExit(Collider other)
        {
            //Test_Checkl = false;
            if (!other.TryGetComponent<Chip>(out var ec))
                return;
            onRailChipList.Remove(ec);
        }
    }
}