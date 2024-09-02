using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;

namespace Factory
{
    public class Factory1 : MonoBehaviour
    {
        public Case2M01 m1f1;
        public Case2M01 m1f2;
        public Case2M01 m1f3;
        public CaseConveyor conv;
        public Transform m1PusherF1;
        public Transform m1PusherF2;
        public Transform m1PusherF3;

        private Vector3 m1origin1;
        private Vector3 m1origin2;
        private Vector3 m1origin3;
        //public List<Chip> runningChips = new();
        public ConcurrentBag<Chip> runningChips = new ConcurrentBag<Chip>();
        public event Action<int> PusherMoved;
        
        private void Awake() 
        {
            conv = GetComponentInChildren<CaseConveyor>();
            m1f1.StartMoving += StartMoving;
            m1f2.StartMoving += StartMoving;
            m1f3.StartMoving += StartMoving;
            m1origin1 = m1PusherF1.position;
            m1origin2 = m1PusherF2.position;
            m1origin3 = m1PusherF3.position;
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
                        Vector3 target = new Vector3(m1origin1.x + 0.07f, m1PusherF1.position.y, m1PusherF1.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF1, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m1origin1.x, m1PusherF1.position.y, m1PusherF1.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF1, target, duration, power, mNum));
                    }
                    break;
                case 2:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m1origin2.x + 0.07f, m1PusherF2.position.y, m1PusherF2.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF2, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m1origin2.x, m1PusherF2.position.y, m1PusherF2.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF2, target, duration, power, mNum));
                    }
                    break;
                case 3:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m1origin3.x + 0.07f, m1PusherF3.position.y, m1PusherF3.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF3, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m1origin3.x, m1PusherF3.position.y, m1PusherF3.position.z);
                        StartCoroutine(MoveToPosition(m1PusherF3, target, duration, power, mNum));
                    }
                    break;
            }
            //Debug.Log("mNum: "+mNum);
            //StartCoroutine(MoveToPosition(start, end.position, duration, power, mNum));
        }
        
    }
}

