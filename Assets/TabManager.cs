using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] panels; // �ǿ� ����� �гε�
    public Button[] tabButtons; // �� ��ư��

    void Start()
    {
        // �� ��ư�� OnClick �̺�Ʈ �߰�
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // Ŭ���� ������ ���ϱ� ���� ���� ���� ���
            tabButtons[i].onClick.AddListener(() => OnTabButtonClicked(index));
        }

        // �ʱ� ���·� ù ��° �� Ȱ��ȭ
        //ActivatePanel(0);
    }

    public void OnTabButtonClicked(int index)
    {
        ActivatePanel(index);
    }

    public void ActivatePanel(int index)
    {
        // ��� �г� ��Ȱ��ȭ
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // ������ �гθ� Ȱ��ȭ
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
        }
    }
}

