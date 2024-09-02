using System;
using System.Collections;
using UnityEngine;

namespace Factory
{
    public class Case2Robot : CaseSubMachine
    {
        public string No3ChipArrival;
        public string No3PowerState;
        public string No3Count;
        public string No3Motor1Position;
        public string No3Motor2Position;
        public string No3Gripper;
        public string No3Motor1Action;
        public string No3Motor2Action;

        public Chip inGrabChip;
        public Transform moter1; // A 오브젝트
        public Transform moter2_1; // B 오브젝트
        public Transform moter2_2; // C 오브젝트
        public Transform grabPoint;
        public Transform targetPosition1; // A 오브젝트 1번째 목표지점
        public Transform targetPosition2; // A 오브젝트 2번째 목표지점
        public Vector3 BTargetRotation; // B 오브젝트 목표 회전값
        public Vector3 CTargetRotation; // C 오브젝트 목표 회전값
        Animator animator;

        private Vector3 moter1OriginalPosition; // A 오브젝트 원래 위치
        private Vector3 moter2_1OriginalRotation; // B 오브젝트 원래 회전값
        private Vector3 moter2_2OriginalRotation; // C 오브젝트 원래 회전값
        private bool isRunningSequence = false; // 트리거 실행 상태를 나타내는 변수
        float originMotor1;
        float m22rot;
        float m21rot;

        public Case2Conveyor Case2Conveyor;

        float moveSpeed = 1.0f;

        public float Axis1stmoveSpeed = 1.0f;
        public float Axis2stmoveSpeed = 1.0f;
        public int caseType;
        private new void Awake()
        {
            base.Awake();
            originMotor1 = moter1.localPosition.y;
            animator = GetComponent<Animator>();
            m21rot = moter2_1.localRotation.eulerAngles.y;
            m22rot = moter2_2.localRotation.eulerAngles.z;
        }

        private new void Start()
        {
            moter1OriginalPosition = moter1.localPosition;
            moter2_1OriginalRotation = moter2_1.localRotation.eulerAngles;
            moter2_2OriginalRotation = moter2_2.localRotation.eulerAngles;
            if(caseType == 1)
            {
                ControlSettingVIewCase2 settingView = GameObject.Find("SettingView").GetComponent<ControlSettingVIewCase2>();
                settingView.RMachine1stSpeed += Setting1stSpeed; //칩간격
                settingView.RMachine2stSpeed += Setting2stSpeed; //공정속도 
            }
            else
            {
                ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
                settingView.RMachine1stSpeed += Setting1stSpeed; //칩간격
                settingView.RMachine2stSpeed += Setting2stSpeed; //공정속도 
            }
            
        }

        void Setting1stSpeed(float loadCapacity) //1축속도
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            Axis1stmoveSpeed = loadCapacity;
            Debug.Log("M01Time updated to: " + Axis1stmoveSpeed);
        }

        void Setting2stSpeed(float loadCapacity) //2축속도
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            Axis2stmoveSpeed = loadCapacity;
            Debug.Log("M01Time updated to: " + Axis2stmoveSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isRunningSequence && other.TryGetComponent<Chip>(out var c))
            {
                if(c.GetQualityState() == Chip.QualityState.ProductSucess)
                {
                    inGrabChip = c;
                    Case2Conveyor.ExitTrigger(other);
                    StartCoroutine(ExecuteSequence());
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isRunningSequence && other.TryGetComponent<Chip>(out var c))
            {
                inGrabChip = null;
            }
        }

        private IEnumerator ExecuteSequence()
        {
            isRunningSequence = true; // 트리거 비활성화

            // OnGrap 수행
            OnGrap();

            // A 오브젝트 1번째 목표지점 이동
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition1.localPosition, moveSpeed));
            // yield return new WaitForSeconds(0.2f);

            // B 오브젝트와 C 오브젝트 목표 회전값으로 회전
            yield return StartCoroutine(WaitAll(
                RotateToRotation(moter2_1, BTargetRotation, moveSpeed),
                RotateToRotation(moter2_2, CTargetRotation, moveSpeed)
            ));
            // yield return new WaitForSeconds(0.2f);

            // A 오브젝트 2번째 목표지점 이동
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition2.localPosition, moveSpeed));
            //yield return new WaitForSeconds(0.2f);

            // OffGrap 수행 - 트리거가 재감지되면 안된다.
            OffGrap();

            // A 오브젝트 2번째 목표지점으로 다시 이동
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition2.localPosition, moveSpeed));
            // yield return new WaitForSeconds(0.2f);

            // C 오브젝트와 B 오브젝트 원래 회전값으로 회전
            yield return StartCoroutine(WaitAll(
                RotateToRotation(moter2_2, moter2_2OriginalRotation, moveSpeed),
                RotateToRotation(moter2_1, moter2_1OriginalRotation, moveSpeed)
            ));
            // yield return new WaitForSeconds(0.2f);

            // A 오브젝트 원래 위치로 이동
            yield return StartCoroutine(MoveToPosition(moter1, moter1OriginalPosition, moveSpeed));

            isRunningSequence = false; // 트리거 다시 활성화
        }

        private IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
        {
            Vector3 start = obj.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                obj.localPosition = Vector3.Lerp(start, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.localPosition = target;
        }

        private IEnumerator RotateToRotation(Transform obj, Vector3 targetRotation, float duration)
        {
            Quaternion startRotation = obj.localRotation;
            Quaternion endRotation = Quaternion.Euler(targetRotation);
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                obj.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.localRotation = endRotation;
        }

        // 두 개의 코루틴을 병렬로 실행하고 둘 다 완료될 때까지 기다리는 메서드
        private IEnumerator WaitAll(IEnumerator coroutine1, IEnumerator coroutine2)
        {
            bool coroutine1Complete = false;
            bool coroutine2Complete = false;

            // 코루틴1
            StartCoroutine(RunAndNotify(coroutine1, () => coroutine1Complete = true));

            // 코루틴2
            StartCoroutine(RunAndNotify(coroutine2, () => coroutine2Complete = true));

            // 둘 다 완료될 때까지 대기
            while (!coroutine1Complete || !coroutine2Complete)
            {
                yield return null;
            }
        }

        // 코루틴이 완료되면 알림을 받는 메서드
        private IEnumerator RunAndNotify(IEnumerator coroutine, Action onComplete)
        {
            yield return StartCoroutine(coroutine);
            onComplete();
        }

        public void OnGrap()
        {
            inGrabChip.transform.position = grabPoint.position;
            inGrabChip?.OnGrab(grabPoint);
            animator.Play("Grab");
        }

        public void OffGrap()
        {
            if (inGrabChip != null)
            {
                inGrabChip.HandOff();
                inGrabChip = null;
            }
            animator.Play("Drop");
        }

    }
}
