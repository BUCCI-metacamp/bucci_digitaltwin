using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Factory
{
    public class Case2CMachine2st : CaseSubMachine
    {
       

        public Transform targetPosition;
        public float moveDuration = 2.0f;
        public float rotationDuration = 3.0f; // ȸ�� �ð�
        public Transform rotatingObject; // ȸ���� ������Ʈ
        public GameObject rotationEffect; // ȸ�� ����Ʈ ������Ʈ

        private Vector3 originalPosition;
        private bool isMoving = false;

        [SerializeField]
        Chip stackChip;
        [SerializeField]
        Transform chipSpawnPoint;
        [SerializeField]
        Transform pusher;
       

        bool pushing;
        int prevCount;

        public event Action onCountReset;
        public event Action<Chip> onCreateChip;

        //BK add
        private int initialValue = int.MaxValue;

        public bool Test_Check = false;
        private bool hasMoved = false; // �̵��� �̹� �߻��ߴ��� ���θ� �����ϴ� �÷���
        public float delay = 5.0f; // Test_Check�� false�� ����� ���� ����� �ð�(��)

        public Case2Conveyor Conveyor;
          
        public Case2CMachine mainCMachine;
        Collider saveOther;

        private void Start()
        {
            originalPosition = transform.position;
        }

        public void TriggerEnter()
        {
            OnTriggerEnter(saveOther);
        }

        private void OnTriggerEnter(Collider other)
        {
            saveOther = other;
            //Debug.Log("������ �Ǿ����ϴ�.");
            if (mainCMachine.Test_Check == false)
                return;
            if (!hasMoved && other.TryGetComponent<Chip>(out var ec))
            {
                Debug.Log("Trigger entered by: " + other.name);
                if (other.name == "ChipSpawnPoint2(Clone)")
                {
                    Test_Check = true;
                    StartCoroutine(MoveToTargetAndBack(other.gameObject));
                    hasMoved = true; // �̵��� �߻������� ǥ��
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            hasMoved = false; // �̵��� �߻������� ǥ��
        }

        private IEnumerator ResetTestCheckAfterDelay()
        {
            //yield return new WaitForSeconds(delay);
            Test_Check = false;
            Debug.Log("Test_Check�� false�� ���µǾ����ϴ�.");
            yield return null;
        }

        private IEnumerator MoveToTargetAndBack(GameObject detectedObject)
        {
            isMoving = true;

            // ��ǥ ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(transform, targetPosition.position, moveDuration));

            // ȸ�� ����
            yield return StartCoroutine(RotateObject(rotatingObject, rotationDuration));

            // ������ ��ü
            ReplacePrefab(detectedObject);
            // �����̾��Ʈ �簡��
            yield return StartCoroutine(ResetTestCheckAfterDelay());

            // ���� ��ġ�� ���ƿ���
            yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

            isMoving = false;


            hasMoved = false; // �̵��� �߻������� ǥ��
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

            // ����Ʈ Ȱ��ȭ
            rotationEffect.SetActive(true);

            while (elapsedTime < duration)
            {
                obj.Rotate(new Vector3(0, 0, anglePerFrame));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ����Ʈ ��Ȱ��ȭ
            rotationEffect.SetActive(false);
        }

        private void ReplacePrefab(GameObject detectedObject)
        {
            // ���� ������Ʈ�� ��ġ�� ȸ������ ����
            Vector3 position = detectedObject.transform.position;
            Quaternion rotation = detectedObject.transform.rotation;


            // ���� ������Ʈ ã�� ����
            Transform childTransform = detectedObject.transform.Find("CanTop"); // ��θ� ���� ���� ������Ʈ ã��
            childTransform.gameObject.SetActive(true);
            if (childTransform != null)
            {
                // ���� ������Ʈ�� ���� �۾� ����
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
