using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class case2VisionTriiger : MonoBehaviour
{
    public Material goodMaterial;
    public Material badMaterial;

    void Start()
    {
        // 초기화 코드 (필요할 경우)
    }

    void Update()
    {
        // 매 프레임 실행되는 코드 (필요할 경우)
    }

    void OnTriggerEnter(Collider other)
    {
        // 트리거 영역에 다른 오브젝트가 들어왔을 때 실행되는 코드
        if (other.CompareTag("Inspectable"))
        {
            Debug.Log("Object entered the trigger!");

            // 검사 수행
            bool isGood = PerformInspection();

            // 검사 결과에 따라 오브젝트 색상 변경
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = isGood ? goodMaterial : badMaterial;
            }
        }
    }

    bool PerformInspection()
    {
        // 검사 로직 (임시로 무작위 결과 반환)
        // 실제 검사 로직을 여기에 구현
        return Random.value > 0.5f;
    }
}
