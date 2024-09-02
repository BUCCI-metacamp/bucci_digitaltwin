using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePopupSave : BaseMono<PagePopupSave>
{
    [SerializeField]
    UIButton btnExit;

    [SerializeField]
    UIButton btnContinue;

    [SerializeField]
    UIButton btnOk;

    [SerializeField]
    UIButton btnCancel;

    [SerializeField]
    AnyObject textCustomMessage;

    [SerializeField]
    AnyObject textTitleMessage;

    public System.Action funcExit = delegate() { };
    public System.Action funcOk = delegate () { };    
    public System.Action funcCancel = delegate () { };
    public System.Action funcDefault { get; set; } = delegate () { };

    public List<AnyObject> objectMessages;

    void Start()
    {
        btnExit.callbackDown = delegate (UIButton btn)
        {
            Activity = false;
            btn.SpellComponent.ScaleEffect();
            funcExit.Invoke();
        };        
        btnOk.callbackDown = delegate (UIButton btn)
        {
            Activity = false;
            btn.SpellComponent.ScaleEffect();
            funcOk.Invoke();
        };
        btnCancel.callbackDown = delegate (UIButton btn)
        {
            Activity = false;
            btn.SpellComponent.ScaleEffect();
            funcCancel.Invoke();
        };
        btnContinue.callbackDown = delegate (UIButton btn)
        {
            Activity = false;
            btn.SpellComponent.ScaleEffect();
            funcOk.Invoke();
        };

        Activity = false;
    }
    public void Show( string msgTitle, string msgContents, System.Action argfuncOk, System.Action argfuncFail, System.Action argfuncCancle)
    {
        funcOk = argfuncOk;
        funcCancel = argfuncFail;        
        funcDefault = argfuncFail;
        funcExit = argfuncFail;
        Activity = true;

        btnOk.Activity = true;
        btnCancel.Activity = true;
        btnContinue.Activity = true;

        textTitleMessage.Text = msgTitle;
        textCustomMessage.Text = msgContents;    
    }
    public void ShowOk(string msgTitle, string msgContents, System.Action argfuncOk, System.Action argfuncFail)
    {
        funcOk = argfuncOk;
        funcCancel = argfuncFail;
        funcDefault = argfuncFail;
        funcExit = argfuncFail;
        Activity = true;

        btnOk.Activity = false;
        btnCancel.Activity = false;
        btnContinue.Activity = true;

        textTitleMessage.Text = msgTitle;
        textCustomMessage.Text = msgContents;
    }
    public void SelectOneObject(AnyObject objctSelect )
    {
        for( int i=0;i< objectMessages.Count; ++i )
            objectMessages[i].Activity = (objectMessages[i] == objctSelect);
    }
    public void OnBackKey()
    {
        funcDefault.Invoke();
        Activity = false;
    }
    public override void OnStart()
    {
        
    }
}
