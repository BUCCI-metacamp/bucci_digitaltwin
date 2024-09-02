using System.Collections.Generic;
using UnityEngine;

namespace Edukit
{
    public class TestConveyor : MonoBehaviour
    {
        public float TotalDistance;
        public string RevPerSecond;
        public string Velocity;

        public bool realSpeed;
        public float speed;
        public float fullSpeed;
        [SerializeField]
        private MeshRenderer mr;
        public Transform startPoint;
        public Transform endPoint;

        //public bool isAnim;

        Vector3 conveyorDir
        {
            get
            {
                return (endPoint.position - startPoint.position).normalized;
            }
        }

        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
        }

        public float prevMoveLength;
        public float currentMoveLength;
        public float conveyorSpeed;

        void Update()
        {
            //if (!isAnim)
            //return;

            UpdateTextureOffset();
            Running(speed);
        }

        private void UpdateTextureOffset()
        {
            var tempSpeed = this.speed;
            if (realSpeed)
            {
                float currentDistance = TotalDistance * 0.166f;
                currentMoveLength = currentDistance - prevMoveLength;
                prevMoveLength = currentDistance;
                tempSpeed = (currentMoveLength / 1000f);
            }
            else
            {
                tempSpeed /= 1000f;
            }

            conveyorSpeed = tempSpeed * 3.2f;
            var u = mr.material.GetTextureOffset("_BaseMap");
            u.x -= conveyorSpeed;
            mr.material.SetTextureOffset("_BaseMap", u);
        }

        void Running(float speed)
        {
            foreach (var c in onRailChipList)
            {
                if (c.onHand)
                    continue;
                var dir = conveyorDir;

                var pos = c.transform.position;
                pos += dir * conveyorSpeed * Time.deltaTime * 6.4f; // speed 적용 및 시간에 따른 이동
                c.transform.position = pos;
            }
        }

        /*public void AnimState(bool value)
        {
            isAnim = value;
        }*/

        public void ChangeConveyorSpeed(float value)
        {
            speed = fullSpeed * value;
        }

        List<EdukitChip> onRailChipList = new List<EdukitChip>();


        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var chip))
                return;
            onRailChipList.Add(chip);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var chip))
                return;
            onRailChipList.Remove(chip);
        }
    }
}
