using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Modal : BaseMono<Modal>
{
    public Button OKButton;
    public TextMeshProUGUI TEXT;
   

    private void Start()
    {
        
       
    }
    // 모달 창을 보여주는 함수
    public void ShowModal(string message)
    {
        // 모달 창의 Text 설정
        TEXT.text = message;

        // 모달 창을 활성화
        gameObject.SetActive(true);

        // OK 버튼에 클릭 이벤트 추가
        OKButton.onClick.AddListener(() => OnOKButtonClicked());
    }

    private void OnOKButtonClicked()
    {
        // 모달 창 비활성화
        gameObject.SetActive(false);
        // 클릭 이벤트 제거
        OKButton.onClick.RemoveAllListeners();
    }
}
