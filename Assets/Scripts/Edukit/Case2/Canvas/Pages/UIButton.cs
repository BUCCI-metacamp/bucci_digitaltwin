using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public enum ButtonKind
    {
        zero = 0, one = 1, two = 2, three = 3, four = 4, five = 5,
        On = 10, Off,
    }

    [SerializeField]
    ButtonKind kind;

    [SerializeField]
    List<Image> images;

    [SerializeField]
    List<Text> texts;

    [SerializeField]
    AudioManager.KindOfAudio audioClickDown;

    [SerializeField]
    float DownDelay = 0.2f;

    public Text textTitle;
    CanvasGroup canvasGr { get { return gameObject.GetComponentInChildren<CanvasGroup>(true); } }

    public float Alpha
    { get { return canvasGr.alpha; }
        set { canvasGr.alpha = value; }
    }
    public bool Activity
    { get { return canvasGr.gameObject.activeSelf; }
        set { canvasGr.gameObject.SetActive(value); }
    }
    //public  float TimeDown { get; set; }
    public float TimeDown { get; set; }
    public ButtonKind Value { get; set; }
    public bool IsPointed { get; private set; }
    public int ValueInt { get; set; }
    public bool IsEnable { get; set; }
    public ButtonKind Kind { get { return kind; } }
    

    public PointerEventData EventData { get; private set; }
    public Image ImageComponent {  get { return gameObject.GetComponentInChildren<Image>(); } }
    public SpellEffect SpellComponent { get { return gameObject.GetComponentInChildren<SpellEffect>(); } }
    public Animator AnimatorComponent { get { return gameObject.GetComponentInChildren<Animator>(); } }
    //public OnOffEffect OnoffComponent { get { return gameObject.GetComponentInChildren<OnOffEffect>(); } }
    //public FloatCircle FloatComponent { get { return gameObject.GetComponentInChildren<FloatCircle>(); } }

    public void InvokeDown() { /*ClickDownSound();*/  callbackDown.Invoke(this);  }
    public void InvokeUp() { callbackUp.Invoke( this ); }

    public UnityAction<UIButton>  callbackEnter { get; set; } = delegate (UIButton btn) { };
    public UnityAction<UIButton> callbackExit { get; set; } = delegate (UIButton btn) { };
    public UnityAction<UIButton> callbackDown { get; set; } = delegate (UIButton btn) { };    
    public UnityAction<UIButton> callbackUp { get; set; } = delegate (UIButton btn) { };
    public int SelectIndex { get; set; }

    public void Update()
    {
        
        //textTitle.text = 
    }

    void Awake()
    {
        IsEnable = true;
        IsPointed = false;
    }
    public void TimeDelayReset()
    {
        TimeDown = Time.time;
    }
    public void SetIndex(int index)
    {
        if (images == null) return;
        SelectIndex = index;

        for (int i = 0; i < images.Count; ++i)
            images[i].enabled = (i == index);


        if (texts == null) return;
        SelectIndex = index;

        for (int i = 0; i < texts.Count; ++i)
            texts[i].enabled = (i == index);
    }
    public void Show( bool isShow )
    {
        if (images == null) return;
        for (int i = 0; i < images.Count; ++i)
            images[i].enabled = isShow;

        if (texts == null) return;
        for (int i = 0; i < texts.Count; ++i)
            texts[i].enabled = isShow;
    }
    public void OnPointerUp(PointerEventData evdata)
    {   
        if (IsEnable == false)
            return;
       
        EventData = evdata;
        callbackUp(this);
    }
    public void OnPointerDown(PointerEventData evdata)
    {
        if (IsEnable == false)
            return;
       
        if ( Time.time < TimeDown + DownDelay)
            return;

        ClickDownSound();

        TimeDown = Time.time;
        EventData = evdata;
        callbackDown(this);       
    }
    public void ResetTimeDelay()
    {
        TimeDown = 0.0f;
    }
    public void ClickDownSound()
    {
        if (audioClickDown != AudioManager.KindOfAudio.None)
            AudioManager.Instance.PlaySfx(audioClickDown);
    }
    public void OnPointerEnter(PointerEventData evdata)
    {
        IsPointed = true;
        EventData = evdata;        
        callbackEnter(this);
    }
    public void OnPointerExit(PointerEventData evdata)
    {
        IsPointed = false;
        EventData = evdata;      
        callbackExit(this);
    }
}
