using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;

namespace Factory
{
    public class Factory : MonoBehaviour
    {
        public M01 m1;
        public M02 m2;
        public M03 m3;
        public CaseConveyor conv;
        
        public Transform m1Pusher;
        public Transform m2Pusher;
        public Transform m3Pusher;

        private Vector3 m1origin;
        private Vector3 m2origin;
        private Vector3 m3origin;
        
        //public List<Chip> runningChips = new();
        public ConcurrentBag<Chip> runningChips = new ConcurrentBag<Chip>();
        public event Action<int> PusherMoved;
        
        private void Awake() 
        {
            //conv = GetComponentInChildren<CaseConveyor>();  
            m1.StartMoving += StartMoving;
            m2.StartMoving += StartMoving;
            m3.StartMoving += StartMoving;

            m1origin = m1Pusher.position;
            m2origin = m2Pusher.position;
            m3origin = m3Pusher.position;
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
            if(mNum==1 && power == "OFF") Debug.Log("1호기 투입기 푸셔 제어OFF");
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
                        Vector3 target = new Vector3(m1origin.x + 0.07f, m1Pusher.position.y, m1Pusher.position.z);
                        StartCoroutine(MoveToPosition(m1Pusher, target, duration, power, mNum));
                        Debug.Log("1호기 투입기 푸셔 제어ON");
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m1origin.x, m1Pusher.position.y, m1Pusher.position.z);
                        StartCoroutine(MoveToPosition(m1Pusher, target, duration, power, mNum));
                    }
                    break;
                // 투입기 푸셔 제어
                case 2:
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m2origin.x + 0.07f, m2Pusher.position.y, m2Pusher.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher, target, duration, power, mNum));
                        //Debug.Log("2호기 투입기 푸셔 제어 호출 됨");
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m2origin.x, m2Pusher.position.y, m2Pusher.position.z);
                        StartCoroutine(MoveToPosition(m2Pusher, target, duration, power, mNum));
                    }
                    break;
                case 3: // 가공기 푸셔 제어
                    if (power == "ON")
                    {
                        Vector3 target = new Vector3(m3Pusher.position.x, m3origin.y- 0.05f, m3Pusher.position.z);
                        Debug.Log("M3ON");
                        StartCoroutine(MoveToPosition(m3Pusher, target, duration, power, mNum));
                    }
                    else if (power == "OFF")
                    {
                        Vector3 target = new Vector3(m3Pusher.position.x, m3origin.y, m3Pusher.position.z);
                        Debug.Log("M3OFF");
                        StartCoroutine(MoveToPosition(m3Pusher, target, duration, power, mNum));
                    }
                    //StartCoroutine(MoveToPosition(m1Pusher, target, duration, power, mNum));
                    break;
            }
            //Debug.Log("mNum: "+mNum);
            //StartCoroutine(MoveToPosition(start, end.position, duration, power, mNum));
        }
        
        /// <summary>
        /// 여긴 모름!
        /// </summary>
        void ResetEdukit()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        
    }
}

