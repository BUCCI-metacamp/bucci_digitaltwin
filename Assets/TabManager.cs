using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] panels; // 탭에 연결된 패널들
    public Button[] tabButtons; // 탭 버튼들

    void Start()
    {
        // 각 버튼에 OnClick 이벤트 추가
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // 클로저 문제를 피하기 위해 로컬 변수 사용
            tabButtons[i].onClick.AddListener(() => OnTabButtonClicked(index));
        }

        // 초기 상태로 첫 번째 탭 활성화
        //ActivatePanel(0);
    }

    public void OnTabButtonClicked(int index)
    {
        ActivatePanel(index);
    }

    public void ActivatePanel(int index)
    {
        // 모든 패널 비활성화
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // 선택한 패널만 활성화
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
        }
    }
}

