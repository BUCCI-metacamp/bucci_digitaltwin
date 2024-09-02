using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;


// This is Wrapper
public class AnyObject : MonoBehaviour
{    
    public string Name { get { return gameObject.name; } }
    public string DataDesc { get; set; }
    public string DataTitle { get; set; }
    public int DataIndex { get; set; }

    public Image ImageComponent
    {
        get { return GetComponent<Image>(); }
    }
    public RawImage RawImageComponent
    {
        get { return GetComponent<RawImage>(); }
    }
    public Text TextComponent
    {
        get { return GetComponent<Text>(); }
    }
    public Slider SliderComponent
    {
        get { return GetComponent<Slider>(); }
    }
    public Button ButtonComponent
    {
        get { return GetComponent<Button>(); }
    }
    public InputField InputFieldComponent
    {
        get { return GetComponent<InputField>(); }
    }
    public CanvasGroup CanvasgroupComonent
    {
        get { return GetComponent<CanvasGroup>(); }
    }
    public bool Activity
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
    public float CavasAlpha
    {
        get { return CanvasgroupComonent.alpha; }
        set { CanvasgroupComonent.alpha = value; }
    }
    public string Text
    {
        get { return TextComponent.text;  }
        set { TextComponent.text = value; }
    }
    public string InputText
    {
        get { return InputFieldComponent.text; }
        set { InputFieldComponent.text = value; }
    }
    public string InputPlaceText
    {
        get { return (InputFieldComponent.placeholder as Text).text ; }
        set { (InputFieldComponent.placeholder as Text).text = value; }
    }
    public int TextInt
    {
        get { return int.Parse(TextComponent.text); }
        set { TextComponent.text = value.ToString(); }
    }
    public Color Color
    {
        get { return gameObject.GetComponent<Graphic>().color; }
        set
        {
            gameObject.GetComponent<Graphic>().color = value;
        }
    }
    public float Alpha
    {
        get { return gameObject.GetComponent<Graphic>().color.a; }
        set {
                var graphic = gameObject.GetComponent<Graphic>();
                var color = gameObject.GetComponent<Graphic>().color;
                color.a = value;
                graphic.color = color;
            }
    }
    public float SpriteAlpha
    {
        get { return gameObject.GetComponent<SpriteRenderer>().color.a; }
        set
        {   var sprenderer = gameObject.GetComponent<SpriteRenderer>();
            var color = sprenderer.color;
            color.a = value;
            sprenderer.color = color;
        }
    }
    public float FillAmount
    {
        get { return ImageComponent.fillAmount; }
        set { ImageComponent.fillAmount = value; }
    }
    public float SliderValue
    {
        get { return SliderComponent.value; }
        set { SliderComponent.value = value; }
    }
    public string Animation
    {
        set { GetComponent<Animator>().Play(value); }
    }

    public bool ToggleOn
    {
        set { GetComponent<Toggle>().isOn = value; }
    }
    public bool Interactable
    {
        set
        {
            //if (GetComponent<Button>() != null)
            //    GetComponent<Button>().interactable = value;
            //else
                GetComponentInChildren<Button>().GetComponent<Image>().raycastTarget = value;
        }
    }

    public UnityEngine.Events.UnityAction AddClickEvent
    {
        set
        {
            if (GetComponent<Button>() != null)
                GetComponent<Button>().onClick.AddListener(value);
            else
                GetComponentInChildren<Button>().onClick.AddListener(value);
        }
    }
    public UnityEngine.Events.UnityAction ClickEvent
    {
        set
        {
            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().onClick.RemoveAllListeners();
                GetComponent<Button>().onClick.AddListener(value);
            }
            else
            {
                GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                GetComponentInChildren<Button>().onClick.AddListener(value);
            }
        }
    }
    public void ClickInvoke()
    {
        GetComponentInChildren<Button>().onClick.Invoke();
    }
    public void PlayAnimation( string name, float time = 0.0f )
    {
        GetComponentInChildren<Animator>().Play(name, -1, time);
    }
    public UnityEngine.Events.UnityAction<bool> ChangeEvent
    {
        set
        {
            if (GetComponent<Toggle>() != null)
                GetComponent<Toggle>().onValueChanged.AddListener(value);
            else
                GetComponentInChildren<Toggle>().onValueChanged.AddListener(value);
        }
    }
    public UnityEngine.Events.UnityAction<float> SliderChangeEvent
    {
        set
        {
            if (GetComponent<Slider>() != null)
                GetComponent<Slider>().onValueChanged.AddListener(value);
            else
                GetComponentInChildren<Slider>().onValueChanged.AddListener(value);
        }
    }
    public void SetAsLastSibling()
    {
        if (GetComponent<RectTransform>() != null)
            GetComponent<RectTransform>().SetAsLastSibling();
    }


}
