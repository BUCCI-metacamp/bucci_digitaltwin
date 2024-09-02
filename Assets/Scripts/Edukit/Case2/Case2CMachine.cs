using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Factory
{
    public class Case2CMachine : CaseSubMachine
    {
       

        public Transform targetPosition;
        public float moveDuration = 2.0f;
        public float rotationDuration = 3.0f; // 회전 시간
        public Transform rotatingObject; // 회전할 오브젝트
        public GameObject rotationEffect; // 회전 이펙트 오브젝트

        private Vector3 originalPosition;
        private bool isMoving = false;

        [SerializeField]
        Chip stackChip;
        [SerializeField]
        Transform chipSpawnPoint;
        [SerializeField]
        Transform pusher;
     

        public event Action onCountReset;
        public event Action<Chip> onCreateChip;

        //BK add


        public bool Test_Check = false;
        private bool hasMoved = false; // 이동이 이미 발생했는지 여부를 추적하는 플래그
        public float delay = 5.0f; // Test_Check을 false로 만들기 전에 대기할 시간(초)

        public Case2Conveyor Conveyor;

        public Case2CMachine1st machine1st;
        public Case2CMachine2st machine2st;

        private void Start()
        {
            originalPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("감지가 되었습니다.");

            if (!hasMoved && other.TryGetComponent<Chip>(out var ec))
            {
                Debug.Log("Trigger entered by: " + other.name);
                if (other.name == "ChipSpawnPoint3(Clone)")
                {
                    Test_Check = true;
                    machine1st.TriggerEnter();
                    machine2st.TriggerEnter();
                    StartCoroutine(MoveToTargetAndBack(other.gameObject));
                    hasMoved = true; // 이동이 발생했음을 표시
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            hasMoved = false; // 이동이 발생했음을 표시
        }

        private IEnumerator ResetTestCheckAfterDelay()
        {
            //yield return new WaitForSeconds(delay);
            Test_Check = false;
            Debug.Log("Test_Check이 false로 리셋되었습니다.");
            yield return null;
        }

        private IEnumerator MoveToTargetAndBack(GameObject detectedObject)
        {
            isMoving = true;

            // 목표 위치로 이동
            yield return StartCoroutine(MoveToPosition(transform, targetPosition.position, moveDuration));

            // 회전 동작
            yield return StartCoroutine(RotateObject(rotatingObject, rotationDuration));

            // 프리팹 교체
            ReplacePrefab(detectedObject);
            // 컨베이어밸트 재가동
            yield return StartCoroutine(ResetTestCheckAfterDelay());

            // 원래 위치로 돌아오기
            yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

            isMoving = false;


            hasMoved = false; // 이동이 발생했음을 표시
        }

        private IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
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

        private IEnumerator RotateObject(Transform obj, float duration)
        {
            float elapsedTime = 0;
            float anglePerFrame = 360f / duration * Time.deltaTime;

            // 이펙트 활성화
            rotationEffect.SetActive(true);

            while (elapsedTime < duration)
            {
                obj.Rotate(new Vector3(0, 0, anglePerFrame));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 이펙트 비활성화
            rotationEffect.SetActive(false);
        }

        private void ReplacePrefab(GameObject detectedObject)
        {
            // 기존 오브젝트의 위치와 회전값을 저장
            Vector3 position = detectedObject.transform.position;
            Quaternion rotation = detectedObject.transform.rotation;


            // 하위 오브젝트 찾기 예제
            Transform childTransform = detectedObject.transform.Find("CanTop"); // 경로를 통해 하위 오브젝트 찾기
            childTransform.gameObject.SetActive(true);
            if (childTransform != null)
            {
                // 하위 오브젝트에 대한 작업 수행
                Debug.Log("Child Object found: " + childTransform.name);
            }
            else
            {
                Debug.LogWarning("Child Object not found!");
            }

            return;            
        }

        
    }
}
