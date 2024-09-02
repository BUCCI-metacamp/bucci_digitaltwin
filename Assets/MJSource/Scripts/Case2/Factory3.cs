using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;

namespace Factory
{
    public class Factory3 : MonoBehaviour
    {
        public Case2M03 M3F1;
        public Case2M03 M3F2;
        public Case2M03 M3F3;
        public CaseConveyor conv;
        
        public Transform m3Pusher1;
        public Transform m3Pusher2;
        public Transform m3Pusher3;

        private Vector3 m3origin1;
        private Vector3 m3origin2;
        private Vector3 m3origin3;
        
        //public List<Chip> runningChips = new();
        public ConcurrentBag<Chip> runningChips = new ConcurrentBag<Chip>();
        public event Action<int> PusherMoved;
        
        private void Awake() 
        {
            conv = GetComponentInChildren<CaseConveyor>();
            M3F1.StartMoving += StartMoving;
            M3F2.StartMoving += StartMoving;
            M3F3.StartMoving += StartMoving;

            m3origin1 = m3Pusher1.position;
            m3origin2 = m3Pusher2.position;
            m3origin3 = m3Pusher3.position;
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
                // 반출기 푸셔 제어
                case 1:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m3Pusher1.position.x, m3origin1.y- 0.05f, m3Pusher1.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher1, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m3Pusher1.position.x, m3origin1.y, m3Pusher1.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher1, target, duration, power, mNum));
                    }
                    break;
                case 2:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m3Pusher2.position.x, m3origin2.y- 0.05f, m3Pusher2.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher2, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m3Pusher2.position.x, m3origin2.y, m3Pusher2.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher2, target, duration, power, mNum));
                    }
                    break;
                case 3: 
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m3Pusher3.position.x, m3origin3.y- 0.05f, m3Pusher3.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher3, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m3Pusher3.position.x, m3origin3.y, m3Pusher3.position.z);
                        StartCoroutine(MoveToPosition(m3Pusher3, target, duration, power, mNum));
                    }
                    break;
            }
            //Debug.Log("mNum: "+mNum);
            //StartCoroutine(MoveToPosition(start, end.position, duration, power, mNum));
        }
        
        
    }
}

