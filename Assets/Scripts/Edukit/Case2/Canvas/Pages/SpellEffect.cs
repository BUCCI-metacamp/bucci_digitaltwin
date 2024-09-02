using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEffect : MonoBehaviour 
{   
    public AnimationCurve curveSpellMoveEffect;
	public AnimationCurve curveAlphaEffect;
	public AnimationCurve curveScaleEffect;
    public CanvasGroup rootEffect;
    public Transform positionTarget;

    Timer.Element currentMove = null;
    Timer.Element currentScale = null;
    Timer.Element currentAlpha = null;
    //Timer.Element timerCount = null;
    Vector3 positionStart { get; set; } = Vector3.one;
    System.Action EffectComplete { get; set; }

    public bool Activity
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }
    public float Scale
    {
        get { return transform.localPosition.x; }
        set { transform.localScale = new Vector3(value, value, 1.0f); }
    }
    public float Alpha
    {
        get { return rootEffect.alpha; }
        set { rootEffect.alpha = value; }
    }
    

    // 투명효과 담당
    public void AlphaCurve( CanvasGroup target, bool isReverse = false )
    {
		if (currentMove != null)
			currentMove.Remove();

        if (currentAlpha != null)
            currentAlpha.Remove();

        currentAlpha = Timer.Instance.CurveSingle(curveAlphaEffect,
		delegate (Timer.Element time, float value)
		{
            if(isReverse == true )
                target.alpha = 1.0f - value;
            else
                target.alpha = value;
        },
		delegate (Timer.Element time, int count)
		{
            currentAlpha = null;

            if (EffectComplete != null)
                EffectComplete.Invoke();

        });
	}
    public void AlphaCurveRepeat(CanvasGroup target, bool isReverse = false)
    {
        if (currentMove != null)
            currentMove.Remove();

        if (currentAlpha != null)
            currentAlpha.Remove();

        currentAlpha = Timer.Instance.CurveRepeat(curveAlphaEffect,
        delegate (Timer.Element time, float value)
        {
            if (isReverse == true)
                target.alpha = 1.0f - value;
            else
                target.alpha = value;
        },
        delegate (Timer.Element time, int count) { });
    }
    // 이동효과 담당
    public void MoveCurve(CanvasGroup target)
    {
        if (currentMove != null)
            currentMove.Remove();

        if (positionStart == Vector3.one)
            positionStart = target.transform.localPosition;

        currentMove = Timer.Instance.CurveSingle(curveSpellMoveEffect,
        delegate (Timer.Element time, float value)
        {
            //Debug.Log("Start==>" + value + "/Pos:" + Vector3.Lerp(positionStart, positionTarget.localPosition, value) +"/Target:" + positionTarget.localPosition +"/" + positionStart);
            target.transform.localPosition = Vector3.Lerp(positionStart, positionTarget.localPosition, value);
        },
        delegate (Timer.Element time, int count)
        {
            currentMove = null;
        });
    }
    // 스캐일효과담당
    public void ScaleCurve(CanvasGroup target)
	{
		if (currentScale != null)
			currentScale.Remove();

		currentScale = Timer.Instance.CurveSingle(curveScaleEffect,
		delegate (Timer.Element time, float value)
		{
			var offsetScale = Vector3.one * value;
			target.transform.localScale = offsetScale;
		},
		delegate (Timer.Element time, int count)
		{
            currentScale = null;
        });
	}
	public void Play(string name)
	{
		var anim = GetComponent<Animator>();
		anim.Play(name, -1, 0.0f);
	}
    public void PositionEffect()
    {
        MoveCurve(rootEffect);
    }
    public void ScaleEffect()
    {
        ScaleCurve(rootEffect);
    }
    public void AlphaEffect( System.Action effectComplete = null )
    {
        EffectComplete = effectComplete;
        AlphaCurve(rootEffect);        
    }
    public void AlphaRepeatffect(System.Action effectComplete = null)
    {
        EffectComplete = effectComplete;
        AlphaCurveRepeat(rootEffect);        
    }
    public void ReverseAlphaEffect(System.Action effectComplete = null)
    {
        EffectComplete = effectComplete;
        AlphaCurve(rootEffect, true);
    }
    public void SetAlphaZero()
    {
        Alpha = 0.0f;
        if (currentAlpha != null)
            currentAlpha.Remove();
    }
    public void AlphaEffectRepeat()
    {
        AlphaCurveRepeat(rootEffect );
    }
    public void Idle()
	{
		Play("anim_idle");
	}

}
