using System.Collections;
using UnityEngine;

namespace Factory
{
    public class Case2BMachineNode : Case2BMachine
    {
        public Case2BMachine mainMachine;
        public Transform nodeTransform; // 자식 클래스의 Transform
        private Vector3 nodeOriginalPosition; // 자식 클래스의 원래 위치

        private int number;

        private new void Start()
        {
            base.Start(); // 부모 클래스의 Start 메서드 호출
            nodeOriginalPosition = nodeTransform.position; // 자식 클래스의 초기 위치 저장

            if (mainMachine != null)
            {
                // 부모 클래스의 RunStart 이벤트에 자식 클래스의 OnParentRunStart 메서드를 구독
                mainMachine.OnRunStart += OnParentRunStart;
                mainMachine.OnDetectObject += OnParentDetectObject;
            }
        }

        private void OnDestroy()
        {
            if (mainMachine != null)
            {
                // 부모 클래스의 RunStart 이벤트 구독 해제
                mainMachine.OnRunStart -= OnParentRunStart;
                mainMachine.OnDetectObject -= OnParentDetectObject;
            }
        }

        // 부모 클래스의 RunStart 이벤트가 호출될 때 실행될 메서드
        private void OnParentRunStart()
        {
            // 자식 클래스에서 추가 작업을 수행할 수 있습니다.
            Debug.Log("Parent RunStart called. Additional actions in Case2BMachineNode.");
            // 자식 클래스의 RunStart 메서드 실행
            RunStart();
        }
        public void DetectObject(Collider other, int Num)
        {
            number = Num;
            OnTriggerEnter(other);
        }

        // 부모 클래스의 DetectObject 이벤트가 호출될 때 실행될 메서드
        private void OnParentDetectObject(Collider detectedCollider)
        {
            Debug.Log("Parent DetectObject called. Additional actions in Case2BMachineNode.");
            // 자식 클래스의 OnTriggerEnter 메서드 실행
            StartCoroutine(MoveToTargetAndBack(detectedCollider));
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Case2BMachineNode: 감지됨.");
            if (other.TryGetComponent<Chip>(out var ec))
            {
                if(number == 1)
                {
                    if (other.name == "ChipSpawnPoint1(Clone)")
                    {
                        //Debug.Log("Trigger entered by: " + other.name);
                        StartCoroutine(MoveToTargetAndBack(other));
                    }
                }
                else if(number== 2)
                {
                    if (other.name == "ChipSpawnPoint2(Clone)")
                    {
                        //Debug.Log("Trigger entered by: " + other.name);
                        StartCoroutine(MoveToTargetAndBack(other));
                    }

                }
                
                       
            }
        }

        protected override IEnumerator MoveToTargetAndBack(Collider detectedCollider)
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
                //Debug.Log("EdukitChip 변수 변경됨: " + ec.mr.material);
            }


            // 원래 위치로 이동
            yield return StartCoroutine(MoveToPosition(nodeTransform, nodeOriginalPosition, moveDuration));

            // 일정 시간 동안 트리거 감지 비활성화
            //yield return new WaitForSeconds(disableTriggerDuration);

            // 다시 목표 위치로 이동
            yield return StartCoroutine(MoveToPosition(nodeTransform, moveTargetPosition.position, moveDuration));

            isMoving = false; // 이동 중이 아님으로 설정
        }

        protected override IEnumerator MoveToInitialPosition()
        {
            // 자식 클래스의 Transform을 사용하여 목표 위치로 이동
            yield return StartCoroutine(MoveToPosition(nodeTransform, moveTargetPosition.position, moveDuration));
        }

        protected override IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
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
    }
}
