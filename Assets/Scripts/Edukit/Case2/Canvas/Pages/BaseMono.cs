
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMono<T> : MonoBehaviour where T : MonoBehaviour
{
    CanvasGroup canvasgroup { get; set; }
    Transform transfromThis { get; set; }
    public Transform Root { get { return CanvasGroup.transform; } } 

    public virtual bool Activity
    {
        get
        {  
            return CanvasGroup.gameObject.activeSelf;
        }
        set
        {
            CanvasGroup.gameObject.SetActive(value);
            if (value == true) OnStart();
            else OnFinish();
        }
    }
    public virtual void Show(bool isShow)
    {
        Alpha = isShow == true ? 1.0f : 0.0f;
        CanvasGroup.blocksRaycasts = isShow == true;
        CameraMove.Instance.IsFullBlockZone = isShow;
    }
    public virtual void OnStart()
    {
    }
    public virtual void OnFinish()
    {
    }
    public virtual void OnBackAndroid()
    {
    }
    public SpellEffect SpellEffect
    {
        get { return CanvasGroup.GetComponent<SpellEffect>(); } 
    }
    public CanvasGroup CanvasGroup
    {
        get
        {   if (canvasgroup == null)
               canvasgroup = gameObject.GetComponentInChildren<CanvasGroup>(true);
            
            return canvasgroup;
        }
        protected set
        {   
            if (canvasgroup == null)
                canvasgroup = gameObject.GetComponentInChildren<CanvasGroup>(true);

            canvasgroup = value;
        }
    }
    public Transform Trans
    {
        get
        {   if (transfromThis == null)
                transfromThis = gameObject.transform;
            return transfromThis;
        }
    }
    public float Alpha
    {
        get { return CanvasGroup.alpha; }
        set 
        {   CanvasGroup.alpha = value;
            CanvasGroup.blocksRaycasts = (value != 0.0f);
        }
    }
    static T instance { get; set; }
    public static T Instance
    {
        get
        {   if (instance == null)
            {   var listObjects = Resources.FindObjectsOfTypeAll<T>();

                for( int i=0;i< listObjects.Length; ++i  )
                {   if(listObjects[i].gameObject.scene.isLoaded == true )
                        instance = listObjects[i];
                }
            }
            return instance;
        }
    }
    public void LateCall( float time, UnityAction callFunction)
    {
        TimeCall.Instance.AddEvent(time, callFunction);
    }
}
public abstract class BaseMonoBehave : MonoBehaviour
{
    CanvasGroup canvasgroup { get; set; }
    Transform transfromThis { get; set; }
    GameObject Root { get; set; }

    public virtual bool Activity
    {
        get
        {
            return CanvasGroup.gameObject.activeSelf;
        }
        set
        {
            CanvasGroup.gameObject.SetActive(value);
        }
    }
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasgroup == null)
                canvasgroup = gameObject.GetComponentInChildren<CanvasGroup>(true);

            return canvasgroup;
        }
        protected set
        {
            if (canvasgroup == null)
                canvasgroup = gameObject.GetComponentInChildren<CanvasGroup>(true);

            canvasgroup = value;
        }
    }
    public Transform Trans
    {
        get
        {
            if (transfromThis == null)
                transfromThis = gameObject.transform;
            return transfromThis;
        }
    }
    public float Alpha
    {
        get { return CanvasGroup.alpha; }
        set { CanvasGroup.alpha = value; }
    }
}

