using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class case2VisionTriiger : MonoBehaviour
{
    public Material goodMaterial;
    public Material badMaterial;

    void Start()
    {
        // �ʱ�ȭ �ڵ� (�ʿ��� ���)
    }

    void Update()
    {
        // �� ������ ����Ǵ� �ڵ� (�ʿ��� ���)
    }

    void OnTriggerEnter(Collider other)
    {
        // Ʈ���� ������ �ٸ� ������Ʈ�� ������ �� ����Ǵ� �ڵ�
        if (other.CompareTag("Inspectable"))
        {
            Debug.Log("Object entered the trigger!");

            // �˻� ����
            bool isGood = PerformInspection();

            // �˻� ����� ���� ������Ʈ ���� ����
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = isGood ? goodMaterial : badMaterial;
            }
        }
    }

    bool PerformInspection()
    {
        // �˻� ���� (�ӽ÷� ������ ��� ��ȯ)
        // ���� �˻� ������ ���⿡ ����
        return Random.value > 0.5f;
    }
}
