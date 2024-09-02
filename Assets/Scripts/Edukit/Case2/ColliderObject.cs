using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Factory
{    
    public class ColliderObject : MonoBehaviour
    {
        public int machineNumber;
        public Case2BMachine BMachine; // �̱��� �ν��Ͻ�
        public Case2BMachineNode BMachineNode;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Chip>(out var ec))
            {
                if(machineNumber == 1)
                {
                    if (other.name == "ChipSpawnPoint1(Clone)")
                    {
                        Debug.Log("ColliderObject: Detectable object entered.");
                        // ������ ������Ʈ ������ ScriptObject�� ����
                        BMachineNode.DetectObject(other, machineNumber);
                    }
                }
                else if(machineNumber == 2)
                {
                    if (other.name == "ChipSpawnPoint2(Clone)")
                    {
                        Debug.Log("ColliderObject: Detectable object entered.");
                        // ������ ������Ʈ ������ ScriptObject�� ����
                        BMachineNode.DetectObject(other, machineNumber);
                    }
                }
                else if(machineNumber == 3)
                {
                    if (other.name == "ChipSpawnPoint3(Clone)")
                    {
                        Debug.Log("ColliderObject: Detectable object entered.");
                        // ������ ������Ʈ ������ ScriptObject�� ����
                        BMachine.DetectObject(other);
                    }
                }
               
                    
            }
                
        }

        private void OnTriggerExit(Collider other)
        {
            BMachine.ExitObject();// �̵��� �߻������� ǥ��
        }
    }
}
   
