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
        public float delay = 5.0f; // Test_Check�� false�� ����� ���� ����� �ð�(��)
        public Transform moveTargetPosition; // �̵��� ��ǥ ��ġ
        public float moveDuration = 2.0f; // �̵��� �ɸ��� �ð�
        public float waitBeforeReturn = 2.0f; // ��ǥ ��ġ���� ����� �ð�
        public float disableTriggerDuration = 0.1f; // Ʈ���� ��Ȱ��ȭ �ð�

        protected Vector3 originalPosition; // ���� ��ġ
        protected bool hasMoved = false; // �̵��� �̹� �߻��ߴ��� ���θ� �����ϴ� �÷���
        protected bool isMoving = false; // �̵� ������ ���θ� �����ϴ� �÷���

        // �̺�Ʈ ����
        public event Action OnRunStart;
        public event Action<Collider> OnDetectObject;

        protected new void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            originalPosition = transform.position; // �ʱ� ��ġ ����
        }

        public void RunStart()
        {
            OnRunStart?.Invoke(); // �̺�Ʈ ȣ��
            StartCoroutine(MoveToInitialPosition());
        }

        protected virtual IEnumerator MoveToInitialPosition()
        {
            // ��ǥ ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(transform, moveTargetPosition.position, moveDuration));
        }

        // �ܺ� �ݸ������� ȣ���� �� �ֵ��� ���� �޼���� ����
        public void DetectObject(Collider other)
        {
            OnTriggerEnter(other);
            OnDetectObject?.Invoke(other);
        }

        public void ExitObject()
        {
            hasMoved = false; // �̵��� �߻������� ǥ��
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("������ �Ǿ����ϴ�. Bmachine,Bmachine,Bmachine,Bmachine,");

            if (!hasMoved && !isMoving && other.TryGetComponent<Chip>(out var ec))
            {
                Debug.Log("Trigger entered by: " + other.name);
                if (other.name == "ChipSpawnPoint3(Clone)")
                {
                    Test_Check = true;                    
                    StartCoroutine(MoveToTargetAndBack(other));
                    hasMoved = true; // �̵��� �߻������� ǥ��
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            hasMoved = false; // �̵��� �߻������� ǥ��
        }

        

        protected virtual IEnumerator MoveToTargetAndBack(Collider detectedCollider)
        {
            isMoving = true; // �̵� ������ ����

            // ��ǥ ��ġ���� ���
            yield return new WaitForSeconds(waitBeforeReturn);

            // detectedCollider�� ������ ����
            if (detectedCollider.TryGetComponent<Chip>(out var ec))
            {
                // ��: EdukitChip�� Ư�� ���� ���� 
                //ec.SetFruit(Chip.Fruit.Apple);
                // ���� ������Ʈ ã�� ����
                Transform childTransform = ec.transform.Find("Apple"); // ��θ� ���� ���� ������Ʈ ã��
                childTransform.gameObject.SetActive(true);
                //sDebug.Log("EdukitChip ���� �����: " + ec.mr.material);
            }
            StartCoroutine(ResetTestCheckAfterDelay());

            // ���� ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

            // ���� �ð� ���� Ʈ���� ���� ��Ȱ��ȭ
           // yield return new WaitForSeconds(disableTriggerDuration);

            // �ٽ� ��ǥ ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(transform, moveTargetPosition.position, moveDuration));

            isMoving = false; // �̵� ���� �ƴ����� ����
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
            Debug.Log("Test_Check�� false�� ���µǾ����ϴ�.");
            yield return null;
        }

        internal void SetDice()
        {
            unknownCube.gameObject.SetActive(true);
        }
    }
}
