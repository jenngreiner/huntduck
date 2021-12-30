using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    public Vector3 movePopup = new Vector3(0f, 0.1f, 0f);
    public float moveSpeedFast = 60f;
    public float moveSpeedSlow = 8f;
    public float scaleAmount = 1f;

    public float disappearTimerMax = 1f;
    public float disappearSpeed = 3f;
    private float disappearTimer = 1f;

    private Text scoreText;
    private Color textColor;

    // dropdown to select effect you want
    public enum EffectType { ScaleAndShrink, ScalePauseFade, FloatAndFade }
    public EffectType effectType;

    public delegate void SelectedDelegate();
    SelectedDelegate effectMethod;

    void Start()
    {
        //Destroy(gameObject, disappearTimer);

        scoreText = GetComponentInChildren<Text>();
        textColor = scoreText.color;
        disappearTimer = disappearTimerMax;
        movePopup *= moveSpeedFast;
        Debug.Log("movePopup + moveSpeedFast is " + movePopup);
        RunEffectSelected(effectType);
    }

    void Update()
    {
        effectMethod();
    }

    void RunEffectSelected(EffectType _effectType)
    {
        switch (effectType)
        {
            case EffectType.ScaleAndShrink:
                effectMethod = ScaleAndShrink;
                break;
            case EffectType.ScalePauseFade:
                effectMethod = ScalePauseFade;
                break;
            case EffectType.FloatAndFade:
                effectMethod = FloatAndFade;
                break;
            default:
                break;
        }
    }

    void ScaleAndShrink()
    {
        transform.position += movePopup * Time.deltaTime;
        movePopup -= movePopup * moveSpeedSlow * Time.deltaTime;
        Debug.Log("movePopup - movePopup * moveSpeedSlow is " + movePopup);

        if (disappearTimer > disappearTimerMax / 2)
        {
            // first half of popup lifetime
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        }
        else 
        {
            // second half of popup lifetime
            transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }

    void ScalePauseFade()
    {
        //transform.position += movePopup * Time.deltaTime;
        //movePopup -= movePopup * moveSpeedSlow * Time.deltaTime;
        //Debug.Log("movePopup - movePopup * moveSpeedSlow is " + movePopup);

        if (disappearTimer > disappearTimerMax / 2)
        {
            // scale up, more quickly at first then slower
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
            scaleAmount -= scaleAmount * moveSpeedSlow * Time.deltaTime;
        }
        //else if (disappearTimer < disappearTimerMax * 2/3 && disappearTimer > disappearTimerMax * 1 / 3)
        //{
        //    transform.position += movePopup * Time.deltaTime;
        //    movePopup -= movePopup * moveSpeedSlow * Time.deltaTime;
        //}
        else
        {
            // move up
            transform.position += movePopup * Time.deltaTime;

            // disappear
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;

            if (textColor.a <= 0)
            {
                // if completely faded, destroy
                Destroy(gameObject);
            }
        }

        disappearTimer -= Time.deltaTime;
    }

    void FloatAndFade()
    {
        //transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        transform.position += movePopup * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // 16:45 sort order https://www.youtube.com/watch?v=iD1_JczQcFY&t=514s
}
