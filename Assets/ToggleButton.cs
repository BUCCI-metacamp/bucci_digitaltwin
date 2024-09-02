using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleButton : MonoBehaviour
{
    public Button toggleButton;
    public TextMeshProUGUI buttonText;

    private bool isOn = false;

    void Start()
    {
        toggleButton.onClick.AddListener(Toggle);
        UpdateButtonText();
    }

    void Toggle()
    {
        isOn = !isOn;
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        ColorBlock colors = toggleButton.colors;
        buttonText.text = isOn ? "On" : "Off";
        if(buttonText.text == "On")
        {
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.red;
            colors.pressedColor = Color.red;
            colors.selectedColor = Color.red;
        }
        else
        {
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            colors.pressedColor = Color.white;
            colors.selectedColor = Color.white;
        }
        toggleButton.colors = colors;

        // 버튼의 색상 변경 등 추가 작업도 여기에 포함할 수 있습니다.
    }
}
