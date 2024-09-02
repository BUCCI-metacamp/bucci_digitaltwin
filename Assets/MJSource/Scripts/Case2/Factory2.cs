using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;

namespace Factory
{
    public class Factory2 : MonoBehaviour
    {
        public Case2M02 M2F1;
        public Case2M02 M2F2;
        public Case2M02 M2F3;
        public CaseConveyor conv;
        
        public Transform m2Pusher1;
        public Transform m2Pusher2;
        public Transform m2Pusher3;

        private Vector3 m2origin1;
        private Vector3 m2origin2;
        private Vector3 m2origin3;
        
        //public List<Chip> runningChips = new();
        public ConcurrentBag<Chip> runningChips = new ConcurrentBag<Chip>();
        public event Action<int> PusherMoved;
        
        private void Awake() 
        {
            conv = GetComponentInChildren<CaseConveyor>();
            M2F1.StartMoving += StartMoving;
            M2F2.StartMoving += StartMoving;
            M2F3.StartMoving += StartMoving;

            m2origin1 = m2Pusher1.position;
            m2origin2 = m2Pusher2.position;
            m2origin3 = m2Pusher3.position;
        }
        
        
        
        // 오브젝트를 목표 위치로 이동시키는 코루틴
        // 기기 제어
        private IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration, string power, int mNum)
        {
            Vector3 start = obj.position;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                obj.position = Vector3.Lerp(start, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.position = target;
        }

        // 이동을 시작하는 메서드
        public void StartMoving(int mNum, string power, float duration)
        {
            switch (mNum)
            {
                case 1:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m2origin1.x + 0.07f, m2Pusher1.position.y, m2Pusher1.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher1, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m2origin1.x, m2Pusher1.position.y, m2Pusher1.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher1, target, duration, power, mNum));
                    }
                    break;
                case 2:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m2origin2.x + 0.07f, m2Pusher2.position.y, m2Pusher2.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher2, target, duration, power, mNum));
                        //Debug.Log("2호기 투입기 푸셔 제어 호출 됨");
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m2origin2.x, m2Pusher2.position.y, m2Pusher2.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher2, target, duration, power, mNum));
                    }
                    break;
                case 3: 
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m2origin3.x + 0.07f, m2Pusher3.position.y, m2Pusher3.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher3, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m2origin3.x, m2Pusher3.position.y, m2Pusher3.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher3, target, duration, power, mNum));
                    }
                    break;
            }
            //Debug.Log("mNum: "+mNum);
            //StartCoroutine(MoveToPosition(start, end.position, duration, power, mNum));
        }
    }
}

