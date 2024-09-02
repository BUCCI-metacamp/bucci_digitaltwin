using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;



namespace WI
{
    public class RealCanvasController : MonoBehaviour
    {
        public CanvasGroup settingCanvas;
        public CanvasGroup controllCanvas;

        public event Action SettingStart; //SettingView�� �� �ֱ�

        public static CanvasController Instance { get; private set; }
        void Start()
        {
            // ĵ���� �׷� ����
            //canvasGroup = GetComponent<CanvasGroup>();
            settingCanvas.alpha = 0f; // ĵ���� ���̱�
            settingCanvas.interactable = false; // UI ��ҿ� ��ȣ�ۿ� ���
            controllCanvas.interactable = true;
            controllCanvas.gameObject.SetActive(true);
            settingCanvas.blocksRaycasts = false; // ĵ���� ���� Ŭ�� ����
        }

        public void ShowCanvas()
        {
            settingCanvas.alpha = 1f; // ĵ���� ���̱�
            settingCanvas.interactable = true; // UI ��ҿ� ��ȣ�ۿ� ���
            settingCanvas.blocksRaycasts = true; // ĵ���� ���� Ŭ�� ����
            controllCanvas.interactable = false;
            controllCanvas.gameObject.SetActive(false);


            // �ʱ� ���� ���� (0�� ������ ����, 1�� ������ ������)
            SetCanvasTransparency(0.9f);
            SettingStart?.Invoke();
        }

        public void HideCanvas()
        {
            settingCanvas.alpha = 0f; // ĵ���� �����
            settingCanvas.interactable = false; // UI ��ҿ� ��ȣ�ۿ� �����
            settingCanvas.blocksRaycasts = false; // ĵ���� ���� Ŭ�� ���
            controllCanvas.interactable = true;
            controllCanvas.gameObject.SetActive(true);

        }

        public void SetCanvasTransparency(float alpha)
        {
            if (settingCanvas != null)
            {
                settingCanvas.alpha = alpha;
            }
        }
    }
}

