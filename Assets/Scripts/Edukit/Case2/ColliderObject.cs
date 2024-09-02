using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Factory
{    
    public class ColliderObject : MonoBehaviour
    {
        public int machineNumber;
        public Case2BMachine BMachine; // 싱글톤 인스턴스
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
                        // 감지된 오브젝트 정보를 ScriptObject에 전달
                        BMachineNode.DetectObject(other, machineNumber);
                    }
                }
                else if(machineNumber == 2)
                {
                    if (other.name == "ChipSpawnPoint2(Clone)")
                    {
                        Debug.Log("ColliderObject: Detectable object entered.");
                        // 감지된 오브젝트 정보를 ScriptObject에 전달
                        BMachineNode.DetectObject(other, machineNumber);
                    }
                }
                else if(machineNumber == 3)
                {
                    if (other.name == "ChipSpawnPoint3(Clone)")
                    {
                        Debug.Log("ColliderObject: Detectable object entered.");
                        // 감지된 오브젝트 정보를 ScriptObject에 전달
                        BMachine.DetectObject(other);
                    }
                }
               
                    
            }
                
        }

        private void OnTriggerExit(Collider other)
        {
            BMachine.ExitObject();// 이동이 발생했음을 표시
        }
    }
}
   
