using System.Collections.Generic;
using UnityEngine;

namespace Edukit
{
    public class Conveyor : SubMachine
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

  

        public bool isAnim;

        private float maxSpeed;
        public float ConvSpeedRatio=0; //0과 1사이의 값.
        private float convSpeed;


        Vector3 conveyorDir
        {
            get
            {
                return (endPoint.position - startPoint.position).normalized;
            }
        }

        private new void Awake()
        {
            base.Awake();
            

            maxSpeed = 0.07395005f;
            convSpeed = maxSpeed / (-2.5f * (ConvSpeedRatio / 100) + 3.5f);            
            mr = GetComponent<MeshRenderer>();


        }

        private void Update()
        {
            Run();
        }

        void Run()
        {
            float temp = this.speed;
            if (realSpeed)
            {
                float currentDistance = 1f * 0.166f;
                currentMoveLength = currentDistance - prevMoveLength;
                prevMoveLength = currentDistance;
                var speed = (currentMoveLength / 1000f);
                this.speed = speed;
            }
            else
            {
                this.speed /= 1000f;
            }

            UpdateTextureOffset();
            Running(convSpeed);
            //Running(speed);
            //conveyorSpeed = speed * 3.2f;
            //var u = mr.material.GetTextureOffset("_BaseMap");
            //u.x -= conveyorSpeed;
            //mr.material.SetTextureOffset("_BaseMap", u);
            //this.speed = temp;
        }

        private void UpdateTextureOffset()
        {
            var u = mr.material.GetTextureOffset("_BaseMap");
            u.x -= convSpeed / 10; // 건너 뛴다.
            mr.material.SetTextureOffset("_BaseMap", u);
        }

        public float prevMoveLength;
        public float currentMoveLength;
        public float conveyorSpeed;
        protected override void VariableUpdateEvent(string variableName, string variableValue)
        {
            if (!isAnim)
                return;
            switch (variableName)
            {
                case nameof(TotalDistance):

                    float temp = this.speed;
                    if (realSpeed)
                    {
                        float currentDistance = float.Parse(variableValue) * 0.166f;
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
                    u.x -= conveyorSpeed;
                    mr.material.SetTextureOffset("_BaseMap", u);
                    this.speed = temp;
                    break;
            }

        }

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
                    pos += dir * 0.018f * Time.deltaTime;
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

        List<EdukitChip> onRailChipList = new();
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var ec))
                return;
            onRailChipList.Add(ec);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var ec))
                return;
            onRailChipList.Remove(ec);
        }
    }
}