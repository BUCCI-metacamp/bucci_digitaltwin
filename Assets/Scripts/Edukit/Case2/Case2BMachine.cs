using System;
using System.Collections;
using UnityEngine;

namespace Factory
{
    public class Case2BMachine : CaseSubMachine
    {
        public string No2Standby;
        public string No2SensingMemory;
        public string No2PowerState;
        public string No2Count;
        public string No2Chip;
        public string No2CubeFull;
        public string No2InPoint;
        public string No2OutPoint;
        public string No2Sol;
        public string No2SolAction;
        public string No2BackToSquare;
        public string No2OperationMode;

        public Transform stackCube;
        public CaseEdukitDice unknownCube;
        public event Action<CaseEdukitDice> onCreateDice;

        public bool Test_Check;
        public float delay = 5.0f; // Test_Check을 false로 만들기 전에 대기할 시간(초)
        public Transform moveTargetPosition; // 이동할 목표 위치
        public float moveDuration = 2.0f; // 이동에 걸리는 시간
        public float waitBeforeReturn = 2.0f; // 목표 위치에서 대기할 시간
        public float disableTriggerDuration = 0.1f; // 트리거 비활성화 시간

        protected Vector3 originalPosition; // 원래 위치
        protected bool hasMoved = false; // 이동이 이미 발생했는지 여부를 추적하는 플래그
        protected bool isMoving = false; // 이동 중인지 여부를 추적하는 플래그

        // 이벤트 선언
        public event Action OnRunStart;
        public event Action<Collider> OnDetectObject;

        protected new void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            originalPosition = transform.position; // 초기 위치 저장
        }

        public void RunStart()
        {
            OnRunStart?.Invoke(); // 이벤트 호출
            StartCoroutine(MoveToInitialPosition());
        }

        protected virtual IEnumerator MoveToInitialPosition()
        {
            // 목표 위치로 이동
            yield return StartCoroutine(MoveToPosition(transform, moveTargetPosition.position, moveDuration));
        }

        // 외부 콜리더에서 호출할 수 있도록 공개 메서드로 정의
        public void DetectObject(Collider other)
        {
            OnTriggerEnter(other);
            OnDetectObject?.Invoke(other);
        }

        public void ExitObject()
        {
            hasMoved = false; // 이동이 발생했음을 표시
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("감지가 되었습니다. Bmachine,Bmachine,Bmachine,Bmachine,");

            if (!hasMoved && !isMoving && other.TryGetComponent<Chip>(out var ec))
            {
                Debug.Log("Trigger entered by: " + other.name);
                if (other.name == "ChipSpawnPoint3(Clone)")
                {
                    Test_Check = true;                    
                    StartCoroutine(MoveToTargetAndBack(other));
                    hasMoved = true; // 이동이 발생했음을 표시
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            hasMoved = false; // 이동이 발생했음을 표시
        }

        

        protected virtual IEnumerator MoveToTargetAndBack(Collider detectedCollider)
        {
            isMoving = true; // 이동 중으로 설정

            // 목표 위치에서 대기
            yield return new WaitForSeconds(waitBeforeReturn);

            // detectedCollider의 변수값 변경
            if (detectedCollider.TryGetComponent<Chip>(out var ec))
            {
                // 예: EdukitChip의 특정 변수 변경 
                //ec.SetFruit(Chip.Fruit.Apple);
                // 하위 오브젝트 찾기 예제
                Transform childTransform = ec.transform.Find("Apple"); // 경로를 통해 하위 오브젝트 찾기
                childTransform.gameObject.SetActive(true);
                //sDebug.Log("EdukitChip 변수 변경됨: " + ec.mr.material);
            }
            StartCoroutine(ResetTestCheckAfterDelay());

            // 원래 위치로 이동
            yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

            // 일정 시간 동안 트리거 감지 비활성화
           // yield return new WaitForSeconds(disableTriggerDuration);

            // 다시 목표 위치로 이동
            yield return StartCoroutine(MoveToPosition(transform, moveTargetPosition.position, moveDuration));

            isMoving = false; // 이동 중이 아님으로 설정
        }

        protected virtual IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
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

        protected virtual IEnumerator ResetTestCheckAfterDelay()
        {
            // yield return new WaitForSeconds(delay);
            Test_Check = false;
            Debug.Log("Test_Check이 false로 리셋되었습니다.");
            yield return null;
        }

        internal void SetDice()
        {
            unknownCube.gameObject.SetActive(true);
        }
    }
}
