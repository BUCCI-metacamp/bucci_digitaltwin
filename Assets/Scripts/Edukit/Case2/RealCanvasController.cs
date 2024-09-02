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

        public event Action SettingStart; //SettingView에 값 넣기

        public static CanvasController Instance { get; private set; }
        void Start()
        {
            // 캔버스 그룹 설정
            //canvasGroup = GetComponent<CanvasGroup>();
            settingCanvas.alpha = 0f; // 캔버스 보이기
            settingCanvas.interactable = false; // UI 요소와 상호작용 허용
            controllCanvas.interactable = true;
            controllCanvas.gameObject.SetActive(true);
            settingCanvas.blocksRaycasts = false; // 캔버스 뒤의 클릭 차단
        }

        public void ShowCanvas()
        {
            settingCanvas.alpha = 1f; // 캔버스 보이기
            settingCanvas.interactable = true; // UI 요소와 상호작용 허용
            settingCanvas.blocksRaycasts = true; // 캔버스 뒤의 클릭 차단
            controllCanvas.interactable = false;
            controllCanvas.gameObject.SetActive(false);


            // 초기 투명도 설정 (0은 완전히 투명, 1은 완전히 불투명)
            SetCanvasTransparency(0.9f);
            SettingStart?.Invoke();
        }

        public void HideCanvas()
        {
            settingCanvas.alpha = 0f; // 캔버스 숨기기
            settingCanvas.interactable = false; // UI 요소와 상호작용 비허용
            settingCanvas.blocksRaycasts = false; // 캔버스 뒤의 클릭 허용
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

