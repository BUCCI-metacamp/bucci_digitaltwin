using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ControlPanel : MonoBehaviour
{
    [Header("AMachine")]
    public TMP_InputField[] AFields; // 인풋 필드 배열    
    public Slider[] ASliders; // 슬라이더 배열
    public Image AIMG; // 변경할 이미지
    public Sprite AFSprite;  // 포커스가 갔을 때의 이미지
    public Sprite AUSprite; // 포커스가 해제되었을 때의 이미지

    [Header("BMachine")]
    public TMP_InputField[] BFields; // 인풋 필드 배열
    public Slider[] BSliders; // 슬라이더 배열
    public Image BIMG; // 변경할 이미지
    public Sprite BFSprite;  // 포커스가 갔을 때의 이미지
    public Sprite BUSprite; // 포커스가 해제되었을 때의 이미지

    [Header("CMachine")]
    public TMP_InputField[] CFields; // 인풋 필드 배열
    public Slider[] CSliders; // 슬라이더 배열
    public Image CIMG; // 변경할 이미지
    public Sprite CFSprite;  // 포커스가 갔을 때의 이미지
    public Sprite CUSprite; // 포커스가 해제되었을 때의 이미지

    [Header("Conveyor")]
    public TMP_InputField[] ConFields; // 인풋 필드 배열
    public Slider[] ConSliders; // 슬라이더 배열
    public Image ConIMG; // 변경할 이미지
    public Sprite ConFSprite;  // 포커스가 갔을 때의 이미지
    public Sprite ConUSprite; // 포커스가 해제되었을 때의 이미지

    [Header("Robot")]
    public TMP_InputField[] RFields; // 인풋 필드 배열
    public Slider[] RSliders; // 슬라이더 배열
    public Image RIMG; // 변경할 이미지
    public Sprite RFSprite;  // 포커스가 갔을 때의 이미지
    public Sprite RUSprite; // 포커스가 해제되었을 때의 이미지



    void Start()
    {
        // 각 인풋 필드 및 슬라이더 배열에 대해 이벤트 트리거 설정
        SetupEventTriggers(AFields, ASliders, AIMG, AFSprite, AFSprite);
        SetupEventTriggers(BFields, BSliders, BIMG, BFSprite, BFSprite);
        SetupEventTriggers(CFields, CSliders, CIMG, CFSprite, CFSprite);
        SetupEventTriggers(ConFields, ConSliders, ConIMG, ConFSprite, ConFSprite);
        SetupEventTriggers(RFields, RSliders, RIMG, RFSprite, RFSprite);
    }

    void SetupEventTriggers(TMP_InputField[] inputFields, Slider[] sliders, Image img, Sprite focusedSprite, Sprite unfocusedSprite)
    {
        foreach (var inputField in inputFields)
        {
            AddEventTrigger(inputField.gameObject, img, focusedSprite, unfocusedSprite);
        }

        foreach (var slider in sliders)
        {
            AddEventTrigger(slider.gameObject, img, focusedSprite, unfocusedSprite);
        }
    }

    void AddEventTrigger(GameObject target, Image img, Sprite focusedSprite, Sprite unfocusedSprite)
    {
        EventTrigger eventTrigger = target.AddComponent<EventTrigger>();

        // Select 이벤트 추가
        EventTrigger.Entry selectEntry = new EventTrigger.Entry();
        selectEntry.eventID = EventTriggerType.Select;
        selectEntry.callback.AddListener((eventData) => { OnInputFieldSelect(img, focusedSprite); });
        eventTrigger.triggers.Add(selectEntry);

        // Deselect 이벤트 추가
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry();
        deselectEntry.eventID = EventTriggerType.Deselect;
        deselectEntry.callback.AddListener((eventData) => { OnInputFieldDeselect(img, unfocusedSprite); });
        eventTrigger.triggers.Add(deselectEntry);
    }

    // 인풋 필드 또는 슬라이더에 포커스가 갔을 때 호출될 메서드
    private void OnInputFieldSelect(Image img, Sprite focusedSprite)
    {
        if (img != null && focusedSprite != null)
        {
            img.sprite = focusedSprite;
            Color color = img.color;
            color.a = 1f; // 알파 값을 설정하여 투명도를 조정
            img.color = color;
        }
    }

    // 인풋 필드 또는 슬라이더에서 포커스가 해제되었을 때 호출될 메서드
    private void OnInputFieldDeselect(Image img, Sprite unfocusedSprite)
    {
        if (img != null && unfocusedSprite != null)
        {
            img.sprite = unfocusedSprite;
            Color color = img.color;
            color.a = 0.1f; // 알파 값을 설정하여 투명도를 조정
            img.color = color;
        }
    }
}
