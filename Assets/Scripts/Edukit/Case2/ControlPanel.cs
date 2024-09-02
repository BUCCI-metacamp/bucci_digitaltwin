using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ControlPanel : MonoBehaviour
{
    [Header("AMachine")]
    public TMP_InputField[] AFields; // ��ǲ �ʵ� �迭    
    public Slider[] ASliders; // �����̴� �迭
    public Image AIMG; // ������ �̹���
    public Sprite AFSprite;  // ��Ŀ���� ���� ���� �̹���
    public Sprite AUSprite; // ��Ŀ���� �����Ǿ��� ���� �̹���

    [Header("BMachine")]
    public TMP_InputField[] BFields; // ��ǲ �ʵ� �迭
    public Slider[] BSliders; // �����̴� �迭
    public Image BIMG; // ������ �̹���
    public Sprite BFSprite;  // ��Ŀ���� ���� ���� �̹���
    public Sprite BUSprite; // ��Ŀ���� �����Ǿ��� ���� �̹���

    [Header("CMachine")]
    public TMP_InputField[] CFields; // ��ǲ �ʵ� �迭
    public Slider[] CSliders; // �����̴� �迭
    public Image CIMG; // ������ �̹���
    public Sprite CFSprite;  // ��Ŀ���� ���� ���� �̹���
    public Sprite CUSprite; // ��Ŀ���� �����Ǿ��� ���� �̹���

    [Header("Conveyor")]
    public TMP_InputField[] ConFields; // ��ǲ �ʵ� �迭
    public Slider[] ConSliders; // �����̴� �迭
    public Image ConIMG; // ������ �̹���
    public Sprite ConFSprite;  // ��Ŀ���� ���� ���� �̹���
    public Sprite ConUSprite; // ��Ŀ���� �����Ǿ��� ���� �̹���

    [Header("Robot")]
    public TMP_InputField[] RFields; // ��ǲ �ʵ� �迭
    public Slider[] RSliders; // �����̴� �迭
    public Image RIMG; // ������ �̹���
    public Sprite RFSprite;  // ��Ŀ���� ���� ���� �̹���
    public Sprite RUSprite; // ��Ŀ���� �����Ǿ��� ���� �̹���



    void Start()
    {
        // �� ��ǲ �ʵ� �� �����̴� �迭�� ���� �̺�Ʈ Ʈ���� ����
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

        // Select �̺�Ʈ �߰�
        EventTrigger.Entry selectEntry = new EventTrigger.Entry();
        selectEntry.eventID = EventTriggerType.Select;
        selectEntry.callback.AddListener((eventData) => { OnInputFieldSelect(img, focusedSprite); });
        eventTrigger.triggers.Add(selectEntry);

        // Deselect �̺�Ʈ �߰�
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry();
        deselectEntry.eventID = EventTriggerType.Deselect;
        deselectEntry.callback.AddListener((eventData) => { OnInputFieldDeselect(img, unfocusedSprite); });
        eventTrigger.triggers.Add(deselectEntry);
    }

    // ��ǲ �ʵ� �Ǵ� �����̴��� ��Ŀ���� ���� �� ȣ��� �޼���
    private void OnInputFieldSelect(Image img, Sprite focusedSprite)
    {
        if (img != null && focusedSprite != null)
        {
            img.sprite = focusedSprite;
            Color color = img.color;
            color.a = 1f; // ���� ���� �����Ͽ� ������ ����
            img.color = color;
        }
    }

    // ��ǲ �ʵ� �Ǵ� �����̴����� ��Ŀ���� �����Ǿ��� �� ȣ��� �޼���
    private void OnInputFieldDeselect(Image img, Sprite unfocusedSprite)
    {
        if (img != null && unfocusedSprite != null)
        {
            img.sprite = unfocusedSprite;
            Color color = img.color;
            color.a = 0.1f; // ���� ���� �����Ͽ� ������ ����
            img.color = color;
        }
    }
}
