using System.Collections;
using UnityEngine;
using System;

namespace Factory
{
    public class Case2AMachine : CaseSubMachine
    {        

        public Transform targetPosition;
        public float moveDuration = 2.0f;
        public float repeatInterval = 5.0f; // �ݺ� �ð� ����

        private Vector3 originalPosition;
        private bool isMoving = false;

        [SerializeField]
        Chip stackChip;
        [SerializeField]
        Transform chipSpawnPoint;
        [SerializeField]
        Transform pusher;

        
        public int currentCount;        

       // public event Action onCountReset;
        public event Action<Chip> onCreateChip;
    

        public BoxCollider Trigger;

        public bool Test_Check = false;

        public int machineNumber;
        //private int nextSerialNumber = 1; // ���� ��ȣ�� ���� ���� ī���� 

        private void Start()
        {
            originalPosition = transform.position;
            // �ʱ� ī��Ʈ ����
            //currentCount = initialValue;
        }

        public void StartRun()
        {
            stackChip.gameObject.SetActive(true);
            StartCoroutine(StartProcess());
        }

        private IEnumerator StartProcess()
        {
            while (currentCount > 0)
            {
                Test_Check = true;
                CreateChip();
                yield return new WaitForSeconds(3);  // 3�� ���
                if (!isMoving)
                {
                    yield return StartCoroutine(MoveToTargetAndBack());
                }
                yield return new WaitForSeconds(repeatInterval); // �ݺ� ���� ���
            }
        }

        public void CreateChip()
        {

            var chip = Instantiate(chipSpawnPoint).GetComponent<Chip>();
            //chip.SetSerialNumber(machineNumber,nextSerialNumber++); // ��ȣ �Ҵ� �� ����
            chip.gameObject.SetActive(true);
            onCreateChip?.Invoke(chip);
        }

        IEnumerator MoveToTargetAndBack()
        {
            isMoving = true;
           
            // �̵�
            yield return StartCoroutine(MoveToPosition(transform, targetPosition.position, moveDuration));

            Test_Check = false;
            // ���� ��ġ�� ���ƿ���
            yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

            currentCount--; // ī��Ʈ ����
            isMoving = false;
        }

        IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
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
