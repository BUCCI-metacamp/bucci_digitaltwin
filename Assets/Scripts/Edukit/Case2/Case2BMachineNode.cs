using System.Collections;
using UnityEngine;

namespace Factory
{
    public class Case2BMachineNode : Case2BMachine
    {
        public Case2BMachine mainMachine;
        public Transform nodeTransform; // �ڽ� Ŭ������ Transform
        private Vector3 nodeOriginalPosition; // �ڽ� Ŭ������ ���� ��ġ

        private int number;

        private new void Start()
        {
            base.Start(); // �θ� Ŭ������ Start �޼��� ȣ��
            nodeOriginalPosition = nodeTransform.position; // �ڽ� Ŭ������ �ʱ� ��ġ ����

            if (mainMachine != null)
            {
                // �θ� Ŭ������ RunStart �̺�Ʈ�� �ڽ� Ŭ������ OnParentRunStart �޼��带 ����
                mainMachine.OnRunStart += OnParentRunStart;
                mainMachine.OnDetectObject += OnParentDetectObject;
            }
        }

        private void OnDestroy()
        {
            if (mainMachine != null)
            {
                // �θ� Ŭ������ RunStart �̺�Ʈ ���� ����
                mainMachine.OnRunStart -= OnParentRunStart;
                mainMachine.OnDetectObject -= OnParentDetectObject;
            }
        }

        // �θ� Ŭ������ RunStart �̺�Ʈ�� ȣ��� �� ����� �޼���
        private void OnParentRunStart()
        {
            // �ڽ� Ŭ�������� �߰� �۾��� ������ �� �ֽ��ϴ�.
            Debug.Log("Parent RunStart called. Additional actions in Case2BMachineNode.");
            // �ڽ� Ŭ������ RunStart �޼��� ����
            RunStart();
        }
        public void DetectObject(Collider other, int Num)
        {
            number = Num;
            OnTriggerEnter(other);
        }

        // �θ� Ŭ������ DetectObject �̺�Ʈ�� ȣ��� �� ����� �޼���
        private void OnParentDetectObject(Collider detectedCollider)
        {
            Debug.Log("Parent DetectObject called. Additional actions in Case2BMachineNode.");
            // �ڽ� Ŭ������ OnTriggerEnter �޼��� ����
            StartCoroutine(MoveToTargetAndBack(detectedCollider));
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Case2BMachineNode: ������.");
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
                //Debug.Log("EdukitChip ���� �����: " + ec.mr.material);
            }


            // ���� ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(nodeTransform, nodeOriginalPosition, moveDuration));

            // ���� �ð� ���� Ʈ���� ���� ��Ȱ��ȭ
            //yield return new WaitForSeconds(disableTriggerDuration);

            // �ٽ� ��ǥ ��ġ�� �̵�
            yield return StartCoroutine(MoveToPosition(nodeTransform, moveTargetPosition.position, moveDuration));

            isMoving = false; // �̵� ���� �ƴ����� ����
        }

        protected override IEnumerator MoveToInitialPosition()
        {
            // �ڽ� Ŭ������ Transform�� ����Ͽ� ��ǥ ��ġ�� �̵�
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
