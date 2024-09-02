using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDropper : MonoBehaviour
{
    public GameObject fruitPrefab; // 과일 프리팹
    public Transform dropPoint; // 과일을 투하할 위치

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

        // 과일을 살짝 위로 밀어내어 자연스럽게 투하되게 함
        fruitRb.AddForce(Vector3.up * 2.0f, ForceMode.Impulse);
    }
}
