using System.Collections;
using UnityEngine;
using System;

namespace Factory
{
    public class Case2AMachineNodeMove : CaseSubMachine
    {
        public string No1ChipEmpty;
        public string No1Push;
        public string No1PowerState;
        public string No1DelayTime;
        public string No1Count;
        public string No1ChipFull;

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

        bool pushing;
        public int currentCount;
        int prevCount; // prevCount ���� �߰�

        public event Action onCountReset;
        public event Action<Chip> onCreateChip;

        private int initialValue = int.MaxValue;

        public BoxCollider Trigger;

        public bool Test_Check = false;
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, 0.01f * Time.deltaTime);
        }

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
            chip.gameObject.SetActive(true);
            onCreateChip?.Invoke(chip);
        }

        IEnumerator MoveToTargetAndBack()
        {
            isMoving = true;

            // �̵�
            yield return StartCoroutine(MoveToPosition(transform, targetPosition.position, moveDuration));

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
