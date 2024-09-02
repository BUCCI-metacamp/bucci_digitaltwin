using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropper : MonoBehaviour
{
    public GameObject fruitPrefab; // ���� ������
    public Transform dropPoint; // ������ ������ ��ġ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropFruit();
        }
    }

    void DropFruit()
    {
        GameObject fruit = Instantiate(fruitPrefab, dropPoint.position, Quaternion.identity);
        Rigidbody fruitRb = fruit.GetComponent<Rigidbody>();

        // ������ ��¦ ���� �о�� �ڿ������� ���ϵǰ� ��
        fruitRb.AddForce(Vector3.up * 2.0f, ForceMode.Impulse);
    }
}
