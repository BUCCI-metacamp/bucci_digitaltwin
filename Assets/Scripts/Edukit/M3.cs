
using UnityEngine;
namespace Edukit
{
    public class M3 : SubMachine
    {
        public string No3ChipArrival;
        public string No3PowerState;
        public string No3Count;
        public string No3Motor1Position;
        public string No3Motor2Position;
        public string No3Gripper;
        public string No3Motor1Action;
        public string No3Motor2Action;

        public EdukitChip inGrabChip;
        public Transform moter1;
        public Transform moter2_1;
        public Transform moter2_2;
        public Transform grabPoint;
        Animator animator;

        private new void Awake()
        {
            base.Awake();
            originMotor1 = moter1.localPosition.y;
            animator = GetComponent<Animator>();
            m21rot = moter2_1.localRotation.eulerAngles.y;
            m22rot = moter2_2.localRotation.eulerAngles.z;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var c))
                return;
            inGrabChip = c;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<EdukitChip>(out var c))
                return;
            inGrabChip = null;
        }

        float originMotor1;
        readonly float motor1ValueConvert = 98000;
        readonly float motor2ValueConvert = 226667;
        float m22rot;
        float m21rot;
        protected override void VariableUpdateEvent(string vn, string vv)
        {
            switch (vn)
            {
                case nameof(No3Motor1Position):
                    var rawPos = float.Parse(vv) / motor1ValueConvert * 0.01f;
                    //Debug.Log($"No1Motor1Pos:{rawPos}");
                    var pos = moter1.localPosition;
                    pos.y = rawPos + originMotor1;
                    moter1.localPosition = pos;
                    break;

                case nameof(No3Motor2Position):
                    var rawRot = float.Parse(vv)/ motor2ValueConvert;
                    var rawRot2 = rawRot;
                    
                    var rot = moter2_1.localRotation;
                    rot.eulerAngles = new Vector3(rot.eulerAngles.x, m21rot+rawRot, rot.eulerAngles.z);
                    moter2_1.localRotation = rot;
                    
                    var rot2 = moter2_2.localRotation;
                    rot2.eulerAngles = new Vector3(rot2.eulerAngles.x, rot2.eulerAngles.y, m22rot - rawRot2);
                    moter2_2.localRotation = rot2;
                    //Debug.Log($"Rot:{rawRot}");
                    break;
            }
        }

        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch(variableName)
            {
                case nameof(No3Gripper):
                    if (isTrue(variableValue))
                    {
                        //Debug.Log($"No3ChipGrab_{ETC.playTick}");
                        //alreadyDrop = false;
                        inGrabChip.transform.position = grabPoint.position;
                        inGrabChip?.OnGrab(grabPoint);
                        animator.Play("Grab");
                    }
                    else
                    {
                        //if (!alreadyDrop)
                        //{
                        //    alreadyDrop = true;
                        if (inGrabChip != null)
                        {
                            inGrabChip.HandOff();
                            inGrabChip = null;
                        }
                        //Debug.Log($"No3ChipDrop_{ETC.playTick}");
                        animator.Play("Drop");
                        //}
                    }
                    break;
            }
        }
    }
}