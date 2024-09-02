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
        public Transform moter1; // A ������Ʈ
        public Transform moter2_1; // B ������Ʈ
        public Transform moter2_2; // C ������Ʈ
        public Transform grabPoint;
        public Transform targetPosition1; // A ������Ʈ 1��° ��ǥ����
        public Transform targetPosition2; // A ������Ʈ 2��° ��ǥ����
        public Vector3 BTargetRotation; // B ������Ʈ ��ǥ ȸ����
        public Vector3 CTargetRotation; // C ������Ʈ ��ǥ ȸ����
        Animator animator;

        private Vector3 moter1OriginalPosition; // A ������Ʈ ���� ��ġ
        private Vector3 moter2_1OriginalRotation; // B ������Ʈ ���� ȸ����
        private Vector3 moter2_2OriginalRotation; // C ������Ʈ ���� ȸ����
        private bool isRunningSequence = false; // Ʈ���� ���� ���¸� ��Ÿ���� ����
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
                settingView.RMachine1stSpeed += Setting1stSpeed; //Ĩ����
                settingView.RMachine2stSpeed += Setting2stSpeed; //�����ӵ� 
            }
            else
            {
                ControlSettingView settingView = GameObject.Find("SettingView").GetComponent<ControlSettingView>();
                settingView.RMachine1stSpeed += Setting1stSpeed; //Ĩ����
                settingView.RMachine2stSpeed += Setting2stSpeed; //�����ӵ� 
            }
            
        }

        void Setting1stSpeed(float loadCapacity) //1��ӵ�
        {
            // Parse the loadCapacity string to an integer and assign it to TotalAmount            
            Axis1stmoveSpeed = loadCapacity;
            Debug.Log("M01Time updated to: " + Axis1stmoveSpeed);
        }

        void Setting2stSpeed(float loadCapacity) //2��ӵ�
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
            isRunningSequence = true; // Ʈ���� ��Ȱ��ȭ

            // OnGrap ����
            OnGrap();

            // A ������Ʈ 1��° ��ǥ���� �̵�
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition1.localPosition, moveSpeed));
            // yield return new WaitForSeconds(0.2f);

            // B ������Ʈ�� C ������Ʈ ��ǥ ȸ�������� ȸ��
            yield return StartCoroutine(WaitAll(
                RotateToRotation(moter2_1, BTargetRotation, moveSpeed),
                RotateToRotation(moter2_2, CTargetRotation, moveSpeed)
            ));
            // yield return new WaitForSeconds(0.2f);

            // A ������Ʈ 2��° ��ǥ���� �̵�
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition2.localPosition, moveSpeed));
            //yield return new WaitForSeconds(0.2f);

            // OffGrap ���� - Ʈ���Ű� �簨���Ǹ� �ȵȴ�.
            OffGrap();

            // A ������Ʈ 2��° ��ǥ�������� �ٽ� �̵�
            yield return StartCoroutine(MoveToPosition(moter1, targetPosition2.localPosition, moveSpeed));
            // yield return new WaitForSeconds(0.2f);

            // C ������Ʈ�� B ������Ʈ ���� ȸ�������� ȸ��
            yield return StartCoroutine(WaitAll(
                RotateToRotation(moter2_2, moter2_2OriginalRotation, moveSpeed),
                RotateToRotation(moter2_1, moter2_1OriginalRotation, moveSpeed)
            ));
            // yield return new WaitForSeconds(0.2f);

            // A ������Ʈ ���� ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(moter1, moter1OriginalPosition, moveSpeed));

            isRunningSequence = false; // Ʈ���� �ٽ� Ȱ��ȭ
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

        // �� ���� �ڷ�ƾ�� ���ķ� �����ϰ� �� �� �Ϸ�� ������ ��ٸ��� �޼���
        private IEnumerator WaitAll(IEnumerator coroutine1, IEnumerator coroutine2)
        {
            bool coroutine1Complete = false;
            bool coroutine2Complete = false;

            // �ڷ�ƾ1
            StartCoroutine(RunAndNotify(coroutine1, () => coroutine1Complete = true));

            // �ڷ�ƾ2
            StartCoroutine(RunAndNotify(coroutine2, () => coroutine2Complete = true));

            // �� �� �Ϸ�� ������ ���
            while (!coroutine1Complete || !coroutine2Complete)
            {
                yield return null;
            }
        }

        // �ڷ�ƾ�� �Ϸ�Ǹ� �˸��� �޴� �޼���
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
