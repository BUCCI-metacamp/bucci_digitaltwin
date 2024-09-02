using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMessage : BaseMono<PageMessage>
{
    [SerializeField]
    AnyObject textCustomMessage;

    [SerializeField]
    AnyObject textTitleMessage;

    [SerializeField]
    AnimationCurve curveShowHide;

    //bskim servomotor
    public float Ax1_Pos;//bskim Servomotor
    public int Ax1_upLimit;
    public int Ax1_downLimit;
    public int Ax1_orgLimit;
    public int Ax2_upLimit;
    public int Ax2_downLimit;
    public int Ax2_orgLimit;


    public int[] AX1_Servo_XGPM = new int[20];
    public int[] AX2_Servo_XGPM = new int[20];
    //bskim servomotor

    public bool LMSCHECK;

    void Start()
    {
        Activity = false;
    }
    public void Show( string msgTitle, string msgContents, int Switch = 0, float timeElapse = 1.0f)
    {
        Activity = true;
        textCustomMessage.Text = msgTitle;
        textTitleMessage.Text = msgContents;

        StopAllCoroutines();
        if(Switch == 0)
            StartCoroutine(coShowAndHide(timeElapse));
        if(Switch == 1)
            StartCoroutine(coShowAndHide2());
        
    }
    IEnumerator coShowAndHide( float time )
    {
        var startAlpha = Alpha;
        var currentAlpha = startAlpha;
        var startTime = Time.time;
        var currentTime = 0.0f;
        var lastTime = curveShowHide.keys[curveShowHide.keys.Length - 1].time;

        while (currentTime < lastTime)
        {
            currentTime = Time.time - startTime;
            var alpha = curveShowHide.Evaluate(currentTime+1);
            Alpha = Mathf.Lerp(startAlpha, 1.0f, alpha);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(time);        

        startAlpha = Alpha;
        currentAlpha = startAlpha;
        startTime = Time.time;
        currentTime = 0.0f;

        while (currentTime < lastTime)
        {
            currentTime = Time.time - startTime;
            var alpha = curveShowHide.Evaluate(currentTime);
            Alpha = Mathf.Lerp(startAlpha, 0.0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        Activity = false;
    }
    IEnumerator coShowAndHide2()
    {
    Debug.Log("메세지가 들어왔습니다.");
        var startAlpha = Alpha;
        var currentAlpha = startAlpha;
        var startTime = Time.time;
        var currentTime = 0.0f;
        var lastTime = curveShowHide.keys[curveShowHide.keys.Length - 1].time;

        while (currentTime < lastTime)
        {
            currentTime = Time.time - startTime;
            var alpha = curveShowHide.Evaluate(currentTime + 1);
            Alpha = Mathf.Lerp(startAlpha, 1.0f, alpha);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(100);
        
        startAlpha = Alpha;
        currentAlpha = startAlpha;
        startTime = Time.time;
        currentTime = 0.0f;
        StopAllCoroutines();
        while (currentTime < lastTime)
        {
            currentTime = Time.time - startTime;
            var alpha = curveShowHide.Evaluate(currentTime);
            Alpha = Mathf.Lerp(startAlpha, 0.0f, alpha);
            yield return new WaitForEndOfFrame();
        }

        Activity = false;
    }

}
